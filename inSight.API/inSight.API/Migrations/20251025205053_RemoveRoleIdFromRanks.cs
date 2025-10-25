using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace inSight.API.Migrations
{
    /// <inheritdoc />
    public partial class RemoveRoleIdFromRanks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ranks_Roles_RoleId",
                table: "Ranks");

            migrationBuilder.DropIndex(
                name: "IX_Ranks_RoleId_Code",
                table: "Ranks");

            migrationBuilder.DropColumn(
                name: "MaxScore",
                table: "Ranks");

            migrationBuilder.DropColumn(
                name: "MinScore",
                table: "Ranks");

            migrationBuilder.DropColumn(
                name: "RoleId",
                table: "Ranks");

            migrationBuilder.CreateIndex(
                name: "IX_Ranks_Code",
                table: "Ranks",
                column: "Code",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Ranks_Code",
                table: "Ranks");

            migrationBuilder.AddColumn<decimal>(
                name: "MaxScore",
                table: "Ranks",
                type: "decimal(5,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "MinScore",
                table: "Ranks",
                type: "decimal(5,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<Guid>(
                name: "RoleId",
                table: "Ranks",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Ranks_RoleId_Code",
                table: "Ranks",
                columns: new[] { "RoleId", "Code" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Ranks_Roles_RoleId",
                table: "Ranks",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
