using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataMashUp.Migrations
{
    public partial class addedadateonRequest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "tbl_Request",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Date",
                table: "tbl_Request");
        }
    }
}
