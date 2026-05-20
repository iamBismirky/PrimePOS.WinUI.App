using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PrimePOS.DAL.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTableVenta : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Estado",
                table: "Ventas");

            migrationBuilder.RenameColumn(
                name: "Efectivo",
                table: "Ventas",
                newName: "MontoPagado");

            migrationBuilder.AddColumn<decimal>(
                name: "BalancePendiente",
                table: "Ventas",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "EstadoPago",
                table: "Ventas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EstadoVenta",
                table: "Ventas",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BalancePendiente",
                table: "Ventas");

            migrationBuilder.DropColumn(
                name: "EstadoPago",
                table: "Ventas");

            migrationBuilder.DropColumn(
                name: "EstadoVenta",
                table: "Ventas");

            migrationBuilder.RenameColumn(
                name: "MontoPagado",
                table: "Ventas",
                newName: "Efectivo");

            migrationBuilder.AddColumn<bool>(
                name: "Estado",
                table: "Ventas",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
