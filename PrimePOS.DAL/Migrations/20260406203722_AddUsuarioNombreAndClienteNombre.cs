using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PrimePOS.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddUsuarioNombreAndClienteNombre : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProductoNombre",
                table: "VentasDetalle",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ClienteNombre",
                table: "Ventas",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UsuarioNombre",
                table: "Ventas",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "ProductoId",
                table: "FacturasDetalle",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ClienteNombre",
                table: "Facturas",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UsuarioNombre",
                table: "Facturas",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_FacturasDetalle_ProductoId",
                table: "FacturasDetalle",
                column: "ProductoId");

            migrationBuilder.AddForeignKey(
                name: "FK_FacturasDetalle_Productos_ProductoId",
                table: "FacturasDetalle",
                column: "ProductoId",
                principalTable: "Productos",
                principalColumn: "ProductoId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FacturasDetalle_Productos_ProductoId",
                table: "FacturasDetalle");

            migrationBuilder.DropIndex(
                name: "IX_FacturasDetalle_ProductoId",
                table: "FacturasDetalle");

            migrationBuilder.DropColumn(
                name: "ProductoNombre",
                table: "VentasDetalle");

            migrationBuilder.DropColumn(
                name: "ClienteNombre",
                table: "Ventas");

            migrationBuilder.DropColumn(
                name: "UsuarioNombre",
                table: "Ventas");

            migrationBuilder.DropColumn(
                name: "ProductoId",
                table: "FacturasDetalle");

            migrationBuilder.DropColumn(
                name: "ClienteNombre",
                table: "Facturas");

            migrationBuilder.DropColumn(
                name: "UsuarioNombre",
                table: "Facturas");
        }
    }
}
