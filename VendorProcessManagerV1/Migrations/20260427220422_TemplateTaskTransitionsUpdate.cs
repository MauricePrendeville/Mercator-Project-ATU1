using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VendorProcessManagerV1.Migrations
{
    /// <inheritdoc />
    public partial class TemplateTaskTransitionsUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ProcessTemplateTransition_FromProcessTemplateTaskId",
                table: "ProcessTemplateTransition");

            migrationBuilder.DropColumn(
                name: "ProcessTemplateId",
                table: "ProcessTemplateTransition");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDefault",
                table: "ProcessTemplateTransition",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ConditionExpression",
                table: "ProcessTemplateTransition",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProcessTemplateTransition_FromProcessTemplateTaskId_IsDefault",
                table: "ProcessTemplateTransition",
                columns: new[] { "FromProcessTemplateTaskId", "IsDefault" },
                unique: true,
                filter: "[IsDefault] =1");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessTemplateTransition_ToProcessTemplateTaskId",
                table: "ProcessTemplateTransition",
                column: "ToProcessTemplateTaskId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProcessTemplateTransition_ProcessTemplatesTasks_ToProcessTemplateTaskId",
                table: "ProcessTemplateTransition",
                column: "ToProcessTemplateTaskId",
                principalTable: "ProcessTemplatesTasks",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProcessTemplateTransition_ProcessTemplatesTasks_ToProcessTemplateTaskId",
                table: "ProcessTemplateTransition");

            migrationBuilder.DropIndex(
                name: "IX_ProcessTemplateTransition_FromProcessTemplateTaskId_IsDefault",
                table: "ProcessTemplateTransition");

            migrationBuilder.DropIndex(
                name: "IX_ProcessTemplateTransition_ToProcessTemplateTaskId",
                table: "ProcessTemplateTransition");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDefault",
                table: "ProcessTemplateTransition",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<int>(
                name: "ConditionExpression",
                table: "ProcessTemplateTransition",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ProcessTemplateId",
                table: "ProcessTemplateTransition",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_ProcessTemplateTransition_FromProcessTemplateTaskId",
                table: "ProcessTemplateTransition",
                column: "FromProcessTemplateTaskId");
        }
    }
}
