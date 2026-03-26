namespace HomeBudgetAPI.DTOs.MonthlyBudget
{
    public class MonthlyBudgetRequest
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public decimal Amount { get; set; }

    }
}
