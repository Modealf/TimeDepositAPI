using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using TimeDepositAPI.Data;
using TimeDepositAPI.DTOs;
using TimeDepositAPI.Models;

namespace TimeDepositAPI.Services
{
    public interface ITopUpService
    {
        Task<string> TopUpAccountAsync(int userId, TopUpRequestDto request);
        Task<decimal> GetWalletBalanceAsync(int userId);
    }

    public class TopUpService : ITopUpService
    {
        private readonly AppDbContext _context;

        public TopUpService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<string> TopUpAccountAsync(int userId, TopUpRequestDto request)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Id == userId);
            user.Balance += request.Amount;
            await _context.SaveChangesAsync();
            return "Account Successfully Topped Up, New Balance is " + user.Balance;
        }

        public async Task<decimal> GetWalletBalanceAsync(int userId)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Id == userId);
            return user.Balance;
        }
    }
}
