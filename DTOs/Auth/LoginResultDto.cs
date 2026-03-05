namespace HomeBudgetAPI.DTOs.Auth
{
    public class LoginResultDto
    {
        public string Token { get; set; } = default!;
        public int UserId { get; set; }
        public UserDto User { get; set; } = default!;
    }
}
