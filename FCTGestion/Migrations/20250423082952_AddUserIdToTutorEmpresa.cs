using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FCTGestion.Migrations
{
    /// <inheritdoc />
    public partial class AddUserIdToTutorEmpresa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RAEs");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "TutoresEmpresa",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "TutoresCentro",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_TutoresCentro_UserId",
                table: "TutoresCentro",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_TutoresCentro_AspNetUsers_UserId",
                table: "TutoresCentro",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TutoresCentro_AspNetUsers_UserId",
                table: "TutoresCentro");

            migrationBuilder.DropIndex(
                name: "IX_TutoresCentro_UserId",
                table: "TutoresCentro");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "TutoresEmpresa");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "TutoresCentro",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "RAEs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AlumnoId = table.Column<int>(type: "int", nullable: false),
                    EmpresaId = table.Column<int>(type: "int", nullable: false),
                    TutorEmpresaId = table.Column<int>(type: "int", nullable: false),
                    FechaFin = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaInicio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Horario = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RAEs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RAEs_Alumnos_AlumnoId",
                        column: x => x.AlumnoId,
                        principalTable: "Alumnos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RAEs_Empresas_EmpresaId",
                        column: x => x.EmpresaId,
                        principalTable: "Empresas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RAEs_TutoresEmpresa_TutorEmpresaId",
                        column: x => x.TutorEmpresaId,
                        principalTable: "TutoresEmpresa",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RAEs_AlumnoId",
                table: "RAEs",
                column: "AlumnoId");

            migrationBuilder.CreateIndex(
                name: "IX_RAEs_EmpresaId",
                table: "RAEs",
                column: "EmpresaId");

            migrationBuilder.CreateIndex(
                name: "IX_RAEs_TutorEmpresaId",
                table: "RAEs",
                column: "TutorEmpresaId");
        }
    }
}
