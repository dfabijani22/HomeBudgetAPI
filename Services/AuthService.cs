using HomeBudgetAPI.Data;
using HomeBudgetAPI.Models;
using Microsoft.AspNetCore.Identity;

namespace HomeBudgetAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;
        public AuthService(ApplicationDbContext context, IPasswordHasher<User> passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }
        public async Task<AuthResponse> RegisterUserAsync(RegisterRequest request)
        {
            if (request.Password != request.ConfirmPassword)
            {
                return new AuthResponse { Success = false, Message = "Lozinke se ne podudaraju." };
            }

            if(_context.Users.Any(u => u.Email == request.Email))
            {
                return new AuthResponse { Success = false, Message = "Korisnik s tom email adresom već postoji." };
            }

            var user = new User
            {
                Email = request.Email,
                Name = request.FirstName,
                Surname = request.LastName
            };

            user.PasswordHash = _passwordHasher.HashPassword(user, request.Password);
            
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return new AuthResponse { Success = true, Message = "Korisnik uspješno registriran." };
        }
    }
}
