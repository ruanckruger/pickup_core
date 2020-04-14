using Microsoft.EntityFrameworkCore.Migrations;

namespace pickupsv2.Migrations
{
    public partial class AddMapImage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Image",
                table: "Maps",
                newName: "ImageExtension");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImageExtension",
                table: "Maps",
                newName: "Image");
        }
    }
}
