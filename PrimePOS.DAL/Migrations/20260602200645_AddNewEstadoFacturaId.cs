using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PrimePOS.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddNewEstadoFacturaId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Estado",
                table: "Facturas",
                newName: "EstadoNombre");

            migrationBuilder.AddColumn<int>(
                name: "EstadoFacturaId",
                table: "Facturas",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EstadoFacturaId",
                table: "Facturas");

            migrationBuilder.RenameColumn(
                name: "EstadoNombre",
                table: "Facturas",
                newName: "Estado");
        }
    }
}
