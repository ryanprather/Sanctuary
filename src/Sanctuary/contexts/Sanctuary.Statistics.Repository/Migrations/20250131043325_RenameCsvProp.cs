using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sanctuary.Statistics.Repository.Migrations
{
    /// <inheritdoc />
    public partial class RenameCsvProp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DataBlobUri",
                schema: "Statistics",
                table: "StatisticalResults",
                newName: "CsvDataUri");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CsvDataUri",
                schema: "Statistics",
                table: "StatisticalResults",
                newName: "DataBlobUri");
        }
    }
}
