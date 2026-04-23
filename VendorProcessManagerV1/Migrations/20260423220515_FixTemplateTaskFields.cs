using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VendorProcessManagerV1.Migrations
{
    /// <inheritdoc />
    public partial class FixTemplateTaskFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApproverId",
                table: "ProcessTemplatesTasks");

            migrationBuilder.DropColumn(
                name: "TaskId",
                table: "ProcessTemplatesTasks");

            migrationBuilder.DropColumn(
                name: "Creator",
                table: "ProcessTemplates");

            migrationBuilder.RenameColumn(
                name: "TemplateID",
                table: "ProcessTemplatesTasks",
                newName: "ProcessTemplateId");

            migrationBuilder.AddColumn<string>(
                name: "ApproverIdId",
                table: "ProcessTemplatesTasks",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Version",
                table: "ProcessTemplates",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "CreatorId",
                table: "ProcessTemplates",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessTemplateTransition_FromProcessTemplateTaskId",
                table: "ProcessTemplateTransition",
                column: "FromProcessTemplateTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessTemplatesTasks_ApproverIdId",
                table: "ProcessTemplatesTasks",
                column: "ApproverIdId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessTemplatesTasks_ProcessTemplateId",
                table: "ProcessTemplatesTasks",
                column: "ProcessTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessTemplates_CreatorId",
                table: "ProcessTemplates",
                column: "CreatorId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProcessTemplates_AspNetUsers_CreatorId",
                table: "ProcessTemplates",
                column: "CreatorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProcessTemplatesTasks_AspNetUsers_ApproverIdId",
                table: "ProcessTemplatesTasks",
                column: "ApproverIdId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProcessTemplatesTasks_ProcessTemplates_ProcessTemplateId",
                table: "ProcessTemplatesTasks",
                column: "ProcessTemplateId",
                principalTable: "ProcessTemplates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProcessTemplateTransition_ProcessTemplatesTasks_FromProcessTemplateTaskId",
                table: "ProcessTemplateTransition",
                column: "FromProcessTemplateTaskId",
                principalTable: "ProcessTemplatesTasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProcessTemplates_AspNetUsers_CreatorId",
                table: "ProcessTemplates");

            migrationBuilder.DropForeignKey(
                name: "FK_ProcessTemplatesTasks_AspNetUsers_ApproverIdId",
                table: "ProcessTemplatesTasks");

            migrationBuilder.DropForeignKey(
                name: "FK_ProcessTemplatesTasks_ProcessTemplates_ProcessTemplateId",
                table: "ProcessTemplatesTasks");

            migrationBuilder.DropForeignKey(
                name: "FK_ProcessTemplateTransition_ProcessTemplatesTasks_FromProcessTemplateTaskId",
                table: "ProcessTemplateTransition");

            migrationBuilder.DropIndex(
                name: "IX_ProcessTemplateTransition_FromProcessTemplateTaskId",
                table: "ProcessTemplateTransition");

            migrationBuilder.DropIndex(
                name: "IX_ProcessTemplatesTasks_ApproverIdId",
                table: "ProcessTemplatesTasks");

            migrationBuilder.DropIndex(
                name: "IX_ProcessTemplatesTasks_ProcessTemplateId",
                table: "ProcessTemplatesTasks");

            migrationBuilder.DropIndex(
                name: "IX_ProcessTemplates_CreatorId",
                table: "ProcessTemplates");

            migrationBuilder.DropColumn(
                name: "ApproverIdId",
                table: "ProcessTemplatesTasks");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "ProcessTemplates");

            migrationBuilder.RenameColumn(
                name: "ProcessTemplateId",
                table: "ProcessTemplatesTasks",
                newName: "TemplateID");

            migrationBuilder.AddColumn<Guid>(
                name: "ApproverId",
                table: "ProcessTemplatesTasks",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "TaskId",
                table: "ProcessTemplatesTasks",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<string>(
                name: "Version",
                table: "ProcessTemplates",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "Creator",
                table: "ProcessTemplates",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }
    }
}
