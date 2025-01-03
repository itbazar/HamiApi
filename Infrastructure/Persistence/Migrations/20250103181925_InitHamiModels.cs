using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitHamiModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfBirth",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "Education",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Gender",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RegistrationStatus",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "RejectionReason",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PatientGroup",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Organ = table.Column<int>(type: "int", nullable: false),
                    DiseaseType = table.Column<int>(type: "int", nullable: false),
                    Stage = table.Column<int>(type: "int", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MentorId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientGroup", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PatientGroup_AspNetUsers_MentorId",
                        column: x => x.MentorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Question",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TestType = table.Column<int>(type: "int", nullable: false),
                    QuestionText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Question", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TestPeriod",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TestType = table.Column<int>(type: "int", nullable: false),
                    PeriodName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Code = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestPeriod", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserMedicalInfo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Organ = table.Column<int>(type: "int", nullable: false),
                    DiseaseType = table.Column<int>(type: "int", nullable: false),
                    PatientStatus = table.Column<int>(type: "int", nullable: false),
                    Stage = table.Column<int>(type: "int", nullable: true),
                    PathologyDiagnosis = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InitialWeight = table.Column<float>(type: "real", nullable: true),
                    SleepDuration = table.Column<int>(type: "int", nullable: true),
                    AppetiteLevel = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserMedicalInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserMedicalInfo_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CounselingSession",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PatientGroupId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ScheduledDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Topic = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MeetingLink = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MentorId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    MentorNote = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CounselingSession", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CounselingSession_AspNetUsers_MentorId",
                        column: x => x.MentorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CounselingSession_PatientGroup_PatientGroupId",
                        column: x => x.PatientGroupId,
                        principalTable: "PatientGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserGroupMembership",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PatientGroupId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserGroupMembership", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserGroupMembership_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserGroupMembership_PatientGroup_PatientGroupId",
                        column: x => x.PatientGroupId,
                        principalTable: "PatientGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Answer",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    QuestionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AnswerValue = table.Column<int>(type: "int", nullable: false),
                    TestPeriodId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AnswerDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Answer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Answer_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Answer_Question_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Question",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Answer_TestPeriod_TestPeriodId",
                        column: x => x.TestPeriodId,
                        principalTable: "TestPeriod",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TestPeriodResult",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    TestType = table.Column<int>(type: "int", nullable: false),
                    TotalScore = table.Column<int>(type: "int", nullable: false),
                    TestPeriodId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestPeriodResult", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TestPeriodResult_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TestPeriodResult_TestPeriod_TestPeriodId",
                        column: x => x.TestPeriodId,
                        principalTable: "TestPeriod",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MedicalEntry",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserMedicalInfoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    EntryType = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AttachedFilePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicalEntry", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MedicalEntry_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MedicalEntry_UserMedicalInfo_UserMedicalInfoId",
                        column: x => x.UserMedicalInfoId,
                        principalTable: "UserMedicalInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SessionAttendanceLog",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CounselingSessionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Attended = table.Column<bool>(type: "bit", nullable: false),
                    MentorNote = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SessionAttendanceLog", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SessionAttendanceLog_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SessionAttendanceLog_CounselingSession_CounselingSessionId",
                        column: x => x.CounselingSessionId,
                        principalTable: "CounselingSession",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Question",
                columns: new[] { "Id", "IsDeleted", "QuestionText", "TestType" },
                values: new object[,]
                {
                    { new Guid("1ea79543-e61a-4d7c-a4c5-d879da4d1f71"), false, "به سهولت عصبی یا بی قرار می شوم", 1 },
                    { new Guid("249a84d8-64ad-4a41-b68f-7d0f175e9b92"), false, "اختلال خواب (به سختی خواب میروم، در خواب بیدار می شوم و یا خیلی زیاد می خوابم)", 2 },
                    { new Guid("32d9f476-7bbc-4136-b899-4962982cca5c"), false, "اختلال در اشتها (اشتهایم کم شده ویا زیاد غذا می خورم)", 2 },
                    { new Guid("34b900ca-53f3-4322-b06c-300e54f0cd95"), false, "بی قراری شدید به حدی که نمی توانم بنشینم", 1 },
                    { new Guid("3a55fbe0-b8f6-4f41-bca1-66e565b78442"), false, "احساس خستگی، پایین بودن انرژی دارم", 2 },
                    { new Guid("59b47353-e8b0-4f1f-a0da-53bdbc4ed8a0"), false, "تمرکز در انجام کارها ندارم مثلا زمانی که مطالعه میکنم یا تلویزیون میبینم", 2 },
                    { new Guid("5b32abcf-3506-4ebc-959b-f2a941511ef5"), false, "علاقه یا لذت کم در اجرای کار ها (علاقه یا لذت کمی برای انجام دادن کار ها دارم)", 2 },
                    { new Guid("5f76a2a9-eb8a-40af-ac55-d60d31bb82d7"), false, "حرکات یا صحبت کردنم به قدری آهسته است که دیگران متوجه آن می شوند یا برعکس آنقدر بی قرارم که خیلی بیشتر از حد معمول در حرکتم", 2 },
                    { new Guid("60c8f10d-0496-4254-b8c1-99ff0b662d8f"), false, "احساس افسردگی، مود پایین یا نا امیدی دارم", 2 },
                    { new Guid("69a6b0fc-07a6-48e7-be89-62dac9bc198a"), false, "احساس بدی نسبت به خود دارم، احساس شکست میکنم، احساس میکنم خودم یا خانواده ام را ناامید کرده ام", 2 },
                    { new Guid("8dda7f6e-cc89-4638-8f48-2453b9240132"), false, "افکاری در مورد مردن یا آسیب زدن به خود به سراغم می آید", 2 },
                    { new Guid("9b991b96-19e1-4f30-a699-56a977a20b34"), false, "ترس این رو دارم که هر لحظه اتفاق بدی بیوفتد", 1 },
                    { new Guid("d061d84d-6dce-4d27-81fb-2f100a18a375"), false, "نگرانی بیش از حد پیرامون مسائل مختلف", 1 },
                    { new Guid("d4debcde-5a59-4f3f-b568-b8375bc3a19f"), false, "ناتوانی در توقف یا کنترل نگرانی", 1 },
                    { new Guid("ecc379cf-0ef3-4498-a865-6c320369f66a"), false, "داشتن احساس بی قراری، خشم، اضطراب، عصبانیت", 1 },
                    { new Guid("fbe74744-276c-4470-8aec-45488b1ae921"), false, "اشکال در آرامش داشتن (عدم توانایی در حفظ آرامش خود)", 1 }
                });

            migrationBuilder.InsertData(
                table: "TestPeriod",
                columns: new[] { "Id", "Code", "EndDate", "IsDeleted", "PeriodName", "StartDate", "TestType" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), 101, new DateTime(2099, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "ارزیابی اولیه GAD هنگام ثبت نام", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { new Guid("22222222-2222-2222-2222-222222222222"), 102, new DateTime(2099, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "ارزیابی اولیه MDD هنگام ثبت نام", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Answer_QuestionId",
                table: "Answer",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_Answer_TestPeriodId",
                table: "Answer",
                column: "TestPeriodId");

            migrationBuilder.CreateIndex(
                name: "IX_Answer_UserId",
                table: "Answer",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CounselingSession_MentorId",
                table: "CounselingSession",
                column: "MentorId");

            migrationBuilder.CreateIndex(
                name: "IX_CounselingSession_PatientGroupId",
                table: "CounselingSession",
                column: "PatientGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicalEntry_CreatedById",
                table: "MedicalEntry",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_MedicalEntry_UserMedicalInfoId",
                table: "MedicalEntry",
                column: "UserMedicalInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientGroup_MentorId",
                table: "PatientGroup",
                column: "MentorId");

            migrationBuilder.CreateIndex(
                name: "IX_SessionAttendanceLog_CounselingSessionId",
                table: "SessionAttendanceLog",
                column: "CounselingSessionId");

            migrationBuilder.CreateIndex(
                name: "IX_SessionAttendanceLog_UserId",
                table: "SessionAttendanceLog",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TestPeriodResult_TestPeriodId",
                table: "TestPeriodResult",
                column: "TestPeriodId");

            migrationBuilder.CreateIndex(
                name: "IX_TestPeriodResult_UserId",
                table: "TestPeriodResult",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserGroupMembership_PatientGroupId",
                table: "UserGroupMembership",
                column: "PatientGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_UserGroupMembership_UserId",
                table: "UserGroupMembership",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserMedicalInfo_UserId",
                table: "UserMedicalInfo",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Answer");

            migrationBuilder.DropTable(
                name: "MedicalEntry");

            migrationBuilder.DropTable(
                name: "SessionAttendanceLog");

            migrationBuilder.DropTable(
                name: "TestPeriodResult");

            migrationBuilder.DropTable(
                name: "UserGroupMembership");

            migrationBuilder.DropTable(
                name: "Question");

            migrationBuilder.DropTable(
                name: "UserMedicalInfo");

            migrationBuilder.DropTable(
                name: "CounselingSession");

            migrationBuilder.DropTable(
                name: "TestPeriod");

            migrationBuilder.DropTable(
                name: "PatientGroup");

            migrationBuilder.DropColumn(
                name: "City",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "DateOfBirth",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Education",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Gender",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "RegistrationStatus",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "RejectionReason",
                table: "AspNetUsers");
        }
    }
}
