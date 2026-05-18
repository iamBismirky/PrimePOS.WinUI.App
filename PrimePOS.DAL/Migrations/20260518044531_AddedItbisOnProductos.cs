using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PrimePOS.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddedItbisOnProductos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PrecioManual",
                table: "Productos");

            migrationBuilder.RenameColumn(
                name: "PrecioVentaManual",
                table: "Productos",
                newName: "Itbis");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Itbis",
                table: "Productos",
                newName: "PrecioVentaManual");

            migrationBuilder.AddColumn<bool>(
                name: "PrecioManual",
                table: "Productos",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
