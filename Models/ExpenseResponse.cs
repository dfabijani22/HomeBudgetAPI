namespace HomeBudgetAPI.Models
{
    public class ExpenseResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public int? ExpenseId { get; set; }
    }
}
