using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VendorProcessManagerV1.Migrations
{
    /// <inheritdoc />
    public partial class SwitchToApproverTeam : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProcessTemplatesTasks_AspNetUsers_ApproverId",
                table: "ProcessTemplatesTasks");

            migrationBuilder.DropIndex(
                name: "IX_ProcessTemplatesTasks_ApproverId",
                table: "ProcessTemplatesTasks");

            migrationBuilder.DropColumn(
                name: "ApproverId",
                table: "ProcessTemplatesTasks");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApproverId",
                table: "ProcessTemplatesTasks",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProcessTemplatesTasks_ApproverId",
                table: "ProcessTemplatesTasks",
                column: "ApproverId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProcessTemplatesTasks_AspNetUsers_ApproverId",
                table: "ProcessTemplatesTasks",
                column: "ApproverId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
