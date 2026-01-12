namespace HomeBudgetAPI.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsDefault { get; set; }
        public int? UserId { get; set; }
        public User? User { get; set; }
        public ICollection<Expense> Expenses { get; set; }
    }
}
