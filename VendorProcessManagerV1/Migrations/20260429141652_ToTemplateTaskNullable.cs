using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VendorProcessManagerV1.Migrations
{
    /// <inheritdoc />
    public partial class ToTemplateTaskNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProcessTemplateTransition_ProcessTemplatesTasks_FromProcessTemplateTaskId",
                table: "ProcessTemplateTransition");

            migrationBuilder.DropForeignKey(
                name: "FK_ProcessTemplateTransition_ProcessTemplatesTasks_ToProcessTemplateTaskId",
                table: "ProcessTemplateTransition");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProcessTemplateTransition",
                table: "ProcessTemplateTransition");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProcessTaskTransition",
                table: "ProcessTaskTransition");

            migrationBuilder.RenameTable(
                name: "ProcessTemplateTransition",
                newName: "ProcessTemplateTransitions");

            migrationBuilder.RenameTable(
                name: "ProcessTaskTransition",
                newName: "ProcessTaskTransitions");

            migrationBuilder.RenameIndex(
                name: "IX_ProcessTemplateTransition_ToProcessTemplateTaskId",
                table: "ProcessTemplateTransitions",
                newName: "IX_ProcessTemplateTransitions_ToProcessTemplateTaskId");

            migrationBuilder.RenameIndex(
                name: "IX_ProcessTemplateTransition_FromProcessTemplateTaskId_IsDefault",
                table: "ProcessTemplateTransitions",
                newName: "IX_ProcessTemplateTransitions_FromProcessTemplateTaskId_IsDefault");

            migrationBuilder.AlterColumn<Guid>(
                name: "ToProcessTemplateTaskId",
                table: "ProcessTemplateTransitions",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProcessTemplateTransitions",
                table: "ProcessTemplateTransitions",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProcessTaskTransitions",
                table: "ProcessTaskTransitions",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProcessTemplateTransitions_ProcessTemplatesTasks_FromProcessTemplateTaskId",
                table: "ProcessTemplateTransitions",
                column: "FromProcessTemplateTaskId",
                principalTable: "ProcessTemplatesTasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProcessTemplateTransitions_ProcessTemplatesTasks_ToProcessTemplateTaskId",
                table: "ProcessTemplateTransitions",
                column: "ToProcessTemplateTaskId",
                principalTable: "ProcessTemplatesTasks",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProcessTemplateTransitions_ProcessTemplatesTasks_FromProcessTemplateTaskId",
                table: "ProcessTemplateTransitions");

            migrationBuilder.DropForeignKey(
                name: "FK_ProcessTemplateTransitions_ProcessTemplatesTasks_ToProcessTemplateTaskId",
                table: "ProcessTemplateTransitions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProcessTemplateTransitions",
                table: "ProcessTemplateTransitions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProcessTaskTransitions",
                table: "ProcessTaskTransitions");

            migrationBuilder.RenameTable(
                name: "ProcessTemplateTransitions",
                newName: "ProcessTemplateTransition");

            migrationBuilder.RenameTable(
                name: "ProcessTaskTransitions",
                newName: "ProcessTaskTransition");

            migrationBuilder.RenameIndex(
                name: "IX_ProcessTemplateTransitions_ToProcessTemplateTaskId",
                table: "ProcessTemplateTransition",
                newName: "IX_ProcessTemplateTransition_ToProcessTemplateTaskId");

            migrationBuilder.RenameIndex(
                name: "IX_ProcessTemplateTransitions_FromProcessTemplateTaskId_IsDefault",
                table: "ProcessTemplateTransition",
                newName: "IX_ProcessTemplateTransition_FromProcessTemplateTaskId_IsDefault");

            migrationBuilder.AlterColumn<Guid>(
                name: "ToProcessTemplateTaskId",
                table: "ProcessTemplateTransition",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProcessTemplateTransition",
                table: "ProcessTemplateTransition",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProcessTaskTransition",
                table: "ProcessTaskTransition",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProcessTemplateTransition_ProcessTemplatesTasks_FromProcessTemplateTaskId",
                table: "ProcessTemplateTransition",
                column: "FromProcessTemplateTaskId",
                principalTable: "ProcessTemplatesTasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProcessTemplateTransition_ProcessTemplatesTasks_ToProcessTemplateTaskId",
                table: "ProcessTemplateTransition",
                column: "ToProcessTemplateTaskId",
                principalTable: "ProcessTemplatesTasks",
                principalColumn: "Id");
        }
    }
}
