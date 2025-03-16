using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskManagementAPI.Migrations
{
    /// <inheritdoc />
    public partial class RenamedTaskToUserTask : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TagUserTask_Tag_TagsId",
                table: "TagUserTask");

            migrationBuilder.DropForeignKey(
                name: "FK_TagUserTask_Usertasks_TasksId",
                table: "TagUserTask");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Usertasks",
                table: "Usertasks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tag",
                table: "Tag");

            migrationBuilder.RenameTable(
                name: "Usertasks",
                newName: "UserTasks");

            migrationBuilder.RenameTable(
                name: "Tag",
                newName: "Tags");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserTasks",
                table: "UserTasks",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tags",
                table: "Tags",
                column: "Id");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TagUserTask_Tags_TagsId",
                table: "TagUserTask");

            migrationBuilder.DropForeignKey(
                name: "FK_TagUserTask_UserTasks_TasksId",
                table: "TagUserTask");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserTasks",
                table: "UserTasks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tags",
                table: "Tags");

            migrationBuilder.RenameTable(
                name: "UserTasks",
                newName: "Usertasks");

            migrationBuilder.RenameTable(
                name: "Tags",
                newName: "Tag");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Usertasks",
                table: "Usertasks",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tag",
                table: "Tag",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TagUserTask_Tag_TagsId",
                table: "TagUserTask",
                column: "TagsId",
                principalTable: "Tag",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TagUserTask_Usertasks_TasksId",
                table: "TagUserTask",
                column: "TasksId",
                principalTable: "Usertasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
