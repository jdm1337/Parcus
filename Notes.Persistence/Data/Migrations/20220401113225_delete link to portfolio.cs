using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Parcus.Persistence.Data.Migrations
{
    public partial class deletelinktoportfolio : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InstrumentsInPortfolio_AspNetUsers_UserId",
                table: "InstrumentsInPortfolio");

            migrationBuilder.DropIndex(
                name: "IX_InstrumentsInPortfolio_UserId",
                table: "InstrumentsInPortfolio");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_InstrumentsInPortfolio_UserId",
                table: "InstrumentsInPortfolio",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_InstrumentsInPortfolio_AspNetUsers_UserId",
                table: "InstrumentsInPortfolio",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
