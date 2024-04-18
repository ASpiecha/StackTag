using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StackTag.Migrations
{
    /// <inheritdoc />
    public partial class PercentageAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Percentage",
                table: "Tags",
                type: "float",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Percentage",
                table: "Tags");
        }
    }
}
