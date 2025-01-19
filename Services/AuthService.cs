using System.Threading.Tasks;
using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using TimeDepositAPI.Data;
using TimeDepositAPI.DTOs;
using TimeDepositAPI.Models;

namespace TimeDepositAPI.Services
{
    public interface IAuthService
    {
        Task<string> RegisterAsync(RegisterRequestDto request);
        Task<string> LoginAsync(LoginRequestDto request);
    }

    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly IJwtTokenService _jwtTokenService;

        public AuthService(AppDbContext context, IJwtTokenService jwtTokenService)
        {
            _context = context;
            _jwtTokenService = jwtTokenService;
        }

        public async Task<string> RegisterAsync(RegisterRequestDto request)
        {
            if (await _context.Users.AnyAsync(u => u.Email == request.Email))
            {
                throw new System.Exception("User with this email already exists.");
            }

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);

            var user = new User
            {
                Email = request.Email,
                PasswordHash = hashedPassword,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Role = UserRole.User,
                Deposits = new List<Deposit>() // Initialize the Deposits property
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return "Registration successful";
        }

        public async Task<string> LoginAsync(LoginRequestDto request)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == request.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                throw new System.Exception("Invalid credentials");
            }

            var token = _jwtTokenService.GenerateToken(user);
            return token;
        }
    }
}
