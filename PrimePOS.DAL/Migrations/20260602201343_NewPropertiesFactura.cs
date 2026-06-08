using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PrimePOS.DAL.Migrations
{
    /// <inheritdoc />
    public partial class NewPropertiesFactura : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "EstadoNombre",
                table: "Facturas",
                newName: "EstadoFacturaNombre");

            migrationBuilder.AddColumn<int>(
                name: "CajaId",
                table: "Facturas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "CajaNombre",
                table: "Facturas",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CajaId",
                table: "Facturas");

            migrationBuilder.DropColumn(
                name: "CajaNombre",
                table: "Facturas");

            migrationBuilder.RenameColumn(
                name: "EstadoFacturaNombre",
                table: "Facturas",
                newName: "EstadoNombre");
        }
    }
}
