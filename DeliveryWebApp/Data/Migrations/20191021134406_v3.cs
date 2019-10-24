using Microsoft.EntityFrameworkCore.Migrations;

namespace DeliveryWebApp.Data.Migrations
{
    public partial class v3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "933912fc-3e14-465c-ac5b-642f98498c27");

            migrationBuilder.DropColumn(
                name: "total",
                table: "PedidoItens");

            migrationBuilder.AddColumn<double>(
                name: "total",
                table: "Pedidos",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "e9873014-fa19-480e-be80-ca0e1ccf3316", "2d7648e8-1fe9-428c-85dd-ee44b70db34c", "Admin", "ADMIN" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e9873014-fa19-480e-be80-ca0e1ccf3316");

            migrationBuilder.DropColumn(
                name: "total",
                table: "Pedidos");

            migrationBuilder.AddColumn<double>(
                name: "total",
                table: "PedidoItens",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "933912fc-3e14-465c-ac5b-642f98498c27", "58d00cb9-892b-42e5-8103-da12f82a66af", "Admin", "ADMIN" });
        }
    }
}
