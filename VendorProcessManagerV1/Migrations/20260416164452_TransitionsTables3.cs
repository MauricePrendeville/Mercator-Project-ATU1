using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VendorProcessManagerV1.Migrations
{
    /// <inheritdoc />
    public partial class TransitionsTables3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProcessTaskTransition",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProcessInstanceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProcessTemplateTrasitionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FromProcessStepId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ToProcessStepId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ActionUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ActionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ActionValue = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcessTaskTransition", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProcessTemplateTransition",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProcessTemplateId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FromProcessTemplateTaskId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ToProcessTemplateTaskId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DisplayLabel = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    ConditionType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConditionExpression = table.Column<int>(type: "int", nullable: true),
                    IsDefault = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcessTemplateTransition", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProcessTaskTransition");

            migrationBuilder.DropTable(
                name: "ProcessTemplateTransition");
        }
    }
}
