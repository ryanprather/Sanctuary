using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sanctuary.Statistics.Repository.Migrations
{
    /// <inheritdoc />
    public partial class AddStatsResultsOPtions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ResultOptionsJson",
                schema: "Statistics",
                table: "StatisticalResults",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ResultOptionsJson",
                schema: "Statistics",
                table: "StatisticalResults");
        }
    }
}
