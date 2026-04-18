using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MeritMatch.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddResearchAreaToApplicationUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ResearchArea",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ResearchArea",
                table: "AspNetUsers");
        }
    }
}
