using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Parcus.Persistence.Data.Migrations
{
    public partial class cascade : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InvestTransactions_BrokeragePortfolios_BrokeragePortfolioId",
                table: "InvestTransactions");

            migrationBuilder.AddForeignKey(
                name: "FK_InvestTransactions_BrokeragePortfolios_BrokeragePortfolioId",
                table: "InvestTransactions",
                column: "BrokeragePortfolioId",
                principalTable: "BrokeragePortfolios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InvestTransactions_BrokeragePortfolios_BrokeragePortfolioId",
                table: "InvestTransactions");

            migrationBuilder.AddForeignKey(
                name: "FK_InvestTransactions_BrokeragePortfolios_BrokeragePortfolioId",
                table: "InvestTransactions",
                column: "BrokeragePortfolioId",
                principalTable: "BrokeragePortfolios",
                principalColumn: "Id");
        }
    }
}
