using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PrimePOS.DAL.Migrations
{
    /// <inheritdoc />
    public partial class Test7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NumeroComprobante",
                table: "Facturas",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "TipoPrecioId",
                table: "Facturas",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TipoPrecioNombre",
                table: "Facturas",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumeroComprobante",
                table: "Facturas");

            migrationBuilder.DropColumn(
                name: "TipoPrecioId",
                table: "Facturas");

            migrationBuilder.DropColumn(
                name: "TipoPrecioNombre",
                table: "Facturas");
        }
    }
}
