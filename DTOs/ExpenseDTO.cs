namespace HomeBudgetAPI.DTOs
{
    public class ExpenseDTO
    {
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string CategoryName { get; set; }
    }
}
