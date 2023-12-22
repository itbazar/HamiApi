using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ComplaintContentIdAddedToMediaExplicitly : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Media_ComplaintContent_ComplaintContentId",
                table: "Media");

            migrationBuilder.AlterColumn<Guid>(
                name: "ComplaintContentId",
                table: "Media",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ComplaintContent",
                table: "Media",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddForeignKey(
                name: "FK_Media_ComplaintContent_ComplaintContentId",
                table: "Media",
                column: "ComplaintContentId",
                principalTable: "ComplaintContent",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Media_ComplaintContent_ComplaintContentId",
                table: "Media");

            migrationBuilder.DropColumn(
                name: "ComplaintContent",
                table: "Media");

            migrationBuilder.AlterColumn<Guid>(
                name: "ComplaintContentId",
                table: "Media",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_Media_ComplaintContent_ComplaintContentId",
                table: "Media",
                column: "ComplaintContentId",
                principalTable: "ComplaintContent",
                principalColumn: "Id");
        }
    }
}
