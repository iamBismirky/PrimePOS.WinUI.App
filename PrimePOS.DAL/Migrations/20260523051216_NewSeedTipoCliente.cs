using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PrimePOS.DAL.Migrations
{
    /// <inheritdoc />
    public partial class NewSeedTipoCliente : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "TipoClientes",
                keyColumn: "TipoClienteId",
                keyValue: 1,
                column: "Tipo",
                value: "Minorista");

            migrationBuilder.UpdateData(
                table: "TipoClientes",
                keyColumn: "TipoClienteId",
                keyValue: 2,
                column: "Tipo",
                value: "Mayorista");

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "UsuarioId",
                keyValue: 1,
                column: "Codigo",
                value: "USER-001");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "TipoClientes",
                keyColumn: "TipoClienteId",
                keyValue: 1,
                column: "Tipo",
                value: "NORMAL");

            migrationBuilder.UpdateData(
                table: "TipoClientes",
                keyColumn: "TipoClienteId",
                keyValue: 2,
                column: "Tipo",
                value: "MAYORISTA");

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "UsuarioId",
                keyValue: 1,
                column: "Codigo",
                value: "");
        }
    }
}
