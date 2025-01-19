using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sanctuary.Statistics.Repository.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Statistics");

            migrationBuilder.CreateTable(
                name: "StatisticsJobTypes",
                schema: "Statistics",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatisticsJobTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StatisticsJobs",
                schema: "Statistics",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StatisticsJobTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Created = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Started = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Completed = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatisticsJobs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StatisticsJobs_StatisticsJobTypes_StatisticsJobTypeId",
                        column: x => x.StatisticsJobTypeId,
                        principalSchema: "Statistics",
                        principalTable: "StatisticsJobTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StatisticalAnalyses",
                schema: "Statistics",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StatisticsJobId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GraphData = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatisticalAnalyses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StatisticalAnalyses_StatisticsJobs_StatisticsJobId",
                        column: x => x.StatisticsJobId,
                        principalSchema: "Statistics",
                        principalTable: "StatisticsJobs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StatisticalAnalyses_StatisticsJobId",
                schema: "Statistics",
                table: "StatisticalAnalyses",
                column: "StatisticsJobId");

            migrationBuilder.CreateIndex(
                name: "IX_StatisticsJobs_StatisticsJobTypeId",
                schema: "Statistics",
                table: "StatisticsJobs",
                column: "StatisticsJobTypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StatisticalAnalyses",
                schema: "Statistics");

            migrationBuilder.DropTable(
                name: "StatisticsJobs",
                schema: "Statistics");

            migrationBuilder.DropTable(
                name: "StatisticsJobTypes",
                schema: "Statistics");
        }
    }
}
