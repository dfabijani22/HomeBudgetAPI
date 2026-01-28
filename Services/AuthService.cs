using HomeBudgetAPI.Data;
using HomeBudgetAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace HomeBudgetAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IConfiguration _configuration;
        public AuthService(ApplicationDbContext context, IPasswordHasher<User> passwordHasher, IConfiguration configuration)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _configuration = configuration;
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

        public async Task<LoginResponse> LoginUserAsync(LoginRequest request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);

            if (user == null)
            {
                return new LoginResponse
                {
                    Success = false,
                    Message = "Uneseni su krivi podaci, pokušajte ponovno"
                };
            }

            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);

            if (result == PasswordVerificationResult.Failed)
            {
                return new LoginResponse
                {
                    Success = false,
                    Message = "Uneseni su krivi podaci, pokušajte ponovno"
                };
            }

            var token = GenerateJwtToken(user);

            return new LoginResponse
            {
                Success = true,
                Message = "Uspješna prijava.",
                Token = token,
                UserId = user.Id
                
            };
        }

        private string GenerateJwtToken(User user)
        {
            var claims = new[]
            {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email)
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
