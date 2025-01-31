using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sanctuary.Statistics.Repository.Migrations
{
    /// <inheritdoc />
    public partial class UpdateStatisticsResults : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "GraphData",
                schema: "Statistics",
                table: "StatisticalResults",
                newName: "DataBlobUri");

            migrationBuilder.AddColumn<string>(
                name: "ChartDataUri",
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
                name: "ChartDataUri",
                schema: "Statistics",
                table: "StatisticalResults");

            migrationBuilder.RenameColumn(
                name: "DataBlobUri",
                schema: "Statistics",
                table: "StatisticalResults",
                newName: "GraphData");
        }
    }
}
