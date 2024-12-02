using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Webshop.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_orderDetails_orders_OrderId1",
                table: "orderDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_orders_customers_CustomerID",
                table: "orders");

            migrationBuilder.DropIndex(
                name: "IX_orderDetails_OrderId1",
                table: "orderDetails");

            migrationBuilder.DropColumn(
                name: "OrderId1",
                table: "orderDetails");

            migrationBuilder.RenameColumn(
                name: "CustomerID",
                table: "orders",
                newName: "CustomerId");

            migrationBuilder.RenameIndex(
                name: "IX_orders_CustomerID",
                table: "orders",
                newName: "IX_orders_CustomerId");

            migrationBuilder.AddColumn<decimal>(
                name: "Amount",
                table: "payments",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "OrderId",
                table: "payments",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "PaymentDate",
                table: "payments",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "PaymentMethod",
                table: "payments",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<int>(
                name: "CustomerId",
                table: "orders",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "OrderDate",
                table: "orders",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<float>(
                name: "TotalAmount",
                table: "orders",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OrderId",
                table: "orderDetails",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "orderDetails",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "orderDetails",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "UnitPrice",
                table: "orderDetails",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "customers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "customers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "customers",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "customers",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "customers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "categories",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "categories",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_orderDetails_OrderId",
                table: "orderDetails",
                column: "OrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_orderDetails_orders_OrderId",
                table: "orderDetails",
                column: "OrderId",
                principalTable: "orders",
                principalColumn: "OrderId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_orders_customers_CustomerId",
                table: "orders",
                column: "CustomerId",
                principalTable: "customers",
                principalColumn: "CustomerID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_orderDetails_orders_OrderId",
                table: "orderDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_orders_customers_CustomerId",
                table: "orders");

            migrationBuilder.DropIndex(
                name: "IX_orderDetails_OrderId",
                table: "orderDetails");

            migrationBuilder.DropColumn(
                name: "Amount",
                table: "payments");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "payments");

            migrationBuilder.DropColumn(
                name: "PaymentDate",
                table: "payments");

            migrationBuilder.DropColumn(
                name: "PaymentMethod",
                table: "payments");

            migrationBuilder.DropColumn(
                name: "OrderDate",
                table: "orders");

            migrationBuilder.DropColumn(
                name: "TotalAmount",
                table: "orders");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "orderDetails");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "orderDetails");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "orderDetails");

            migrationBuilder.DropColumn(
                name: "UnitPrice",
                table: "orderDetails");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "customers");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "customers");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "customers");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "customers");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "customers");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "categories");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "categories");

            migrationBuilder.RenameColumn(
                name: "CustomerId",
                table: "orders",
                newName: "CustomerID");

            migrationBuilder.RenameIndex(
                name: "IX_orders_CustomerId",
                table: "orders",
                newName: "IX_orders_CustomerID");

            migrationBuilder.AlterColumn<int>(
                name: "CustomerID",
                table: "orders",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<int>(
                name: "OrderId1",
                table: "orderDetails",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_orderDetails_OrderId1",
                table: "orderDetails",
                column: "OrderId1");

            migrationBuilder.AddForeignKey(
                name: "FK_orderDetails_orders_OrderId1",
                table: "orderDetails",
                column: "OrderId1",
                principalTable: "orders",
                principalColumn: "OrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_orders_customers_CustomerID",
                table: "orders",
                column: "CustomerID",
                principalTable: "customers",
                principalColumn: "CustomerID");
        }
    }
}
