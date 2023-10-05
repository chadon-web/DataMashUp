using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataMashUp.Migrations
{
    public partial class Requestmodifes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DietRecomended",
                table: "tbl_Request",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DietRecomended",
                table: "tbl_Request");
        }
    }
}
