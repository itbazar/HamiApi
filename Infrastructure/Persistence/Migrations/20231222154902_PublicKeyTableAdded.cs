using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class PublicKeyTableAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "PublicKeyId",
                table: "Complaint",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "PublicKey",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Key = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InspectorId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PublicKey", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PublicKey_AspNetUsers_InspectorId",
                        column: x => x.InspectorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Complaint_PublicKeyId",
                table: "Complaint",
                column: "PublicKeyId");

            migrationBuilder.CreateIndex(
                name: "IX_PublicKey_InspectorId",
                table: "PublicKey",
                column: "InspectorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Complaint_PublicKey_PublicKeyId",
                table: "Complaint",
                column: "PublicKeyId",
                principalTable: "PublicKey",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Complaint_PublicKey_PublicKeyId",
                table: "Complaint");

            migrationBuilder.DropTable(
                name: "PublicKey");

            migrationBuilder.DropIndex(
                name: "IX_Complaint_PublicKeyId",
                table: "Complaint");

            migrationBuilder.DropColumn(
                name: "PublicKeyId",
                table: "Complaint");
        }
    }
}
