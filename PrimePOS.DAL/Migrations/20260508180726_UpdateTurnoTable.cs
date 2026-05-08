using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PrimePOS.DAL.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTurnoTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Diferencia",
                table: "Turnos",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "EfectivoContado",
                table: "Turnos",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalEfectivo",
                table: "Turnos",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalGeneral",
                table: "Turnos",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalTarjeta",
                table: "Turnos",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalTransferencia",
                table: "Turnos",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Diferencia",
                table: "Turnos");

            migrationBuilder.DropColumn(
                name: "EfectivoContado",
                table: "Turnos");

            migrationBuilder.DropColumn(
                name: "TotalEfectivo",
                table: "Turnos");

            migrationBuilder.DropColumn(
                name: "TotalGeneral",
                table: "Turnos");

            migrationBuilder.DropColumn(
                name: "TotalTarjeta",
                table: "Turnos");

            migrationBuilder.DropColumn(
                name: "TotalTransferencia",
                table: "Turnos");
        }
    }
}
