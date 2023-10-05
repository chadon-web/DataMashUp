using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataMashUp.Migrations
{
    public partial class deskpokesettings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tbl_DespokeSettings",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    APIKey = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BespokeBaseUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserKey = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UseTestkey = table.Column<bool>(type: "bit", nullable: false),
                    nutritionRequestUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Auxilary = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_DespokeSettings", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tbl_DespokeSettings");
        }
    }
}
