using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TimeDepositAPI.Data;
using TimeDepositAPI.DTOs;
using TimeDepositAPI.Models;

namespace TimeDepositAPI.Services
{
    public interface IDepositService
    {
        Task<Deposit> CreateCustomDepositAsync(int userId, CreateCustomDepositRequestDto request);
        Task<IEnumerable<Deposit>> GetUserDepositsAsync(int userId);
        Task<Deposit> GetDepositByIdAsync(int depositId, int userId);
        // Add methods for enrolling in crowd deposits, rollovers, etc.
        Task<IEnumerable<CrowdDepositOffer>> GetAvailableCrowdDeposits();
    }

    public class DepositService : IDepositService
    {
        private readonly AppDbContext _context;

        public DepositService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Deposit> CreateCustomDepositAsync(int userId, CreateCustomDepositRequestDto request)
        {
            // Basic validation & business logic could go here

            // Example APY calculation logic (simplified; adjust as needed)
            double calculatedApy = CalculateApy(request.Amount, request.DurationInDays);
            User? user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

            if (user is null)
                throw new ApplicationException("User not found");
            
            // Create new deposit
            var deposit = new Deposit
            {
                UserId = userId,
                Amount = request.Amount,
                Type = DepositType.Customized,
                StartDate = DateTime.UtcNow,
                MaturityDate = DateTime.UtcNow.AddDays(request.DurationInDays),
                APY = calculatedApy,
                RolloverUntil = null,
                User = user
            };

            _context.Deposits.Add(deposit);
            await _context.SaveChangesAsync();

            return deposit;
        }

        public async Task<IEnumerable<Deposit>> GetUserDepositsAsync(int userId)
        {
            return await _context.Deposits
                .Where(d => d.UserId == userId)
                .ToListAsync();
        }

        public async Task<Deposit> GetDepositByIdAsync(int depositId, int userId)
        {
            return await _context.Deposits
                .FirstAsync(d => d.Id == depositId && d.UserId == userId);
        }

        public async Task<IEnumerable<CrowdDepositOffer>> GetAvailableCrowdDeposits()
        {
            DateTime now = DateTime.UtcNow;
            var deposits= await _context.CrowdDepositOffers.Where(d => d.StartDate < now).ToListAsync();
            return deposits;
        }

        // Placeholder for APY calculation logic
        private double CalculateApy(decimal amount, int durationInDays)
        {
            
            // Implement your APY logic based on business requirements
            return 5.0; // Example APY
        }
    }
}
