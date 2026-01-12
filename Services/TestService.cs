namespace HomeBudgetAPI.Services
{
    public interface ITestService
    {
        string GetStatus();
    }
    public class TestService : ITestService
    {
        public string GetStatus() => "Backend radi";
    }
}
