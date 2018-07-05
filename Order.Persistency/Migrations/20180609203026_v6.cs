using Microsoft.EntityFrameworkCore.Migrations;

namespace Order.Persistency.Migrations
{
    public partial class v6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SubTotalAmount",
                schema: "orders",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "TaxValue",
                schema: "orders",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "TotalAmount",
                schema: "orders",
                table: "Order");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "SubTotalAmount",
                schema: "orders",
                table: "Order",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "TaxValue",
                schema: "orders",
                table: "Order",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "TotalAmount",
                schema: "orders",
                table: "Order",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
