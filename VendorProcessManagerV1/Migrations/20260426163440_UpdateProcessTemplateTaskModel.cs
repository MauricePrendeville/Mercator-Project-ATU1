using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VendorProcessManagerV1.Migrations
{
    /// <inheritdoc />
    public partial class UpdateProcessTemplateTaskModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProcessTemplatesTasks_AspNetUsers_ApproverIdId",
                table: "ProcessTemplatesTasks");

            migrationBuilder.RenameColumn(
                name: "ApproverIdId",
                table: "ProcessTemplatesTasks",
                newName: "ApproverId");

            migrationBuilder.RenameIndex(
                name: "IX_ProcessTemplatesTasks_ApproverIdId",
                table: "ProcessTemplatesTasks",
                newName: "IX_ProcessTemplatesTasks_ApproverId");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "ProcessTemplatesTasks",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "ApproverTeam",
                table: "ProcessTemplatesTasks",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddForeignKey(
                name: "FK_ProcessTemplatesTasks_AspNetUsers_ApproverId",
                table: "ProcessTemplatesTasks",
                column: "ApproverId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProcessTemplatesTasks_AspNetUsers_ApproverId",
                table: "ProcessTemplatesTasks");

            migrationBuilder.RenameColumn(
                name: "ApproverId",
                table: "ProcessTemplatesTasks",
                newName: "ApproverIdId");

            migrationBuilder.RenameIndex(
                name: "IX_ProcessTemplatesTasks_ApproverId",
                table: "ProcessTemplatesTasks",
                newName: "IX_ProcessTemplatesTasks_ApproverIdId");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "ProcessTemplatesTasks",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ApproverTeam",
                table: "ProcessTemplatesTasks",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ProcessTemplatesTasks_AspNetUsers_ApproverIdId",
                table: "ProcessTemplatesTasks",
                column: "ApproverIdId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
