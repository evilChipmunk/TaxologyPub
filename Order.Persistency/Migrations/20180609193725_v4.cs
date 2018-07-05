using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Order.Persistency.Migrations
{
    public partial class v4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Money",
                schema: "orders");

            migrationBuilder.RenameColumn(
                name: "Amount",
                schema: "orders",
                table: "Product",
                newName: "Price");

            migrationBuilder.AddColumn<double>(
                name: "SubTotal",
                schema: "orders",
                table: "Order",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Tax",
                schema: "orders",
                table: "Order",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Total",
                schema: "orders",
                table: "Order",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SubTotal",
                schema: "orders",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "Tax",
                schema: "orders",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "Total",
                schema: "orders",
                table: "Order");

            migrationBuilder.RenameColumn(
                name: "Price",
                schema: "orders",
                table: "Product",
                newName: "Amount");

            migrationBuilder.CreateTable(
                name: "Money",
                schema: "orders",
                columns: table => new
                {
                    ProductId = table.Column<Guid>(nullable: false),
                    ProductOrderId = table.Column<Guid>(nullable: false),
                    Amount = table.Column<double>(nullable: false),
                    Exchange = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Money", x => new { x.ProductId, x.ProductOrderId });
                    table.ForeignKey(
                        name: "FK_Money_Product_ProductId_ProductOrderId",
                        columns: x => new { x.ProductId, x.ProductOrderId },
                        principalSchema: "orders",
                        principalTable: "Product",
                        principalColumns: new[] { "Id", "OrderId" },
                        onDelete: ReferentialAction.Cascade);
                });
        }
    }
}
