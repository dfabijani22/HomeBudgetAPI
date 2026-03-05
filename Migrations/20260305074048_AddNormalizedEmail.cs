using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeBudgetAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddNormalizedEmail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NormalizedEmail",
                table: "Users",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");
        }
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NormalizedEmail",
                table: "Users");
        }

    }
}
