using Microsoft.EntityFrameworkCore.Migrations;

namespace PaymentGateway.Data.Migrations
{
    public partial class RemoveCardNumber : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CardNumber",
                table: "Payments");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CardNumber",
                table: "Payments",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
