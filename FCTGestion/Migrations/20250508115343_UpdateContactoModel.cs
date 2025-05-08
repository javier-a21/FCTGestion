using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FCTGestion.Migrations
{
    /// <inheritdoc />
    public partial class UpdateContactoModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Como",
                table: "Contactos");

            migrationBuilder.DropColumn(
                name: "ConQuien",
                table: "Contactos");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "TutoresEmpresa",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Telefono",
                table: "Empresas",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Direccion",
                table: "Empresas",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Resumen",
                table: "Contactos",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "AlumnoId",
                table: "Contactos",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Medio",
                table: "Contactos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TutorEmpresaId",
                table: "Contactos",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TutoresEmpresa_UserId",
                table: "TutoresEmpresa",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Contactos_AlumnoId",
                table: "Contactos",
                column: "AlumnoId");

            migrationBuilder.CreateIndex(
                name: "IX_Contactos_TutorEmpresaId",
                table: "Contactos",
                column: "TutorEmpresaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Contactos_Alumnos_AlumnoId",
                table: "Contactos",
                column: "AlumnoId",
                principalTable: "Alumnos",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Contactos_TutoresEmpresa_TutorEmpresaId",
                table: "Contactos",
                column: "TutorEmpresaId",
                principalTable: "TutoresEmpresa",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TutoresEmpresa_AspNetUsers_UserId",
                table: "TutoresEmpresa",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contactos_Alumnos_AlumnoId",
                table: "Contactos");

            migrationBuilder.DropForeignKey(
                name: "FK_Contactos_TutoresEmpresa_TutorEmpresaId",
                table: "Contactos");

            migrationBuilder.DropForeignKey(
                name: "FK_TutoresEmpresa_AspNetUsers_UserId",
                table: "TutoresEmpresa");

            migrationBuilder.DropIndex(
                name: "IX_TutoresEmpresa_UserId",
                table: "TutoresEmpresa");

            migrationBuilder.DropIndex(
                name: "IX_Contactos_AlumnoId",
                table: "Contactos");

            migrationBuilder.DropIndex(
                name: "IX_Contactos_TutorEmpresaId",
                table: "Contactos");

            migrationBuilder.DropColumn(
                name: "AlumnoId",
                table: "Contactos");

            migrationBuilder.DropColumn(
                name: "Medio",
                table: "Contactos");

            migrationBuilder.DropColumn(
                name: "TutorEmpresaId",
                table: "Contactos");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "TutoresEmpresa",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Telefono",
                table: "Empresas",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Direccion",
                table: "Empresas",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Resumen",
                table: "Contactos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Como",
                table: "Contactos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ConQuien",
                table: "Contactos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
