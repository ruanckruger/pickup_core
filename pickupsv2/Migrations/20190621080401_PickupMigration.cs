using Microsoft.EntityFrameworkCore.Migrations;

namespace pickupsv2.Migrations
{
    public partial class PickupMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "surname",
                table: "Players");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "surname",
                table: "Players",
                nullable: true);
        }
    }
}
