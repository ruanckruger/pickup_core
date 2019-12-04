using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace pickupsv2.Migrations
{
    public partial class LinkMatchAndGame : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "GameID",
                table: "Matches",
                newName: "GameId");

            migrationBuilder.AlterColumn<Guid>(
                name: "GameId",
                table: "Matches",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.CreateIndex(
                name: "IX_Matches_GameId",
                table: "Matches",
                column: "GameId");

            migrationBuilder.AddForeignKey(
                name: "FK_Matches_Games_GameId",
                table: "Matches",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "GameId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Matches_Games_GameId",
                table: "Matches");

            migrationBuilder.DropIndex(
                name: "IX_Matches_GameId",
                table: "Matches");

            migrationBuilder.RenameColumn(
                name: "GameId",
                table: "Matches",
                newName: "GameID");

            migrationBuilder.AlterColumn<Guid>(
                name: "GameID",
                table: "Matches",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);
        }
    }
}
