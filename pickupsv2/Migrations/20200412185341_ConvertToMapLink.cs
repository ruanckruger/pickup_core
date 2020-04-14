using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace pickupsv2.Migrations
{
    public partial class ConvertToMapLink : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Map",
                table: "Matches");

            migrationBuilder.RenameColumn(
                name: "imageExtension",
                table: "Games",
                newName: "ImageExtension");

            migrationBuilder.AddColumn<Guid>(
                name: "MapId",
                table: "Matches",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Matches_MapId",
                table: "Matches",
                column: "MapId");

            migrationBuilder.AddForeignKey(
                name: "FK_Matches_Maps_MapId",
                table: "Matches",
                column: "MapId",
                principalTable: "Maps",
                principalColumn: "MapId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Matches_Maps_MapId",
                table: "Matches");

            migrationBuilder.DropIndex(
                name: "IX_Matches_MapId",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "MapId",
                table: "Matches");

            migrationBuilder.RenameColumn(
                name: "ImageExtension",
                table: "Games",
                newName: "imageExtension");

            migrationBuilder.AddColumn<string>(
                name: "Map",
                table: "Matches",
                nullable: true);
        }
    }
}
