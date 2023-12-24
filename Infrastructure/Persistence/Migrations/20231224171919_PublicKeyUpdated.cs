using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class PublicKeyUpdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "PublicKey",
                newName: "IsActive");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "PublicKey",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Title",
                table: "PublicKey");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "PublicKey",
                newName: "IsDeleted");
        }
    }
}
