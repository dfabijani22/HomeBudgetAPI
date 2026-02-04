using HomeBudgetAPI.Models;

namespace HomeBudgetAPI.Services
{
    public interface IAuthService
    {
        Task<AuthResponse> RegisterUserAsync(RegisterRequest request);
        Task<LoginResponse> LoginUserAsync(LoginRequest request);

    }
}
