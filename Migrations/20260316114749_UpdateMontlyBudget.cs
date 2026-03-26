using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeBudgetAPI.Migrations
{
    /// <inheritdoc />
    public partial class UpdateMontlyBudget : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "MonthlyBudgets");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "MonthlyBudgets");

            migrationBuilder.AddColumn<int>(
                name: "Month",
                table: "MonthlyBudgets",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Year",
                table: "MonthlyBudgets",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Month",
                table: "MonthlyBudgets");

            migrationBuilder.DropColumn(
                name: "Year",
                table: "MonthlyBudgets");

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "MonthlyBudgets",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "MonthlyBudgets",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
