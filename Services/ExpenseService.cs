using AutoMapper;
using AutoMapper.QueryableExtensions;
using HomeBudgetAPI.Data;
using HomeBudgetAPI.DTOs;
using HomeBudgetAPI.DTOs.Common;
using HomeBudgetAPI.DTOs.Expense;
using HomeBudgetAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace HomeBudgetAPI.Services
{
    public class ExpenseService: IExpenseService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public ExpenseService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ApiResponse<ExpenseResponse>> AddExpenseAsync(ExpenseRequest request, int userId)
        {
            if (request.Amount <= 0)
            {
                return new ApiResponse<ExpenseResponse> { Success = false, Message = "Iznos mora biti pozitivan broj."};
            }

            var category = await _context.Categories.FindAsync(request.CategoryId);
            if (category == null) 
            {
                return new ApiResponse<ExpenseResponse> { Success = false, Message = "Kategorija nije pronađena." };
            }

            var expense = _mapper.Map<Expense>(request);
            expense.UserId = userId;
            try
            {
                _context.Expenses.Add(expense);
                await _context.SaveChangesAsync();
                var response = _mapper.Map<ExpenseResponse>(expense);

                return new ApiResponse<ExpenseResponse> { Success = true, Message = "Trošak uspješno dodan.", Data = response };
            }

            catch (Exception ex)
            {
                return new ApiResponse<ExpenseResponse> { Success = false, Message = "Trošak nije uspješno dodan."};
            }

        }

        public async Task<ApiResponse<List<ExpenseResponse>>> GetUserExpenses(int userId, int? month = null, int? year = null, int? categoryId = null)
        {
            var query = _context.Expenses
                .Include(e => e.Category)
                .Where(e => e.UserId == userId);

            if (month.HasValue)
            {
                query = query.Where(e => e.Date.Month == month.Value);
            }
            if (year.HasValue)
            {
                query = query.Where(e => e.Date.Year == year.Value);
            }

            if (categoryId.HasValue && categoryId.Value != 0)
            {
                query = query.Where(e => e.CategoryId == categoryId.Value);
            }

            var expenses = await query
                .OrderByDescending(e => e.Date)
                .ProjectTo<ExpenseResponse>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return new ApiResponse<List<ExpenseResponse>>
            {
                Success = true,
                Message = "Lista troškova uspješno dohvaćena.",
                Data = expenses
            };

        }
        public async Task<ApiResponse<ExpenseResponse>> UpdateExpenseAsync(int userId, int expenseId, ExpenseRequest request)
        {


            var expense = await _context.Expenses
                .FirstOrDefaultAsync(e => e.Id == expenseId && e.UserId == userId);

            if (expense == null)
            {
                return new ApiResponse<ExpenseResponse>
                {
                    Success = false,
                    Message = "Trošak nije pronađen ili ne pripada korisniku."
                };
            }

            if (request.Amount != default && request.Amount <= 0)
            {
                return new ApiResponse<ExpenseResponse>
                {
                    Success = false,
                    Message = "Iznos mora biti pozitivan broj."
                };
            }

            if (request.CategoryId != default)
            {
                var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == request.CategoryId && (c.UserId == userId || c.IsDefault));

                if (category == null)
                {
                    return new ApiResponse<ExpenseResponse>
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
            var response = _mapper.Map<ExpenseResponse>(expense);
            return new ApiResponse<ExpenseResponse>
            {
                Success = true,
                Message = "Trošak uspješno izmijenjen.",
                Data = response
            };

        }
        public async Task<ApiResponse<ExpenseResponse>> DeleteExpenseAsync(int userId, int expenseId)
        {
            var expense = await _context.Expenses
                .FirstOrDefaultAsync(e => e.Id == expenseId && e.UserId == userId);

            if (expense == null)
            {
                return new ApiResponse<ExpenseResponse>
                {
                    Success = false,
                    Message = "Trošak nije pronađen ili ne pripada korisniku."
                };
            }

            _context.Expenses.Remove(expense);
            await _context.SaveChangesAsync();

            var response = _mapper.Map<ExpenseResponse>(expense);
            return new ApiResponse<ExpenseResponse>
            {
                Success = true,
                Message = "Trošak je uspješno obrisan.",
                Data = response
            };
        }

        public async Task<ApiResponse<ExpenseResponse>> GetExpenseByIdAsync(int userId, int expenseId)
        {

            var expense = await _context.Expenses
                            .Where(e => e.Id == expenseId && e.UserId == userId)
                            .ProjectTo<ExpenseResponse>(_mapper.ConfigurationProvider)
                            .FirstOrDefaultAsync();

            if (expense == null)
            {
                return new ApiResponse<ExpenseResponse>
                {
                    Success = false,
                    Message = "Trošak nije pronađen.",
                    Data = null
                };
            }

            return new ApiResponse<ExpenseResponse>
            {
                Success = true,
                Message = "Trošak uspješno dohvaćen.",
                Data = expense
            };
        }
    }
}
