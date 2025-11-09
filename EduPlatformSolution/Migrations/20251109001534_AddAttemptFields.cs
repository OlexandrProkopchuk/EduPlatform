using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduPlatformSolution.Migrations
{
    /// <inheritdoc />
    public partial class AddAttemptFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MaxScore",
                table: "Attempts",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaxScore",
                table: "Attempts");
        }
    }
}
