using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FCTGestion.Migrations
{
    /// <inheritdoc />
    public partial class ActualizarTareaDiaria : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EstadoValidacion",
                table: "TareasDiarias");

            migrationBuilder.AddColumn<int>(
                name: "Estado",
                table: "TareasDiarias",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaValidacion",
                table: "TareasDiarias",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Observaciones",
                table: "TareasDiarias",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Estado",
                table: "TareasDiarias");

            migrationBuilder.DropColumn(
                name: "FechaValidacion",
                table: "TareasDiarias");

            migrationBuilder.DropColumn(
                name: "Observaciones",
                table: "TareasDiarias");

            migrationBuilder.AddColumn<string>(
                name: "EstadoValidacion",
                table: "TareasDiarias",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
