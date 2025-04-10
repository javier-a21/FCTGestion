﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FCTGestion.Migrations
{
    /// <inheritdoc />
    public partial class FixDebeCambiarPasswordColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "DebeCambiarPassword",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DebeCambiarPassword",
                table: "AspNetUsers");
        }
    }
}
