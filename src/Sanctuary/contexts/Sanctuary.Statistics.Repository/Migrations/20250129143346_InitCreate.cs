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
                name: "Jobs",
                schema: "Statistics",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Created = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Started = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    Completed = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StatisticsJobDetailsJson = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Jobs", x => x.Id);
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
                name: "IX_StatisticalResults_StatisticsJobId",
                schema: "Statistics",
                table: "StatisticalResults",
                column: "StatisticsJobId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StatisticalResults",
                schema: "Statistics");

            migrationBuilder.DropTable(
                name: "Jobs",
                schema: "Statistics");
        }
    }
}
