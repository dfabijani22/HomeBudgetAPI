namespace HomeBudgetAPI.Models
{
    public class MonthlyBudget
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
