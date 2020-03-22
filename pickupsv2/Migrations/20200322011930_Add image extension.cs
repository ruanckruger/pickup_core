using Microsoft.EntityFrameworkCore.Migrations;

namespace pickupsv2.Migrations
{
    public partial class Addimageextension : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "imageExtension",
                table: "Games",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "imageExtension",
                table: "Games");
        }
    }
}
