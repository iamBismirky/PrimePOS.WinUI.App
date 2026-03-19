using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PrimePOS.DAL.Migrations
{
    /// <inheritdoc />
    public partial class RenameTableAperturaCajaToTurno : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ventas_AperturaCajas_AperturaCajaId",
                table: "Ventas");

            migrationBuilder.DropTable(
                name: "AperturaCajas");

            migrationBuilder.RenameColumn(
                name: "AperturaCajaId",
                table: "Ventas",
                newName: "TurnoId");

            migrationBuilder.RenameIndex(
                name: "IX_Ventas_AperturaCajaId",
                table: "Ventas",
                newName: "IX_Ventas_TurnoId");

            migrationBuilder.CreateTable(
                name: "Turnos",
                columns: table => new
                {
                    TurnoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CajaId = table.Column<int>(type: "int", nullable: false),
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    FechaApertura = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MontoInicial = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MontoCierre = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    FechaCierre = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EstaAbierto = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Turnos", x => x.TurnoId);
                    table.ForeignKey(
                        name: "FK_Turnos_Cajas_CajaId",
                        column: x => x.CajaId,
                        principalTable: "Cajas",
                        principalColumn: "CajaId");
                    table.ForeignKey(
                        name: "FK_Turnos_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "UsuarioId");
                });

            migrationBuilder.UpdateData(
                table: "Cajas",
                keyColumn: "CajaId",
                keyValue: 1,
                column: "Estado",
                value: false);

            migrationBuilder.CreateIndex(
                name: "IX_Turnos_CajaId",
                table: "Turnos",
                column: "CajaId");

            migrationBuilder.CreateIndex(
                name: "IX_Turnos_UsuarioId",
                table: "Turnos",
                column: "UsuarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ventas_Turnos_TurnoId",
                table: "Ventas",
                column: "TurnoId",
                principalTable: "Turnos",
                principalColumn: "TurnoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ventas_Turnos_TurnoId",
                table: "Ventas");

            migrationBuilder.DropTable(
                name: "Turnos");

            migrationBuilder.RenameColumn(
                name: "TurnoId",
                table: "Ventas",
                newName: "AperturaCajaId");

            migrationBuilder.RenameIndex(
                name: "IX_Ventas_TurnoId",
                table: "Ventas",
                newName: "IX_Ventas_AperturaCajaId");

            migrationBuilder.CreateTable(
                name: "AperturaCajas",
                columns: table => new
                {
                    AperturaCajaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CajaId = table.Column<int>(type: "int", nullable: false),
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    Estado = table.Column<bool>(type: "bit", nullable: false),
                    FechaApertura = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaCierre = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MontoCierre = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    MontoInicial = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Turno = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AperturaCajas", x => x.AperturaCajaId);
                    table.ForeignKey(
                        name: "FK_AperturaCajas_Cajas_CajaId",
                        column: x => x.CajaId,
                        principalTable: "Cajas",
                        principalColumn: "CajaId");
                    table.ForeignKey(
                        name: "FK_AperturaCajas_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "UsuarioId");
                });

            migrationBuilder.UpdateData(
                table: "Cajas",
                keyColumn: "CajaId",
                keyValue: 1,
                column: "Estado",
                value: true);

            migrationBuilder.CreateIndex(
                name: "IX_AperturaCajas_CajaId",
                table: "AperturaCajas",
                column: "CajaId");

            migrationBuilder.CreateIndex(
                name: "IX_AperturaCajas_UsuarioId",
                table: "AperturaCajas",
                column: "UsuarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ventas_AperturaCajas_AperturaCajaId",
                table: "Ventas",
                column: "AperturaCajaId",
                principalTable: "AperturaCajas",
                principalColumn: "AperturaCajaId");
        }
    }
}
