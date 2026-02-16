using HomeBudgetAPI.Data;
using HomeBudgetAPI.DTOs;
using HomeBudgetAPI.Models;
using Microsoft.EntityFrameworkCore;

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

        public async Task<List<ExpenseDTO>> GetUserExpenses(int userId, int? month = null, int? categoryId = null)
        {
            var query = _context.Expenses
                .Include(e => e.Category)
                .Where(e => e.UserId == userId);

            if (month.HasValue)
            {
                query = query.Where(e => e.Date.Month == month.Value);
            }

            if (categoryId.HasValue && categoryId.Value != 0)
            {
                query = query.Where(e => e.CategoryId == categoryId.Value);
            }
            

                return await query
                    .OrderByDescending(e => e.Date)
                    .Select(e => new ExpenseDTO
                    {
                        Name = e.Name,
                        Amount = e.Amount,
                        Date = e.Date,
                        CategoryId = e.Category.Id
                    })
                    .ToListAsync();
        }
        public async Task<ExpenseResponse> UpdateExpenseAsync(int userId, int expenseId, ExpenseRequest request)
        {


            var expense = await _context.Expenses
                .FirstOrDefaultAsync(e => e.Id == expenseId && e.UserId == userId);

            if (expense == null)
            {
                return new ExpenseResponse
                {
                    Success = false,
                    Message = "Trošak nije pronađen ili ne pripada korisniku."
                };
            }

            if (request.Amount != default && request.Amount <= 0)
            {
                return new ExpenseResponse
                {
                    Success = false,
                    Message = "Iznos mora biti pozitivan broj."
                };
            }

            if (request.CategoryId != default)
            {
                var category = await _context.Categories.FindAsync(request.CategoryId);
                if (category == null)
                {
                    return new ExpenseResponse
                    {
                        Success = false,
                        Message = "Kategorija nije pronađena."
                    };
                }
            }

            if (!string.IsNullOrWhiteSpace(request.Name))
                expense.Name = request.Name;

            if (request.Amount != default)
                expense.Amount = request.Amount;

            if (request.Date != default)
                expense.Date = request.Date;

            if (request.CategoryId != default)
                expense.CategoryId = request.CategoryId;

            await _context.SaveChangesAsync();

            return new ExpenseResponse
            {
                Success = true,
                Message = "Trošak uspješno izmijenjen.",
                ExpenseId = expense.Id
            };

        }
        public async Task<ExpenseResponse> DeleteExpenseAsync(int userId, int expenseId)
        {
            var expense = await _context.Expenses
                .FirstOrDefaultAsync(e => e.Id == expenseId && e.UserId == userId);

            if (expense == null)
            {
                return new ExpenseResponse
                {
                    Success = false,
                    Message = "Trošak nije pronađen ili ne pripada korisniku."
                };
            }

            _context.Expenses.Remove(expense);
            await _context.SaveChangesAsync();

            return new ExpenseResponse
            {
                Success = true,
                Message = "Trošak je uspješno obrisan.",
                ExpenseId = expense.Id
            };
        }
    }
}
