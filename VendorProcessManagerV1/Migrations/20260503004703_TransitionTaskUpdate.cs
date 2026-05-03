using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VendorProcessManagerV1.Migrations
{
    /// <inheritdoc />
    public partial class TransitionTaskUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ActivatedByTransitionId",
                table: "ProcessTasks",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "ProcessTasks",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "SelectedTransitionId",
                table: "ProcessTasks",
                type: "uniqueidentifier",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActivatedByTransitionId",
                table: "ProcessTasks");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "ProcessTasks");

            migrationBuilder.DropColumn(
                name: "SelectedTransitionId",
                table: "ProcessTasks");
        }
    }
}
