using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskManagementAPI.Migrations
{
    /// <inheritdoc />
    public partial class ApplicationDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TagUserTask_Tags_TagsId",
                table: "TagUserTask");

            migrationBuilder.DropForeignKey(
                name: "FK_TagUserTask_UserTasks_TasksId",
                table: "TagUserTask");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TagUserTask",
                table: "TagUserTask");

            migrationBuilder.RenameTable(
                name: "TagUserTask",
                newName: "UserTaskTags");

            migrationBuilder.RenameColumn(
                name: "TasksId",
                table: "UserTaskTags",
                newName: "UserTasksId");

            migrationBuilder.RenameIndex(
                name: "IX_TagUserTask_TasksId",
                table: "UserTaskTags",
                newName: "IX_UserTaskTags_UserTasksId");

            migrationBuilder.AddColumn<Guid>(
                name: "AppUserId",
                table: "UserTasks",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserTaskTags",
                table: "UserTaskTags",
                columns: new[] { "TagsId", "UserTasksId" });

            migrationBuilder.CreateTable(
                name: "AppUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUsers", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserTasks_AppUserId",
                table: "UserTasks",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AppUsers_Email",
                table: "AppUsers",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppUsers_Username",
                table: "AppUsers",
                column: "Username",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_UserTasks_AppUsers_AppUserId",
                table: "UserTasks",
                column: "AppUserId",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserTaskTags_Tags_TagsId",
                table: "UserTaskTags",
                column: "TagsId",
                principalTable: "Tags",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserTaskTags_UserTasks_UserTasksId",
                table: "UserTaskTags",
                column: "UserTasksId",
                principalTable: "UserTasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserTasks_AppUsers_AppUserId",
                table: "UserTasks");

            migrationBuilder.DropForeignKey(
                name: "FK_UserTaskTags_Tags_TagsId",
                table: "UserTaskTags");

            migrationBuilder.DropForeignKey(
                name: "FK_UserTaskTags_UserTasks_UserTasksId",
                table: "UserTaskTags");

            migrationBuilder.DropTable(
                name: "AppUsers");

            migrationBuilder.DropIndex(
                name: "IX_UserTasks_AppUserId",
                table: "UserTasks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserTaskTags",
                table: "UserTaskTags");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "UserTasks");

            migrationBuilder.RenameTable(
                name: "UserTaskTags",
                newName: "TagUserTask");

            migrationBuilder.RenameColumn(
                name: "UserTasksId",
                table: "TagUserTask",
                newName: "TasksId");

            migrationBuilder.RenameIndex(
                name: "IX_UserTaskTags_UserTasksId",
                table: "TagUserTask",
                newName: "IX_TagUserTask_TasksId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TagUserTask",
                table: "TagUserTask",
                columns: new[] { "TagsId", "TasksId" });

            migrationBuilder.AddForeignKey(
                name: "FK_TagUserTask_Tags_TagsId",
                table: "TagUserTask",
                column: "TagsId",
                principalTable: "Tags",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TagUserTask_UserTasks_TasksId",
                table: "TagUserTask",
                column: "TasksId",
                principalTable: "UserTasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
