using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Parcus.Persistence.Data.Migrations
{
    public partial class modifypreviouschanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BrokeragePortfolios_Broker_PortfolioBrokerId",
                table: "BrokeragePortfolios");

            migrationBuilder.AlterColumn<int>(
                name: "PortfolioBrokerId",
                table: "BrokeragePortfolios",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "BrokeragePortfolios",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddForeignKey(
                name: "FK_BrokeragePortfolios_Broker_PortfolioBrokerId",
                table: "BrokeragePortfolios",
                column: "PortfolioBrokerId",
                principalTable: "Broker",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BrokeragePortfolios_Broker_PortfolioBrokerId",
                table: "BrokeragePortfolios");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "BrokeragePortfolios");

            migrationBuilder.AlterColumn<int>(
                name: "PortfolioBrokerId",
                table: "BrokeragePortfolios",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_BrokeragePortfolios_Broker_PortfolioBrokerId",
                table: "BrokeragePortfolios",
                column: "PortfolioBrokerId",
                principalTable: "Broker",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
