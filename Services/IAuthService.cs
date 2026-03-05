using HomeBudgetAPI.DTOs.Auth;
using HomeBudgetAPI.DTOs.Common;

namespace HomeBudgetAPI.Services
{
    public interface IAuthService
    {
        Task<ApiResponse<UserDto>> RegisterUserAsync(RegisterRequest request);
        Task<ApiResponse<LoginResultDto>> LoginUserAsync(LoginRequest request);

    }
}
