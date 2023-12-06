using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Initial3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Password_Salt",
                table: "Complaint",
                newName: "EncryptionKeyPassword_Salt");

            migrationBuilder.RenameColumn(
                name: "Password_Hash",
                table: "Complaint",
                newName: "EncryptionKeyPassword_Hash");

            migrationBuilder.AddColumn<byte[]>(
                name: "CitizenPassword_Hash",
                table: "Complaint",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<byte[]>(
                name: "CitizenPassword_Salt",
                table: "Complaint",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CitizenPassword_Hash",
                table: "Complaint");

            migrationBuilder.DropColumn(
                name: "CitizenPassword_Salt",
                table: "Complaint");

            migrationBuilder.RenameColumn(
                name: "EncryptionKeyPassword_Salt",
                table: "Complaint",
                newName: "Password_Salt");

            migrationBuilder.RenameColumn(
                name: "EncryptionKeyPassword_Hash",
                table: "Complaint",
                newName: "Password_Hash");
        }
    }
}
