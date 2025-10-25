using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using backend.Data;
using backend.DTOs;
using backend.Helpers;
using backend.Models;

namespace backend.Services
{
    public class UserService
    {
        private readonly AppDbContext _db;
        private readonly IPasswordHasher<User> _hasher;
        private readonly JwtHelper _jwtHelper;

        public UserService(AppDbContext db, IPasswordHasher<User> hasher, JwtHelper jwtHelper)
        {
            _db = db;
            _hasher = hasher;
            _jwtHelper = jwtHelper;
        }

        public async Task<string?> RegisterAsync(RegisterDto dto)
        {
            if (await _db.Users.AnyAsync(u => u.Username == dto.Username))
                return null;

            var user = new User
            {
                Username = dto.Username,
                PasswordHash = _hasher.HashPassword(null!, dto.Password)
            };

            _db.Users.Add(user);
            await _db.SaveChangesAsync();
            return _jwtHelper.GenerateToken(user);
        }

        public async Task<string?> LoginAsync(LoginDto dto)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Username == dto.Username);
            if (user == null) return null;

            var result = _hasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);
            if (result == PasswordVerificationResult.Failed) return null;

            return _jwtHelper.GenerateToken(user);
        }
    }
}
