using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DeliveryWebApp.Data.Migrations
{
    public partial class dbupdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Pedidos",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ClientId = table.Column<Guid>(nullable: false),
                    ClienteId = table.Column<string>(nullable: true),
                    DataPedido = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pedidos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pedidos_AspNetUsers_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ItemsCardapio",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Nome = table.Column<string>(nullable: false),
                    Descricao = table.Column<string>(nullable: true),
                    Preco = table.Column<double>(nullable: false),
                    Imagem = table.Column<string>(nullable: false),
                    PedidoId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemsCardapio", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItemsCardapio_Pedidos_PedidoId",
                        column: x => x.PedidoId,
                        principalTable: "Pedidos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "d55cf046-7ff9-4c7b-81de-5266555f4c39", "1cf65399-fcc5-4951-be33-36346bf4dee5", "Admin", "ADMIN" });

            migrationBuilder.CreateIndex(
                name: "IX_ItemsCardapio_PedidoId",
                table: "ItemsCardapio",
                column: "PedidoId");

            migrationBuilder.CreateIndex(
                name: "IX_Pedidos_ClienteId",
                table: "Pedidos",
                column: "ClienteId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ItemsCardapio");

            migrationBuilder.DropTable(
                name: "Pedidos");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d55cf046-7ff9-4c7b-81de-5266555f4c39");
        }
    }
}
