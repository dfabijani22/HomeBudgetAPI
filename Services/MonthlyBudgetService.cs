using AutoMapper;
using HomeBudgetAPI.Data;
using HomeBudgetAPI.DTOs.Common;
using HomeBudgetAPI.DTOs.MonthlyBudget;
using HomeBudgetAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace HomeBudgetAPI.Services
{
    public class MonthlyBudgetService : IMonthlyBudgetService
    {

        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public MonthlyBudgetService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ApiResponse<MonthlyBudgetResponse?>> GetBudgetAsync(int year, int month, int userId)
        {

            var budget = await _context.MonthlyBudgets
                        .FirstOrDefaultAsync(b => b.Year == year && b.Month == month && b.UserId == userId);

            if (budget != null)
            {
                return new ApiResponse<MonthlyBudgetResponse?>
                {
                    Success = true,
                    Data = _mapper.Map<MonthlyBudgetResponse>(budget)
                };
            }

            var last = await _context.MonthlyBudgets
                .Where(b => b.UserId == userId)
                .OrderByDescending(b => b.Year)
                .ThenByDescending(b => b.Month)
                .FirstOrDefaultAsync();

            if (last == null)
            {
                return new ApiResponse<MonthlyBudgetResponse?>
                {
                    Success = true,
                    Data = null,
                    Message = "Nije pronađen budžet za prošle mjesece."
                };
            }

            return new ApiResponse<MonthlyBudgetResponse?>
            {
                Success = true,
                Data = _mapper.Map<MonthlyBudgetResponse>(last),
                Message = "Ne postoji budđet za ovaj mjesec. Koristi se zadnje poznati budžet."
            };
        }

        public async Task<ApiResponse<MonthlyBudgetResponse>> SetBudgetAsync(MonthlyBudgetRequest request, int userId)
        {
            var existing = await _context.MonthlyBudgets
                    .FirstOrDefaultAsync(b => b.Year == request.Year
                                           && b.Month == request.Month
                                           && b.UserId == userId);

            MonthlyBudget budget;

            if (existing == null)
            {
                budget = _mapper.Map<MonthlyBudget>(request);
                budget.UserId = userId;

                _context.MonthlyBudgets.Add(budget);
            }
            else
            {
                existing.Amount = request.Amount;
                budget = existing; 
            }

            await _context.SaveChangesAsync();

            var response = _mapper.Map<MonthlyBudgetResponse>(budget);

            return new ApiResponse<MonthlyBudgetResponse>
            {
                Success = true,
                Message = "Budžet uspješno postavljen.",
                Data = response
            };

        }

    }
}
