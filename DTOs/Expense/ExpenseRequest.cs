namespace HomeBudgetAPI.DTOs.Expense
{
    public class ExpenseRequest
    {
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public int CategoryId { get; set; }
    }
}
