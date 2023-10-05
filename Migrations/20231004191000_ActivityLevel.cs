using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataMashUp.Migrations
{
    public partial class ActivityLevel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ActivityLevel",
                table: "tbl_Request",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActivityLevel",
                table: "tbl_Request");
        }
    }
}
