namespace HomeBudgetAPI.DTOs.Category
{
    public class CategoryRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsDefault { get; set; }
    }
}
