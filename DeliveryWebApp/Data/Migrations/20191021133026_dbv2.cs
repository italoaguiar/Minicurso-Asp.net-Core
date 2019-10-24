using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DeliveryWebApp.Data.Migrations
{
    public partial class dbv2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItemsCardapio_Pedidos_PedidoId",
                table: "ItemsCardapio");

            migrationBuilder.DropIndex(
                name: "IX_ItemsCardapio_PedidoId",
                table: "ItemsCardapio");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d55cf046-7ff9-4c7b-81de-5266555f4c39");

            migrationBuilder.DropColumn(
                name: "PedidoId",
                table: "ItemsCardapio");

            migrationBuilder.CreateTable(
                name: "PedidoItens",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ItemId = table.Column<Guid>(nullable: false),
                    Quantidade = table.Column<long>(nullable: false),
                    total = table.Column<double>(nullable: false),
                    PedidoId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PedidoItens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PedidoItens_ItemsCardapio_ItemId",
                        column: x => x.ItemId,
                        principalTable: "ItemsCardapio",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PedidoItens_Pedidos_PedidoId",
                        column: x => x.PedidoId,
                        principalTable: "Pedidos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "933912fc-3e14-465c-ac5b-642f98498c27", "58d00cb9-892b-42e5-8103-da12f82a66af", "Admin", "ADMIN" });

            migrationBuilder.CreateIndex(
                name: "IX_PedidoItens_ItemId",
                table: "PedidoItens",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_PedidoItens_PedidoId",
                table: "PedidoItens",
                column: "PedidoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PedidoItens");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "933912fc-3e14-465c-ac5b-642f98498c27");

            migrationBuilder.AddColumn<Guid>(
                name: "PedidoId",
                table: "ItemsCardapio",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "d55cf046-7ff9-4c7b-81de-5266555f4c39", "1cf65399-fcc5-4951-be33-36346bf4dee5", "Admin", "ADMIN" });

            migrationBuilder.CreateIndex(
                name: "IX_ItemsCardapio_PedidoId",
                table: "ItemsCardapio",
                column: "PedidoId");

            migrationBuilder.AddForeignKey(
                name: "FK_ItemsCardapio_Pedidos_PedidoId",
                table: "ItemsCardapio",
                column: "PedidoId",
                principalTable: "Pedidos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
