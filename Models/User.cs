using System.ComponentModel.DataAnnotations;

namespace HomeBudgetAPI.Models
{
    public class User
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Surname { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; } = string.Empty;
        public ICollection<Expense> Expenses { get; set; }
    }
}
