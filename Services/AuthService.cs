using AutoMapper;
using HomeBudgetAPI.Data;
using HomeBudgetAPI.DTOs.Auth;
using HomeBudgetAPI.DTOs.Common;
using HomeBudgetAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.SqlServer.Server;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HomeBudgetAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        public AuthService(ApplicationDbContext context, IPasswordHasher<User> passwordHasher, IConfiguration configuration, IMapper mapper)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _configuration = configuration;
            _mapper = mapper;
        }
        public async Task<ApiResponse<UserDto>> RegisterUserAsync(RegisterRequest request)
        {
            if (request.Password != request.ConfirmPassword)
            {
                return new ApiResponse<UserDto> 
                { Success = false, 
                  Message = "Lozinke se ne podudaraju." 
                };
            }

            var email = request.Email?.Trim();
            var normalizedEmail = email?.ToUpperInvariant();

            var exists = await _context.Users
                .AnyAsync(u => u.NormalizedEmail == normalizedEmail);

            if (exists)
            {
                return new ApiResponse<UserDto>
                {
                    Success = false,
                    Message = "Korisnik s tom email adresom već postoji."
                };
            }

            var user = _mapper.Map<User>(request);
            user.Email = email;
            user.NormalizedEmail = normalizedEmail;
            user.PasswordHash = _passwordHasher.HashPassword(user, request.Password);
            
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return new ApiResponse<UserDto> 
            {   
                Success = true, 
                Message = "Korisnik uspješno registriran.",
                Data = _mapper.Map<UserDto>(user)
            };
        }

        public async Task<ApiResponse<LoginResultDto>> LoginUserAsync(LoginRequest request)
        {
            var normalizedEmail = request.Email.Trim().ToUpperInvariant();
            var user = await _context.Users.AsNoTracking()
                .FirstOrDefaultAsync(u => u.NormalizedEmail == normalizedEmail);

            if (user == null)
            {
                return new ApiResponse<LoginResultDto>
                {
                    Success = false,
                    Message = "Uneseni su krivi podaci, pokušajte ponovno"
                };
            }

            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);

            if (result == PasswordVerificationResult.Failed)
            {
                return new ApiResponse<LoginResultDto>
                {
                    Success = false,
                    Message = "Uneseni su krivi podaci, pokušajte ponovno"
                };
            }

            var token = GenerateJwtToken(user);

            return new ApiResponse<LoginResultDto>
            {
                Success = true,
                Message = "Uspješna prijava.",
                Data = new LoginResultDto
                {
                    Token = token,
                    UserId = user.Id,
                    User = _mapper.Map<UserDto>(user)
                }
                
            };
        }

        private string GenerateJwtToken(User user)
        {

            var keyStr = _configuration["Jwt:Key"];
            var issuer = _configuration["Jwt:Issuer"];
            var audience = _configuration["Jwt:Audience"];

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyStr));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var now = DateTime.UtcNow;

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat,
                    new DateTimeOffset(now).ToUnixTimeSeconds().ToString(),
                    ClaimValueTypes.Integer64),

                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email)
            };

            var token = new JwtSecurityToken(
                issuer,
                audience,
                claims,
                expires: now.AddDays(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

}
