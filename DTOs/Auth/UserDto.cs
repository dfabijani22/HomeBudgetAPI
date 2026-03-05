namespace HomeBudgetAPI.DTOs.Auth
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Email { get; set; } = default!;
        public string? Name { get; set; }
        public string? Surname { get; set; }
    }
}
