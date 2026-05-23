using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PrimePOS.DAL.Migrations
{
    /// <inheritdoc />
    public partial class RenamePrecioOnProductos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PrecioVenta",
                table: "Productos",
                newName: "PrecioMinorista");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PrecioMinorista",
                table: "Productos",
                newName: "PrecioVenta");
        }
    }
}
