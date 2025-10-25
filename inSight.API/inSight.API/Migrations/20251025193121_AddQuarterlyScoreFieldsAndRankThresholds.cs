using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace inSight.API.Migrations
{
    /// <inheritdoc />
    public partial class AddQuarterlyScoreFieldsAndRankThresholds : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuarterlyScores_Quarters_QuarterId",
                table: "QuarterlyScores");

            migrationBuilder.DropForeignKey(
                name: "FK_QuarterlyScores_Users_UserId",
                table: "QuarterlyScores");

            migrationBuilder.DropIndex(
                name: "IX_QuarterlyScores_UserId",
                table: "QuarterlyScores");

            migrationBuilder.DropColumn(
                name: "AssignedRank",
                table: "QuarterlyScores");

            migrationBuilder.DropColumn(
                name: "EvaluationScore",
                table: "QuarterlyScores");

            migrationBuilder.DropColumn(
                name: "ManagementBonusPoints",
                table: "QuarterlyScores");

            migrationBuilder.DropColumn(
                name: "ManagementComment",
                table: "QuarterlyScores");

            migrationBuilder.AddColumn<int>(
                name: "CurrentTotalScore",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "StartingScore",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "QuarterlyScores",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "TotalScore",
                table: "QuarterlyScores",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(5,2)");

            migrationBuilder.AddColumn<int>(
                name: "BaseScore",
                table: "QuarterlyScores",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "EvaluationId",
                table: "QuarterlyScores",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "LeaderAverageScore",
                table: "QuarterlyScores",
                type: "decimal(5,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ManagementBonusReason",
                table: "QuarterlyScores",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ManagementBonusScore",
                table: "QuarterlyScores",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "RankScoreThresholds",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RankId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MinScore = table.Column<int>(type: "int", nullable: false),
                    MaxScore = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RankScoreThresholds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RankScoreThresholds_Ranks_RankId",
                        column: x => x.RankId,
                        principalTable: "Ranks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RankScoreThresholds_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_QuarterlyScores_UserId_QuarterId",
                table: "QuarterlyScores",
                columns: new[] { "UserId", "QuarterId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RankScoreThresholds_RankId_RoleId",
                table: "RankScoreThresholds",
                columns: new[] { "RankId", "RoleId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RankScoreThresholds_RoleId",
                table: "RankScoreThresholds",
                column: "RoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_QuarterlyScores_Quarters_QuarterId",
                table: "QuarterlyScores",
                column: "QuarterId",
                principalTable: "Quarters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_QuarterlyScores_Users_UserId",
                table: "QuarterlyScores",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuarterlyScores_Quarters_QuarterId",
                table: "QuarterlyScores");

            migrationBuilder.DropForeignKey(
                name: "FK_QuarterlyScores_Users_UserId",
                table: "QuarterlyScores");

            migrationBuilder.DropTable(
                name: "RankScoreThresholds");

            migrationBuilder.DropIndex(
                name: "IX_QuarterlyScores_UserId_QuarterId",
                table: "QuarterlyScores");

            migrationBuilder.DropColumn(
                name: "CurrentTotalScore",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "StartingScore",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "BaseScore",
                table: "QuarterlyScores");

            migrationBuilder.DropColumn(
                name: "EvaluationId",
                table: "QuarterlyScores");

            migrationBuilder.DropColumn(
                name: "LeaderAverageScore",
                table: "QuarterlyScores");

            migrationBuilder.DropColumn(
                name: "ManagementBonusReason",
                table: "QuarterlyScores");

            migrationBuilder.DropColumn(
                name: "ManagementBonusScore",
                table: "QuarterlyScores");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "QuarterlyScores",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalScore",
                table: "QuarterlyScores",
                type: "decimal(5,2)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "AssignedRank",
                table: "QuarterlyScores",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "EvaluationScore",
                table: "QuarterlyScores",
                type: "decimal(5,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "ManagementBonusPoints",
                table: "QuarterlyScores",
                type: "decimal(5,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "ManagementComment",
                table: "QuarterlyScores",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_QuarterlyScores_UserId",
                table: "QuarterlyScores",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_QuarterlyScores_Quarters_QuarterId",
                table: "QuarterlyScores",
                column: "QuarterId",
                principalTable: "Quarters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_QuarterlyScores_Users_UserId",
                table: "QuarterlyScores",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
