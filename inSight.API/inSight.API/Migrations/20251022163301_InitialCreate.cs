using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace inSight.API.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EvaluationCategories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    QuestionnaireType = table.Column<int>(type: "int", nullable: false),
                    OrderIndex = table.Column<int>(type: "int", nullable: false),
                    IsManagementOnly = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EvaluationCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Quarters",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    QuarterNumber = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsLocked = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Quarters", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RankConfigurations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Rank = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Position = table.Column<int>(type: "int", nullable: false),
                    MinPoints = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    MaxPoints = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    Salary = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    EffectiveFrom = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EffectiveTo = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RankConfigurations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<int>(type: "int", nullable: false),
                    Position = table.Column<int>(type: "int", nullable: false),
                    CurrentRank = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    TeamLeadId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Users_TeamLeadId",
                        column: x => x.TeamLeadId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EvaluationQuestions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuestionText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OrderIndex = table.Column<int>(type: "int", nullable: false),
                    IsManagementOnly = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EvaluationQuestions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EvaluationQuestions_EvaluationCategories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "EvaluationCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmailNotifications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmailType = table.Column<int>(type: "int", nullable: false),
                    Subject = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Body = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    SentAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ErrorMessage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RetryCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailNotifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmailNotifications_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Evaluations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuarterId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EvaluatedUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EvaluatorUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EvaluationType = table.Column<int>(type: "int", nullable: false),
                    QuestionnaireType = table.Column<int>(type: "int", nullable: false),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: false),
                    OverallScore = table.Column<decimal>(type: "decimal(3,2)", nullable: true),
                    GeneralComment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Evaluations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Evaluations_Quarters_QuarterId",
                        column: x => x.QuarterId,
                        principalTable: "Quarters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Evaluations_Users_EvaluatedUserId",
                        column: x => x.EvaluatedUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Evaluations_Users_EvaluatorUserId",
                        column: x => x.EvaluatorUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "HROneOnOnes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HRRepresentativeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MeetingDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Topics = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ActionItems = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HROneOnOnes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HROneOnOnes_Users_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HROneOnOnes_Users_HRRepresentativeId",
                        column: x => x.HRRepresentativeId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LeaderJournals",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LeaderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuarterId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EntryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EntryType = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeaderJournals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LeaderJournals_Quarters_QuarterId",
                        column: x => x.QuarterId,
                        principalTable: "Quarters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LeaderJournals_Users_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LeaderJournals_Users_LeaderId",
                        column: x => x.LeaderId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "QuarterlyScores",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuarterId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EvaluationScore = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    ManagementBonusPoints = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    TotalScore = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    AssignedRank = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    ManagementComment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuarterlyScores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuarterlyScores_Quarters_QuarterId",
                        column: x => x.QuarterId,
                        principalTable: "Quarters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QuarterlyScores_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RankHistories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OldRank = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    NewRank = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    QuarterId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ChangedById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ChangedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RankHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RankHistories_Quarters_QuarterId",
                        column: x => x.QuarterId,
                        principalTable: "Quarters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RankHistories_Users_ChangedById",
                        column: x => x.ChangedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RankHistories_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EvaluationAnswers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EvaluationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuestionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Score = table.Column<int>(type: "int", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EvaluationAnswers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EvaluationAnswers_EvaluationQuestions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "EvaluationQuestions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EvaluationAnswers_Evaluations_EvaluationId",
                        column: x => x.EvaluationId,
                        principalTable: "Evaluations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ManagementReviews",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EvaluationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReviewedById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ManagementReviews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ManagementReviews_Evaluations_EvaluationId",
                        column: x => x.EvaluationId,
                        principalTable: "Evaluations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ManagementReviews_Users_ReviewedById",
                        column: x => x.ReviewedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmailNotifications_UserId",
                table: "EmailNotifications",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_EvaluationAnswers_EvaluationId",
                table: "EvaluationAnswers",
                column: "EvaluationId");

            migrationBuilder.CreateIndex(
                name: "IX_EvaluationAnswers_QuestionId",
                table: "EvaluationAnswers",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_EvaluationQuestions_CategoryId",
                table: "EvaluationQuestions",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Evaluations_EvaluatedUserId",
                table: "Evaluations",
                column: "EvaluatedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Evaluations_EvaluatorUserId",
                table: "Evaluations",
                column: "EvaluatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Evaluations_QuarterId",
                table: "Evaluations",
                column: "QuarterId");

            migrationBuilder.CreateIndex(
                name: "IX_HROneOnOnes_EmployeeId",
                table: "HROneOnOnes",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_HROneOnOnes_HRRepresentativeId",
                table: "HROneOnOnes",
                column: "HRRepresentativeId");

            migrationBuilder.CreateIndex(
                name: "IX_LeaderJournals_EmployeeId",
                table: "LeaderJournals",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_LeaderJournals_LeaderId",
                table: "LeaderJournals",
                column: "LeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_LeaderJournals_QuarterId",
                table: "LeaderJournals",
                column: "QuarterId");

            migrationBuilder.CreateIndex(
                name: "IX_ManagementReviews_EvaluationId",
                table: "ManagementReviews",
                column: "EvaluationId");

            migrationBuilder.CreateIndex(
                name: "IX_ManagementReviews_ReviewedById",
                table: "ManagementReviews",
                column: "ReviewedById");

            migrationBuilder.CreateIndex(
                name: "IX_QuarterlyScores_QuarterId",
                table: "QuarterlyScores",
                column: "QuarterId");

            migrationBuilder.CreateIndex(
                name: "IX_QuarterlyScores_UserId",
                table: "QuarterlyScores",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Quarters_Year_QuarterNumber",
                table: "Quarters",
                columns: new[] { "Year", "QuarterNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RankConfigurations_Rank_Position_EffectiveFrom",
                table: "RankConfigurations",
                columns: new[] { "Rank", "Position", "EffectiveFrom" });

            migrationBuilder.CreateIndex(
                name: "IX_RankHistories_ChangedById",
                table: "RankHistories",
                column: "ChangedById");

            migrationBuilder.CreateIndex(
                name: "IX_RankHistories_QuarterId",
                table: "RankHistories",
                column: "QuarterId");

            migrationBuilder.CreateIndex(
                name: "IX_RankHistories_UserId",
                table: "RankHistories",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_TeamLeadId",
                table: "Users",
                column: "TeamLeadId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmailNotifications");

            migrationBuilder.DropTable(
                name: "EvaluationAnswers");

            migrationBuilder.DropTable(
                name: "HROneOnOnes");

            migrationBuilder.DropTable(
                name: "LeaderJournals");

            migrationBuilder.DropTable(
                name: "ManagementReviews");

            migrationBuilder.DropTable(
                name: "QuarterlyScores");

            migrationBuilder.DropTable(
                name: "RankConfigurations");

            migrationBuilder.DropTable(
                name: "RankHistories");

            migrationBuilder.DropTable(
                name: "EvaluationQuestions");

            migrationBuilder.DropTable(
                name: "Evaluations");

            migrationBuilder.DropTable(
                name: "EvaluationCategories");

            migrationBuilder.DropTable(
                name: "Quarters");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
