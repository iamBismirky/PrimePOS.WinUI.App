using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PrimePOS.DAL.Migrations
{
    /// <inheritdoc />
    public partial class SeedIntoMetodoPago : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "MetodoPagos",
                columns: new[] { "MetodoPagoId", "Estado", "Nombre" },
                values: new object[,]
                {
                    { 1, true, "Efectivo" },
                    { 2, true, "Tarjeta" },
                    { 3, true, "Teansferencia" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "MetodoPagos",
                keyColumn: "MetodoPagoId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "MetodoPagos",
                keyColumn: "MetodoPagoId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "MetodoPagos",
                keyColumn: "MetodoPagoId",
                keyValue: 3);
        }
    }
}
