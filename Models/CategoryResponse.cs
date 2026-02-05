namespace HomeBudgetAPI.Models
{
    public class CategoryResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public int? CategoryId { get; set; }
    }
}
