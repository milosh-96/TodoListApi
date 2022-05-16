using Microsoft.EntityFrameworkCore.Migrations;

namespace TodoListApi.Data.Migrations
{
    public partial class AddTimeZoneInfoIdToUsersTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TimezoneInfoId",
                table: "AspNetUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TimezoneInfoId",
                table: "AspNetUsers");
        }
    }
}
