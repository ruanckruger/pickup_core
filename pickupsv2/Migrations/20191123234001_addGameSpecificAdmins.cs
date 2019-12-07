using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace pickupsv2.Migrations
{
    public partial class addGameSpecificAdmins : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GameAdmin",
                columns: table => new
                {
                    GameAdminId = table.Column<Guid>(nullable: false),
                    Id = table.Column<Guid>(nullable: true),
                    GameId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameAdmin", x => x.GameAdminId);
                    table.ForeignKey(
                        name: "FK_GameAdmin_AspNetUsers_GameId",
                        column: x => x.GameId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GameAdmin_Games_Id",
                        column: x => x.Id,
                        principalTable: "Games",
                        principalColumn: "GameId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GameAdmin_GameId",
                table: "GameAdmin",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_GameAdmin_Id",
                table: "GameAdmin",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GameAdmin");
        }
    }
}
