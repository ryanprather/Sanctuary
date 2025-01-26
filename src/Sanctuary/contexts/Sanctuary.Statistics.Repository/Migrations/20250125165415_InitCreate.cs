using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sanctuary.Statistics.Repository.Migrations
{
    /// <inheritdoc />
    public partial class InitCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Statistics");

            migrationBuilder.CreateTable(
                name: "JobTypes",
                schema: "Statistics",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Jobs",
                schema: "Statistics",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StatisticsJobTypeId = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Created = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Started = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    Completed = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Jobs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Jobs_JobTypes_StatisticsJobTypeId",
                        column: x => x.StatisticsJobTypeId,
                        principalSchema: "Statistics",
                        principalTable: "JobTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "JobDataFiles",
                schema: "Statistics",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StatisticsJobId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BlobUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DataFileMapJson = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobDataFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JobDataFiles_Jobs_StatisticsJobId",
                        column: x => x.StatisticsJobId,
                        principalSchema: "Statistics",
                        principalTable: "Jobs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "JobEndpoints",
                schema: "Statistics",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StatisticsJobId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EndpointMapJson = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobEndpoints", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JobEndpoints_Jobs_StatisticsJobId",
                        column: x => x.StatisticsJobId,
                        principalSchema: "Statistics",
                        principalTable: "Jobs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "JobPatients",
                schema: "Statistics",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StatisticsJobId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PatientIdentifer = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobPatients", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JobPatients_Jobs_StatisticsJobId",
                        column: x => x.StatisticsJobId,
                        principalSchema: "Statistics",
                        principalTable: "Jobs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StatisticalResults",
                schema: "Statistics",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StatisticsJobId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GraphData = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatisticalResults", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StatisticalResults_Jobs_StatisticsJobId",
                        column: x => x.StatisticsJobId,
                        principalSchema: "Statistics",
                        principalTable: "Jobs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_JobDataFiles_StatisticsJobId",
                schema: "Statistics",
                table: "JobDataFiles",
                column: "StatisticsJobId");

            migrationBuilder.CreateIndex(
                name: "IX_JobEndpoints_StatisticsJobId",
                schema: "Statistics",
                table: "JobEndpoints",
                column: "StatisticsJobId");

            migrationBuilder.CreateIndex(
                name: "IX_JobPatients_StatisticsJobId",
                schema: "Statistics",
                table: "JobPatients",
                column: "StatisticsJobId");

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_StatisticsJobTypeId",
                schema: "Statistics",
                table: "Jobs",
                column: "StatisticsJobTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_StatisticalResults_StatisticsJobId",
                schema: "Statistics",
                table: "StatisticalResults",
                column: "StatisticsJobId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JobDataFiles",
                schema: "Statistics");

            migrationBuilder.DropTable(
                name: "JobEndpoints",
                schema: "Statistics");

            migrationBuilder.DropTable(
                name: "JobPatients",
                schema: "Statistics");

            migrationBuilder.DropTable(
                name: "StatisticalResults",
                schema: "Statistics");

            migrationBuilder.DropTable(
                name: "Jobs",
                schema: "Statistics");

            migrationBuilder.DropTable(
                name: "JobTypes",
                schema: "Statistics");
        }
    }
}
