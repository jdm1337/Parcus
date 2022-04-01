using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Parcus.Persistence.Data.Migrations
{
    public partial class transactionsinportfoliochanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Amount",
                table: "InvestTransactions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "InstrumentsInPortfolio",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Tiker",
                table: "InstrumentsInPortfolio",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Amount",
                table: "InvestTransactions");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "InstrumentsInPortfolio");

            migrationBuilder.DropColumn(
                name: "Tiker",
                table: "InstrumentsInPortfolio");
        }
    }
}
