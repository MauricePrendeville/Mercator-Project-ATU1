using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VendorProcessManagerV1.Migrations
{
    /// <inheritdoc />
    public partial class UpdateProcessInstanceAndTask : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                table: "AspNetRoleClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                table: "AspNetUserClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                table: "AspNetUserLogins");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                table: "AspNetUserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                table: "AspNetUserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                table: "AspNetUserTokens");

            migrationBuilder.DropForeignKey(
                name: "FK_ProcessTemplates_AspNetUsers_CreatorId",
                table: "ProcessTemplates");

            migrationBuilder.DropForeignKey(
                name: "FK_ProcessTemplateTaskProcessTemplateTask_ProcessTemplatesTasks_DependsOnId",
                table: "ProcessTemplateTaskProcessTemplateTask");

            migrationBuilder.DropTable(
                name: "ProcessTaskProcessTask");

            migrationBuilder.DropColumn(
                name: "Approver",
                table: "ProcessTasks");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "ProcessTasks");

            migrationBuilder.DropColumn(
                name: "TaskStatus",
                table: "ProcessTasks");

            migrationBuilder.DropColumn(
                name: "Task_Id",
                table: "ProcessTasks");

            migrationBuilder.DropColumn(
                name: "InitiatedBy",
                table: "ProcessInstances");

            migrationBuilder.RenameColumn(
                name: "RequiresApproval",
                table: "ProcessTasks",
                newName: "ApprovalRequired");

            migrationBuilder.RenameColumn(
                name: "Owner",
                table: "ProcessTasks",
                newName: "ProcessTemplateTaskId");

            migrationBuilder.RenameColumn(
                name: "Creator",
                table: "ProcessTasks",
                newName: "ProcessInstanceId");

            migrationBuilder.RenameColumn(
                name: "TemplateId",
                table: "ProcessInstances",
                newName: "ProcessTemplateId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedDate",
                table: "ProcessTasks",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "ProcessTasks",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "ApproveStatus",
                table: "ProcessTasks",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApproverId",
                table: "ProcessTasks",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatorId",
                table: "ProcessTasks",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OwnerId",
                table: "ProcessTasks",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProcessTaskStatus",
                table: "ProcessTasks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SortOrder",
                table: "ProcessTasks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartedDate",
                table: "ProcessTasks",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TaskNotes",
                table: "ProcessTasks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "TargetEndDate",
                table: "ProcessInstances",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "ProcessInstances",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartDate",
                table: "ProcessInstances",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ActualEndDate",
                table: "ProcessInstances",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "ProcessInstances",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "InitiatedById",
                table: "ProcessInstances",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "InstanceName",
                table: "ProcessInstances",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessTasks_ApproverId",
                table: "ProcessTasks",
                column: "ApproverId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessTasks_CreatorId",
                table: "ProcessTasks",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessTasks_OwnerId",
                table: "ProcessTasks",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessTasks_ProcessInstanceId",
                table: "ProcessTasks",
                column: "ProcessInstanceId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessTasks_ProcessTemplateTaskId",
                table: "ProcessTasks",
                column: "ProcessTemplateTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessInstances_InitiatedById",
                table: "ProcessInstances",
                column: "InitiatedById");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessInstances_ProcessTemplateId",
                table: "ProcessInstances",
                column: "ProcessTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessInstances_VendorCandidateId",
                table: "ProcessInstances",
                column: "VendorCandidateId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId",
                principalTable: "AspNetRoles",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                table: "AspNetUserClaims",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                table: "AspNetUserLogins",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId",
                principalTable: "AspNetRoles",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                table: "AspNetUserRoles",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                table: "AspNetUserTokens",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProcessInstances_AspNetUsers_InitiatedById",
                table: "ProcessInstances",
                column: "InitiatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProcessInstances_ProcessTemplates_ProcessTemplateId",
                table: "ProcessInstances",
                column: "ProcessTemplateId",
                principalTable: "ProcessTemplates",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProcessInstances_VendorCandidates_VendorCandidateId",
                table: "ProcessInstances",
                column: "VendorCandidateId",
                principalTable: "VendorCandidates",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProcessTasks_AspNetUsers_ApproverId",
                table: "ProcessTasks",
                column: "ApproverId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProcessTasks_AspNetUsers_CreatorId",
                table: "ProcessTasks",
                column: "CreatorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProcessTasks_AspNetUsers_OwnerId",
                table: "ProcessTasks",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProcessTasks_ProcessInstances_ProcessInstanceId",
                table: "ProcessTasks",
                column: "ProcessInstanceId",
                principalTable: "ProcessInstances",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProcessTasks_ProcessTemplatesTasks_ProcessTemplateTaskId",
                table: "ProcessTasks",
                column: "ProcessTemplateTaskId",
                principalTable: "ProcessTemplatesTasks",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProcessTemplates_AspNetUsers_CreatorId",
                table: "ProcessTemplates",
                column: "CreatorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProcessTemplateTaskProcessTemplateTask_ProcessTemplatesTasks_DependsOnId",
                table: "ProcessTemplateTaskProcessTemplateTask",
                column: "DependsOnId",
                principalTable: "ProcessTemplatesTasks",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                table: "AspNetRoleClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                table: "AspNetUserClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                table: "AspNetUserLogins");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                table: "AspNetUserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                table: "AspNetUserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                table: "AspNetUserTokens");

            migrationBuilder.DropForeignKey(
                name: "FK_ProcessInstances_AspNetUsers_InitiatedById",
                table: "ProcessInstances");

            migrationBuilder.DropForeignKey(
                name: "FK_ProcessInstances_ProcessTemplates_ProcessTemplateId",
                table: "ProcessInstances");

            migrationBuilder.DropForeignKey(
                name: "FK_ProcessInstances_VendorCandidates_VendorCandidateId",
                table: "ProcessInstances");

            migrationBuilder.DropForeignKey(
                name: "FK_ProcessTasks_AspNetUsers_ApproverId",
                table: "ProcessTasks");

            migrationBuilder.DropForeignKey(
                name: "FK_ProcessTasks_AspNetUsers_CreatorId",
                table: "ProcessTasks");

            migrationBuilder.DropForeignKey(
                name: "FK_ProcessTasks_AspNetUsers_OwnerId",
                table: "ProcessTasks");

            migrationBuilder.DropForeignKey(
                name: "FK_ProcessTasks_ProcessInstances_ProcessInstanceId",
                table: "ProcessTasks");

            migrationBuilder.DropForeignKey(
                name: "FK_ProcessTasks_ProcessTemplatesTasks_ProcessTemplateTaskId",
                table: "ProcessTasks");

            migrationBuilder.DropForeignKey(
                name: "FK_ProcessTemplates_AspNetUsers_CreatorId",
                table: "ProcessTemplates");

            migrationBuilder.DropForeignKey(
                name: "FK_ProcessTemplateTaskProcessTemplateTask_ProcessTemplatesTasks_DependsOnId",
                table: "ProcessTemplateTaskProcessTemplateTask");

            migrationBuilder.DropIndex(
                name: "IX_ProcessTasks_ApproverId",
                table: "ProcessTasks");

            migrationBuilder.DropIndex(
                name: "IX_ProcessTasks_CreatorId",
                table: "ProcessTasks");

            migrationBuilder.DropIndex(
                name: "IX_ProcessTasks_OwnerId",
                table: "ProcessTasks");

            migrationBuilder.DropIndex(
                name: "IX_ProcessTasks_ProcessInstanceId",
                table: "ProcessTasks");

            migrationBuilder.DropIndex(
                name: "IX_ProcessTasks_ProcessTemplateTaskId",
                table: "ProcessTasks");

            migrationBuilder.DropIndex(
                name: "IX_ProcessInstances_InitiatedById",
                table: "ProcessInstances");

            migrationBuilder.DropIndex(
                name: "IX_ProcessInstances_ProcessTemplateId",
                table: "ProcessInstances");

            migrationBuilder.DropIndex(
                name: "IX_ProcessInstances_VendorCandidateId",
                table: "ProcessInstances");

            migrationBuilder.DropColumn(
                name: "ApproverId",
                table: "ProcessTasks");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "ProcessTasks");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "ProcessTasks");

            migrationBuilder.DropColumn(
                name: "ProcessTaskStatus",
                table: "ProcessTasks");

            migrationBuilder.DropColumn(
                name: "SortOrder",
                table: "ProcessTasks");

            migrationBuilder.DropColumn(
                name: "StartedDate",
                table: "ProcessTasks");

            migrationBuilder.DropColumn(
                name: "TaskNotes",
                table: "ProcessTasks");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "ProcessInstances");

            migrationBuilder.DropColumn(
                name: "InitiatedById",
                table: "ProcessInstances");

            migrationBuilder.DropColumn(
                name: "InstanceName",
                table: "ProcessInstances");

            migrationBuilder.RenameColumn(
                name: "ProcessTemplateTaskId",
                table: "ProcessTasks",
                newName: "Owner");

            migrationBuilder.RenameColumn(
                name: "ProcessInstanceId",
                table: "ProcessTasks",
                newName: "Creator");

            migrationBuilder.RenameColumn(
                name: "ApprovalRequired",
                table: "ProcessTasks",
                newName: "RequiresApproval");

            migrationBuilder.RenameColumn(
                name: "ProcessTemplateId",
                table: "ProcessInstances",
                newName: "TemplateId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedDate",
                table: "ProcessTasks",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "ProcessTasks",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ApproveStatus",
                table: "ProcessTasks",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "Approver",
                table: "ProcessTasks",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "ProcessTasks",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "TaskStatus",
                table: "ProcessTasks",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Task_Id",
                table: "ProcessTasks",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<DateTime>(
                name: "TargetEndDate",
                table: "ProcessInstances",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "ProcessInstances",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartDate",
                table: "ProcessInstances",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ActualEndDate",
                table: "ProcessInstances",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "InitiatedBy",
                table: "ProcessInstances",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.CreateTable(
                name: "ProcessTaskProcessTask",
                columns: table => new
                {
                    DependsOnId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SuccessorTasksId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcessTaskProcessTask", x => new { x.DependsOnId, x.SuccessorTasksId });
                    table.ForeignKey(
                        name: "FK_ProcessTaskProcessTask_ProcessTasks_DependsOnId",
                        column: x => x.DependsOnId,
                        principalTable: "ProcessTasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProcessTaskProcessTask_ProcessTasks_SuccessorTasksId",
                        column: x => x.SuccessorTasksId,
                        principalTable: "ProcessTasks",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProcessTaskProcessTask_SuccessorTasksId",
                table: "ProcessTaskProcessTask",
                column: "SuccessorTasksId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId",
                principalTable: "AspNetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                table: "AspNetUserClaims",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                table: "AspNetUserLogins",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId",
                principalTable: "AspNetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                table: "AspNetUserRoles",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                table: "AspNetUserTokens",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProcessTemplates_AspNetUsers_CreatorId",
                table: "ProcessTemplates",
                column: "CreatorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProcessTemplateTaskProcessTemplateTask_ProcessTemplatesTasks_DependsOnId",
                table: "ProcessTemplateTaskProcessTemplateTask",
                column: "DependsOnId",
                principalTable: "ProcessTemplatesTasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
