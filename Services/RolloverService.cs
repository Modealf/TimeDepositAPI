using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TimeDepositAPI.Data;
using TimeDepositAPI.Models;
using System.Threading.Tasks;
using Hangfire;
using TimeDepositAPI.DTOs;
using TimeDepositAPI.Services;

namespace TimeDepositAPI.Services
{
    public interface IRolloverService
    {
        Task ProcessMaturedDepositsAsync();
    }


    public class RolloverService : IRolloverService
    {
        private readonly AppDbContext _context;
        private readonly ITopUpService _topUpService;
        private readonly IBackgroundJobClient _backgroundJobClient;

        public RolloverService(AppDbContext context, ITopUpService topUpService, IBackgroundJobClient backgroundJobClient)
        {
            _context = context;
            _topUpService = topUpService;
            _backgroundJobClient = backgroundJobClient;
        }

        public async Task ProcessMaturedDepositsAsync()
        {
            var now = DateTime.UtcNow;

            // Fetch all deposits that have matured and are still active
            var maturedDeposits = await _context.Deposits
                .Include(d => d.User)
                //should add  condition
                .Where(d => d.MaturityDate <= now )
                .ToListAsync();

            foreach (var deposit in maturedDeposits)
            {
                // Mark the deposit as completed
                deposit.Status = Status.Completed;

                // Calculate profit
                decimal profit = CalculateProfit(deposit);

                if (deposit.RolloverUntil.HasValue && deposit.RolloverUntil > now)
                {
                    // Rollover is enabled; perform rollover logic
                    Console.WriteLine($"Deposit ID {deposit.Id} for User ID {deposit.UserId} has been rolled over.");
                    var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == deposit.UserId);
                    // Example: Create a new deposit for rollover
                    var newDeposit = new Deposit
                    {
                        UserId = deposit.UserId,
                        Amount = deposit.Amount + profit,
                        Type = deposit.Type,
                        StartDate = now,
                        MaturityDate = GetNextMaturityDate(deposit),
                        APY = deposit.APY,
                        RolloverUntil = deposit.RolloverUntil, // Retain rollover settings
                        Status = Status.InProgress,
                        User = user!
                    };

                    _context.Deposits.Add(newDeposit);
                }
                else
                {
                    // Rollover not enabled; transfer to wallet
                    decimal totalAmount = deposit.Amount + profit;
                    await _topUpService.TopUpAccountAsync(deposit.UserId, new TopUpRequestDto { Amount = totalAmount });
                }
            }

            await _context.SaveChangesAsync();
        }

        private decimal CalculateProfit(Deposit deposit)
        {
            // Simplified profit calculation based on APY and duration
            // APY is Annual Percentage Yield
            double durationInDays = (deposit.MaturityDate - deposit.StartDate).TotalDays;
            decimal profit = deposit.Amount * (decimal)(deposit.APY / 100) * (decimal)(durationInDays / 365);
            return profit;
        }

        private DateTime GetNextMaturityDate(Deposit deposit)
        {
            if (deposit.Period == Period.Daily)
                return deposit.StartDate.AddDays(1);
            
            if (deposit.Period == Period.Weekly)
                return deposit.StartDate.AddDays(7);
            
            return deposit.MaturityDate.AddMonths(1);
        }
    }
}