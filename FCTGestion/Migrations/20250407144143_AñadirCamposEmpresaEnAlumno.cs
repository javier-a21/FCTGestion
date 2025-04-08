using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FCTGestion.Migrations
{
    /// <inheritdoc />
    public partial class AñadirCamposEmpresaEnAlumno : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Alumnos",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "EmpresaId",
                table: "Alumnos",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaFinFCT",
                table: "Alumnos",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaInicioFCT",
                table: "Alumnos",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TutorEmpresaId",
                table: "Alumnos",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Alumnos_EmpresaId",
                table: "Alumnos",
                column: "EmpresaId");

            migrationBuilder.CreateIndex(
                name: "IX_Alumnos_TutorEmpresaId",
                table: "Alumnos",
                column: "TutorEmpresaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Alumnos_Empresas_EmpresaId",
                table: "Alumnos",
                column: "EmpresaId",
                principalTable: "Empresas",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Alumnos_TutoresEmpresa_TutorEmpresaId",
                table: "Alumnos",
                column: "TutorEmpresaId",
                principalTable: "TutoresEmpresa",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Alumnos_Empresas_EmpresaId",
                table: "Alumnos");

            migrationBuilder.DropForeignKey(
                name: "FK_Alumnos_TutoresEmpresa_TutorEmpresaId",
                table: "Alumnos");

            migrationBuilder.DropIndex(
                name: "IX_Alumnos_EmpresaId",
                table: "Alumnos");

            migrationBuilder.DropIndex(
                name: "IX_Alumnos_TutorEmpresaId",
                table: "Alumnos");

            migrationBuilder.DropColumn(
                name: "EmpresaId",
                table: "Alumnos");

            migrationBuilder.DropColumn(
                name: "FechaFinFCT",
                table: "Alumnos");

            migrationBuilder.DropColumn(
                name: "FechaInicioFCT",
                table: "Alumnos");

            migrationBuilder.DropColumn(
                name: "TutorEmpresaId",
                table: "Alumnos");


            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Alumnos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
