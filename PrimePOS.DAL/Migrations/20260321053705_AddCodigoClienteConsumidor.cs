using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PrimePOS.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddCodigoClienteConsumidor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Cajas",
                keyColumn: "CajaId",
                keyValue: 1,
                columns: new[] { "Estado", "Nombre" },
                values: new object[] { true, "Principal" });

            migrationBuilder.UpdateData(
                table: "Clientes",
                keyColumn: "ClienteId",
                keyValue: 1,
                column: "Codigo",
                value: "CLIENT-0001");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Cajas",
                keyColumn: "CajaId",
                keyValue: 1,
                columns: new[] { "Estado", "Nombre" },
                values: new object[] { false, "Caja Principal" });

            migrationBuilder.UpdateData(
                table: "Clientes",
                keyColumn: "ClienteId",
                keyValue: 1,
                column: "Codigo",
                value: "");
        }
    }
}
