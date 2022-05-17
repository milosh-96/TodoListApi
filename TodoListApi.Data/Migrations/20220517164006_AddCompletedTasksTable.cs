using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TodoListApi.Data.Migrations
{
    public partial class AddCompletedTasksTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.CreateTable(
                name: "CompletedTasksUsers",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    TaskId = table.Column<string>(nullable: true),
                    CompletedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompletedTasksUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompletedTasksUsers_TodoTasks_TaskId",
                        column: x => x.TaskId,
                        principalTable: "TodoTasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CompletedTasksUsers_TaskId",
                table: "CompletedTasksUsers",
                column: "TaskId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompletedTasksUsers");
        }
    }
}
