namespace HomeBudgetAPI.Models
{
    public class LoginResponse
    {
        public string Token { get; set; }
        public string Message { get; set; }
        public bool Success { get; set; }
        public int UserId { get; set; }
    }
}
