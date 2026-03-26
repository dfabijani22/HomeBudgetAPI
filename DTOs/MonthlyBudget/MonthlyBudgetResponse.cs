namespace HomeBudgetAPI.DTOs.MonthlyBudget
{
    public class MonthlyBudgetResponse
    {
        public int Id { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public decimal Amount { get; set; }

    }
}
