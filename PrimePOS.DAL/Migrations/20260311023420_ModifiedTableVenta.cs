using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PrimePOS.DAL.Migrations
{
    /// <inheritdoc />
    public partial class ModifiedTableVenta : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AperturaCajaId",
                table: "Ventas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "NumeroComprobante",
                table: "Ventas",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Ventas_AperturaCajaId",
                table: "Ventas",
                column: "AperturaCajaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ventas_AperturaCajas_AperturaCajaId",
                table: "Ventas",
                column: "AperturaCajaId",
                principalTable: "AperturaCajas",
                principalColumn: "AperturaCajaId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ventas_AperturaCajas_AperturaCajaId",
                table: "Ventas");

            migrationBuilder.DropIndex(
                name: "IX_Ventas_AperturaCajaId",
                table: "Ventas");

            migrationBuilder.DropColumn(
                name: "AperturaCajaId",
                table: "Ventas");

            migrationBuilder.DropColumn(
                name: "NumeroComprobante",
                table: "Ventas");
        }
    }
}
