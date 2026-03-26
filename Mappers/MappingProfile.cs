using AutoMapper;
using HomeBudgetAPI.DTOs.Auth;
using HomeBudgetAPI.DTOs.Category;
using HomeBudgetAPI.DTOs.Expense;
using HomeBudgetAPI.DTOs.MonthlyBudget;
using HomeBudgetAPI.Models;

namespace HomeBudgetAPI.Mappers
{
    public class MappingProfile : Profile
    {

        public MappingProfile()
        {
            CreateMap<Category, CategoryResponse>();
            CreateMap<CategoryRequest, Category>()
                .ForMember(d => d.Id, o => o.Ignore())
                .ForMember(d => d.UserId, o => o.Ignore())
                .ForMember(d => d.IsDefault, o => o.Ignore());

            CreateMap<Expense, ExpenseResponse>();
            CreateMap<ExpenseRequest, Expense>()
                .ForMember(d => d.Id, o => o.Ignore())
                .ForMember(d => d.UserId, o => o.Ignore());


            CreateMap<User, UserDto>();

            CreateMap<RegisterRequest, User>()
                .ForMember(d => d.Id, o => o.Ignore())
                .ForMember(d => d.PasswordHash, o => o.Ignore())
                .ForMember(d => d.NormalizedEmail, o => o.Ignore());


            CreateMap<MonthlyBudget, MonthlyBudgetResponse>();
            CreateMap<MonthlyBudgetRequest, MonthlyBudget>();

        }

    }
}
