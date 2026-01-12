using HomeBudgetAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace HomeBudgetAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<Expense> Expenses => Set<Expense>();
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<MonthlyBudget> MonthlyBudgets => Set<MonthlyBudget>();
        public DbSet<User> Users => Set<User>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Hrana", IsDefault = true, Description = "Troškovi za prehranu - trgovina, restorani, dostava hrane, kafici" },
                new Category { Id = 2, Name = "Stanarina", IsDefault = true, Description = "Mjesecni trošak za nekretninu - stanarina, kredit..." },
                new Category { Id = 3, Name = "Režije", IsDefault = true, Description = "Mjesečni racuni: struja, voda, grijanje, internet, komunalije" },
                new Category { Id = 4, Name = "Prijevoz", IsDefault = true, Description = "Gorivo, javni prijevoz, parking, taksi, odrzavanje vozila" },
                new Category { Id = 5, Name = "Osobno", IsDefault = true, Description = "Hobi, treninzi, obrazovanje, knjige..." }
                );

            modelBuilder.Entity<MonthlyBudget>()
                .Property(e => e.Amount)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Expense>()
                .Property(e => e.Amount)
                .HasColumnType("decimal(18,2)");
        }
    }

    
}
