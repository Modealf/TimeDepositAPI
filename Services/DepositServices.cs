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
        public Task<string> RequestCustomDepositAsync(int userId, CreateCustomDepositRequestDto request);
        public Task<IEnumerable<Deposit>> GetUserDepositsAsync(int userId);
        public Task<Deposit> GetDepositByIdAsync(int depositId, int userId);
        public Task<IEnumerable<CrowdDepositOffer>> GetAvailableCrowdDepositsAsync();
        public Task<string> EnrollInCrowdDepositAsync(int userId, EnrollCrowdDepositRequestDto request);
    }

    public class DepositService : IDepositService
    {
        private readonly AppDbContext _context;

        public DepositService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<string> RequestCustomDepositAsync(int userId, CreateCustomDepositRequestDto request)
        {
            User? user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

            if (user is null)
                throw new ApplicationException("User not found");

            var customDepositRequest = new CustomDepositRequest
            {
                Amount = request.Amount,
                DurationInDays = request.DurationInDays,
                User = user,
                UserId = user.Id,
            };
            // should await for both lines or only one?
            await _context.CustomDepositRequests.AddAsync(customDepositRequest);
            await _context.SaveChangesAsync();
            return "Your deposit request has been created";
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

        public async Task<IEnumerable<CrowdDepositOffer>> GetAvailableCrowdDepositsAsync()
        {
            DateTime now = DateTime.UtcNow;
            var deposits = await _context.CrowdDepositOffers.Where(d => d.StartDate < now).ToListAsync();
            return deposits;
        }

        public async Task<string> EnrollInCrowdDepositAsync(int userId, EnrollCrowdDepositRequestDto request)
        {
            User? user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user is null)
                throw new BadHttpRequestException("User not found");

            var crowdDepositOffer = await _context.CrowdDepositOffers.FirstOrDefaultAsync(d => d.Id == request.OfferId);

            if (crowdDepositOffer is null)
                throw new BadHttpRequestException("Crowd deposit offer not found");

            if (request.Amount > user.Balance)
                throw new BadHttpRequestException("Insufficient funds for this deposit please charge " +
                                                  (request.Amount - user.Balance));

            user.Balance -= request.Amount;

            var deposit = new Deposit
            {
                UserId = userId,
                Amount = request.Amount,
                Type = DepositType.Crowd,
                Period = crowdDepositOffer.Period,
                StartDate = crowdDepositOffer.StartDate,
                MaturityDate = crowdDepositOffer.MaturityDate,
                APY = crowdDepositOffer.APY,
                RolloverUntil = request.RolloverUntilDate,
                User = user
            };
            // user.Deposits.Add(deposit);
            await _context.Deposits.AddAsync(deposit);
            await _context.SaveChangesAsync();
            return "successfully enrolled in crowd deposit";
        }
    }
}