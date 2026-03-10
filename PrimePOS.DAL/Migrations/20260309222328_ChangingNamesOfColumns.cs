using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PrimePOS.DAL.Migrations
{
    /// <inheritdoc />
    public partial class ChangingNamesOfColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FechaRegistro",
                table: "Categorias");

            migrationBuilder.RenameColumn(
                name: "Fecha",
                table: "Ventas",
                newName: "FechaRegistro");

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaRegistro",
                table: "Clientes",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FechaRegistro",
                table: "Clientes");

            migrationBuilder.RenameColumn(
                name: "FechaRegistro",
                table: "Ventas",
                newName: "Fecha");

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaRegistro",
                table: "Categorias",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
