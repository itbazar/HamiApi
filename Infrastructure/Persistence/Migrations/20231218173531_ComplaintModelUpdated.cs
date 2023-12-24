using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Infrastructure.Persistence
{
    /// <inheritdoc />
    public partial class ComplaintModelUpdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastActor",
                table: "Complaint");

            migrationBuilder.AddColumn<int>(
                name: "Visibility",
                table: "ComplaintContent",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Visibility",
                table: "ComplaintContent");

            migrationBuilder.AddColumn<int>(
                name: "LastActor",
                table: "Complaint",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
