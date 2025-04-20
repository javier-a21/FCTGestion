using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FCTGestion.Migrations
{
    /// <inheritdoc />
    public partial class AddUserIdToTutorCentro : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "TutoresCentro",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "TutoresCentro");
        }
    }
}
