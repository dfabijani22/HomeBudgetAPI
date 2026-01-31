using HomeBudgetAPI.Data;
using HomeBudgetAPI.Models;

namespace HomeBudgetAPI.Services
{
    public class ExpenseService: IExpenseService
    {
        private readonly ApplicationDbContext _context;

        public ExpenseService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ExpenseResponse> AddExpenseAsync(ExpenseRequest expenseRequest, int userId)
        {
            if (expenseRequest.Amount <= 0)
            {
                return new ExpenseResponse { Success = false, Message = "Iznos mora biti pozitivan broj." };
            }

            var category = await _context.Categories.FindAsync(expenseRequest.CategoryId);
            if (category == null) 
            {
                return new ExpenseResponse { Success = false, Message = "Kategorija nije pronađena." };
            }

            var expense = new Expense {
                Name = expenseRequest.Name,
                Amount = expenseRequest.Amount,
                Date = expenseRequest.Date,
                CategoryId = expenseRequest.CategoryId,
                UserId = userId };

            _context.Expenses.Add(expense);
            await _context.SaveChangesAsync();

            return new ExpenseResponse { Success = true, Message = "Trošak uspješno dodan.", ExpenseId = expense.Id };
            
        }
    }
}
