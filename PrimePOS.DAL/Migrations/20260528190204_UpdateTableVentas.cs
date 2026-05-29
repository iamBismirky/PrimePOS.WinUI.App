using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PrimePOS.DAL.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTableVentas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CajaNombre",
                table: "Ventas",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "EstadoVentaNombre",
                table: "Ventas",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MetodoPagoNombre",
                table: "Ventas",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TipoPrecioNombre",
                table: "Ventas",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TipoVentaNombre",
                table: "Ventas",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<int>(
                name: "NumeroTurno",
                table: "Facturas",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CajaNombre",
                table: "Ventas");

            migrationBuilder.DropColumn(
                name: "EstadoVentaNombre",
                table: "Ventas");

            migrationBuilder.DropColumn(
                name: "MetodoPagoNombre",
                table: "Ventas");

            migrationBuilder.DropColumn(
                name: "TipoPrecioNombre",
                table: "Ventas");

            migrationBuilder.DropColumn(
                name: "TipoVentaNombre",
                table: "Ventas");

            migrationBuilder.AlterColumn<string>(
                name: "NumeroTurno",
                table: "Facturas",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
