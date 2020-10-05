using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PaymentGateway.Data.Migrations
{
    public partial class AddCardNumberBytes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "CardNumber",
                table: "Payments",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CardNumber",
                table: "Payments");
        }
    }
}
