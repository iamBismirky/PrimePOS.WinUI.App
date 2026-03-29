using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PrimePOS.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddedTableFacturaAndFacturasDetalle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Facturas",
                columns: table => new
                {
                    FacturaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Numero = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    VentaId = table.Column<int>(type: "int", nullable: false),
                    Subtotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Descuento = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Impuesto = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Total = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MetodoPago = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Facturas", x => x.FacturaId);
                    table.ForeignKey(
                        name: "FK_Facturas_Ventas_VentaId",
                        column: x => x.VentaId,
                        principalTable: "Ventas",
                        principalColumn: "VentaId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FacturasDetalle",
                columns: table => new
                {
                    FacturaDetalleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FacturaId = table.Column<int>(type: "int", nullable: false),
                    ProductoNombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Precio = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Cantidad = table.Column<int>(type: "int", nullable: false),
                    Total = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FacturasDetalle", x => x.FacturaDetalleId);
                    table.ForeignKey(
                        name: "FK_FacturasDetalle_Facturas_FacturaId",
                        column: x => x.FacturaId,
                        principalTable: "Facturas",
                        principalColumn: "FacturaId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Facturas_VentaId",
                table: "Facturas",
                column: "VentaId");

            migrationBuilder.CreateIndex(
                name: "IX_FacturasDetalle_FacturaId",
                table: "FacturasDetalle",
                column: "FacturaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FacturasDetalle");

            migrationBuilder.DropTable(
                name: "Facturas");
        }
    }
}
