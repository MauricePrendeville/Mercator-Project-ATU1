using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VendorProcessManagerV1.Migrations
{
    /// <inheritdoc />
    public partial class VendorOwnerUpdate2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Creator",
                table: "VendorCandidates");

            migrationBuilder.AddColumn<string>(
                name: "OwnerId",
                table: "VendorCandidates",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_VendorCandidates_OwnerId",
                table: "VendorCandidates",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_VendorCandidates_AspNetUsers_OwnerId",
                table: "VendorCandidates",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VendorCandidates_AspNetUsers_OwnerId",
                table: "VendorCandidates");

            migrationBuilder.DropIndex(
                name: "IX_VendorCandidates_OwnerId",
                table: "VendorCandidates");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "VendorCandidates");

            migrationBuilder.AddColumn<Guid>(
                name: "Creator",
                table: "VendorCandidates",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }
    }
}
