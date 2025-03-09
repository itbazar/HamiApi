using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class DropTrahsTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Media_ComplaintContent_ComplaintContentId",
                table: "Media");

            migrationBuilder.DropTable(
                name: "ComplaintContent");

            migrationBuilder.DropTable(
                name: "News");

            migrationBuilder.DropTable(
                name: "Slider");

            migrationBuilder.DropTable(
                name: "WebContent");

            migrationBuilder.DropTable(
                name: "Complaint");

            migrationBuilder.DropTable(
                name: "ComplaintCategory");

            migrationBuilder.DropTable(
                name: "ComplaintOrganization");

            migrationBuilder.DropTable(
                name: "PublicKey");

            migrationBuilder.DropIndex(
                name: "IX_Media_ComplaintContentId",
                table: "Media");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ComplaintCategory",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComplaintCategory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ComplaintOrganization",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComplaintOrganization", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "News",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Image_AlternateText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Image_Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Image_MediaType = table.Column<int>(type: "int", nullable: false),
                    Image_Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Image_Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Image_Url2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Image_Url3 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Image_Url4 = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_News", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PublicKey",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InspectorId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Key = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false)
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

            migrationBuilder.CreateTable(
                name: "Slider",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Image_AlternateText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Image_Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Image_MediaType = table.Column<int>(type: "int", nullable: false),
                    Image_Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Image_Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Image_Url2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Image_Url3 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Image_Url4 = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Slider", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WebContent",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WebContent", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Complaint",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ComplaintOrganizationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PublicKeyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CipherKeyCitizen = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    CipherKeyInspector = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    Complaining = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EncryptionIv = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    EncryptionIvCitizen = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    IsFailed = table.Column<bool>(type: "bit", nullable: false),
                    LastChanged = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RegisteredAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ServerPassword = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TrackingNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CitizenPassword_Hash = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    CitizenPassword_Salt = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    EncryptionKeyPassword_Hash = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    EncryptionKeyPassword_Salt = table.Column<byte[]>(type: "varbinary(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Complaint", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Complaint_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Complaint_ComplaintCategory_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "ComplaintCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Complaint_ComplaintOrganization_ComplaintOrganizationId",
                        column: x => x.ComplaintOrganizationId,
                        principalTable: "ComplaintOrganization",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Complaint_PublicKey_PublicKeyId",
                        column: x => x.PublicKeyId,
                        principalTable: "PublicKey",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ComplaintContent",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Cipher = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    ComplaintId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IntegrityHash = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    IsEncrypted = table.Column<bool>(type: "bit", nullable: false),
                    Operation = table.Column<int>(type: "int", nullable: false),
                    Sender = table.Column<int>(type: "int", nullable: false),
                    Visibility = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComplaintContent", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ComplaintContent_Complaint_ComplaintId",
                        column: x => x.ComplaintId,
                        principalTable: "Complaint",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Media_ComplaintContentId",
                table: "Media",
                column: "ComplaintContentId");

            migrationBuilder.CreateIndex(
                name: "IX_Complaint_CategoryId",
                table: "Complaint",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Complaint_ComplaintOrganizationId",
                table: "Complaint",
                column: "ComplaintOrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_Complaint_PublicKeyId",
                table: "Complaint",
                column: "PublicKeyId");

            migrationBuilder.CreateIndex(
                name: "IX_Complaint_UserId",
                table: "Complaint",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ComplaintContent_ComplaintId",
                table: "ComplaintContent",
                column: "ComplaintId");

            migrationBuilder.CreateIndex(
                name: "IX_PublicKey_InspectorId",
                table: "PublicKey",
                column: "InspectorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Media_ComplaintContent_ComplaintContentId",
                table: "Media",
                column: "ComplaintContentId",
                principalTable: "ComplaintContent",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
