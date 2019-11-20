using Microsoft.EntityFrameworkCore.Migrations;

namespace pickupsv2.Migrations
{
    public partial class LinkMaps : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Maps_GameId",
                table: "Maps",
                column: "GameId");

            migrationBuilder.AddForeignKey(
                name: "FK_Maps_Games_GameId",
                table: "Maps",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "GameId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Maps_Games_GameId",
                table: "Maps");

            migrationBuilder.DropIndex(
                name: "IX_Maps_GameId",
                table: "Maps");
        }
    }
}
