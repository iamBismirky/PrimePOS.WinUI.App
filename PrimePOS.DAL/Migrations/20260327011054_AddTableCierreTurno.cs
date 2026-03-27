using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PrimePOS.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddTableCierreTurno : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CierresTurno",
                columns: table => new
                {
                    CierreTurnoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TurnoId = table.Column<int>(type: "int", nullable: false),
                    MontoInicial = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalEfectivo = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalTarjeta = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalTransferencia = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalGeneral = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    EfectivoContado = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Diferencia = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CierresTurno", x => x.CierreTurnoId);
                    table.ForeignKey(
                        name: "FK_CierresTurno_Turnos_TurnoId",
                        column: x => x.TurnoId,
                        principalTable: "Turnos",
                        principalColumn: "TurnoId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CierresTurno_TurnoId",
                table: "CierresTurno",
                column: "TurnoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CierresTurno");
        }
    }
}
