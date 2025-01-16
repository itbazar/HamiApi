using Api.Abstractions;
using Api.Contracts.CounselingSessionContract;
using Api.Contracts.KeyManagement;
using Api.Contracts.Patient;
using Api.Contracts.TestPeriodContract;
using Api.ExtensionMethods;
using Application.Common.Interfaces.Persistence;
using Application.Setup.Commands.AddPublicKey;
using Application.Setup.Commands.ChangeInspectorKey;
using Application.Setup.Commands.EditChartNames;
using Application.Setup.Commands.GenerateKeyPair;
using Application.Setup.Commands.Init;
using Application.Setup.Queries.GetPublicKeys;
using Application.Users.Commands.ApprovedRegisterPatient;
using Application.Users.Common;
using Application.Users.Queries.GetMentors;
using Application.Users.Queries.GetPatients;
using Application.Users.Queries.GetPatientsSessionReport;
using Application.Users.Queries.GetPatientTestPeriodsReport;
using Application.Users.Queries.GetUserMedicalInfoById;
using Domain.Models.ComplaintAggregate.Encryption;
using Domain.Models.Hami;
using Domain.Models.IdentityAggregate;
using IpPanelSmsApi;
using MassTransit.Internals.GraphValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Text;

namespace Api.Controllers;

public class AdminController : ApiController
{
    public AdminController(ISender sender) : base(sender)
    {
    }

    [HttpGet("Init")]
    public async Task<IActionResult> Init()
    {
        var command = new InitCommand();
        var result = await Sender.Send(command);
        return result.Match(
            s => File(Encoding.ASCII.GetBytes(s), "text/plain", "private.txt"),
            f => Problem(f));
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("Mentors")]
    public async Task<ActionResult<List<PatientResponse>>> GetMentorsList([FromQuery] PagingInfo pagingInfo)
    {
        var command = new GetMentorsQuery(pagingInfo);
        //var result = await Sender.Send(command);
        //return result.Match(
        //s => Ok(s),
        //    f => Problem(f));

        var result = await Sender.Send(command);
        if (result.IsFailed)
            return Problem(result.ToResult());

        Response.AddPaginationHeaders(result.Value.Meta);

        var dtoList = result.Value.Select(user => new PatientResponse(
           user.Id,
           user.FirstName,
           user.LastName,
           user.UserName,
           user.DateOfBirth,
           user.UserGroupMemberships
               .Select(ugm => ugm.PatientGroup.Description) // گرفتن نام گروه از PatientGroup
               .FirstOrDefault() ?? "بدون گروه", // مدیریت کاربرانی که گروهی ندارند
           user.City,
           user.RegistrationStatus
       )).ToList();

        return Ok(dtoList);
    }

    [Authorize(Roles = "Admin,Patient,Mentor")]
    [HttpPost("SessionReport")]
    public async Task<ActionResult<List<CounselingSessionListItemDto>>> GetSessionReport([FromBody] string userId)
    {
        if (string.IsNullOrEmpty(userId)) // برای گزارشی که خود بیمار می‌گیرد
            userId = User.GetUserId();

        if (string.IsNullOrEmpty(userId))
            return Unauthorized("کاربر وارد نشده است.");

        var query = new GetPatientsSessionReportQuery(userId);
        var result = await Sender.Send(query);

        if (result.IsFailed)
            return Problem(result.ToResult());

        // تبدیل داده‌ها به مدل DTO
        var sessionDtos = result.Value
            .Select(cs =>
            {
                // پیدا کردن لاگ مربوط به کاربر
                var attendanceLog = cs.AttendanceLogs
                    .FirstOrDefault(al => al.UserId == userId);

                // بررسی وضعیت برگزاری جلسه
                //bool isSessionHeld = cs.ScheduledDate <= DateTime.UtcNow;

                return new CounselingSessionListItemDto(
                    cs.Id,
                    cs.PatientGroupId,
                    cs.PatientGroup.Description, // نام گروه
                    cs.MentorId ?? "",
                    cs.Mentor != null ? $"{cs.Mentor.FirstName} {cs.Mentor.LastName}" : "نامشخص", // نام منتور
                    cs.ScheduledDate,
                    cs.Topic,
                    cs.MeetingLink,
                    cs.IsConfirmed,
                    cs.IsConfirmed
                        ? attendanceLog?.MentorNote ?? "" // اگر جلسه برگزار شده، نوت منتور یا مقدار خالی
                        : "جلسه هنوز برگزار نشده", // اگر جلسه برگزار نشده باشد
                    cs.IsConfirmed ? attendanceLog?.Attended : null // اگر جلسه برگزار نشده باشد، وضعیت حضور نال است
                );
            })
            .ToList();

        return Ok(sessionDtos);
    }


    [Authorize(Roles = "Admin,Patient,Mento")]
    [HttpPost("MyTests")]
    public async Task<ActionResult<List<TestPeriodResponse>>> PatientTestPeriodsReport([FromBody] string userId)
    {
        if (string.IsNullOrEmpty(userId)) 
            userId = User.GetUserId();

        if (string.IsNullOrEmpty(userId))
            return Unauthorized("کاربر وارد نشده است.");

        var query = new GetPatientTestPeriodsReportQuery(userId);
        var result = await Sender.Send(query);

        if (result.IsFailed)
            return Problem(result.ToResult());

        return result.Match(
            tests => Ok(tests),
            errors => Problem(errors)
        );
    }


    [Authorize(Roles = "Admin,Mentor")]
    [HttpGet("Patients")]
    public async Task<ActionResult<List<PatientResponse>>> GetPatientList([FromQuery] PagingInfo pagingInfo, [FromQuery] RegistrationStatus? Status)
    {
        var currentUserId = User.GetUserId(); // گرفتن آی‌دی کاربر لاگین‌شده
        var command = new GetPatientsQuery(pagingInfo, Status, currentUserId);

        var result = await Sender.Send(command);
        if (result.IsFailed)
            return Problem(result.ToResult());

        Response.AddPaginationHeaders(result.Value.Meta);

        var dtoList = result.Value.Select(user => new PatientResponse(
           user.Id,
           user.FirstName,
           user.LastName,
           user.UserName,
           user.DateOfBirth,
           user.UserGroupMemberships
               .Select(ugm => ugm.PatientGroup.Description) // گرفتن نام گروه از PatientGroup
               .FirstOrDefault() ?? "بدون گروه", // مدیریت کاربرانی که گروهی ندارند
           user.City,
           user.RegistrationStatus
       )).ToList();

        return Ok(dtoList);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("approve-patient")]
    public async Task<IActionResult> ApprovedPatientGroup([FromBody] ApprovedPatientDto dto)
    {
        var command = new ApprovedRegisterPatientCommand(
            dto.UserId,
            dto.PatientGroupId,
            dto.IsApproved,
            dto.RejectionReason);

        var result = await Sender.Send(command);
        return result.Match(
            s => Ok(s),
            f => Problem(f));
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("patients/{userId}/medical-info")]
    public async Task<ActionResult<UserMedicalInfo>> GetUserMedicalInfoById(string userId)
    {
        // ایجاد Query
        var query = new GetUserMedicalInfoByIdQuery(userId);
        // ارسال Query به MediatR
        var result = await Sender.Send(query);
        // مدیریت نتیجه
        return result.Match(
            s => Ok(s),        // در صورت موفقیت، اطلاعات را برمی‌گرداند
            f => Problem(f));  // در صورت شکست، خطا را برمی‌گرداند
    }

    //[HttpGet("MentalAssessment")]
    //public async Task<ActionResult<MentalAssessmentResponse>> GetMentalAssessment(
    //[FromQuery] string userId,
    //[FromQuery] DateTime? startDate,
    //[FromQuery] DateTime? endDate)
    //{
    //    var query = _dbContext.TestPeriodResults
    //        .Where(r => r.UserId == userId);

    //    if (startDate.HasValue)
    //        query = query.Where(r => r.CreatedAt >= startDate.Value);

    //    if (endDate.HasValue)
    //        query = query.Where(r => r.CreatedAt <= endDate.Value);

    //    var results = await query
    //        .OrderBy(r => r.CreatedAt)
    //        .ToListAsync();

    //    var response = new MentalAssessmentResponse
    //    {
    //        Dates = results.Select(r => r.CreatedAt.ToString("yyyy-MM-dd")).ToList(),
    //        GadScores = results.Where(r => r.TestType == TestType.GAD).Select(r => r.TotalScore).ToList(),
    //        MddScores = results.Where(r => r.TestType == TestType.MDD).Select(r => r.TotalScore).ToList(),
    //    };

    //    return Ok(response);
    //}



    [Authorize(Roles = "Admin")]
    [HttpGet("GenerateKeyPair")]
    public async Task<ActionResult<AsymmetricKey>> GenerateKeyPair()
    {
        var command = new GenerateKeyPairCommand();
        var result = await Sender.Send(command);
        return result.Match(
            s => Ok(s),
            f => Problem(f));
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("GetKeys")]
    public async Task<ActionResult<List<PublicKeyResponse>>> GetKeys()
    {
        var command = new GetPublicKeysQuery();
        var result = await Sender.Send(command);
        return result.Match(
            s => Ok(s),
            f => Problem(f));
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("AddKey")]
    public async Task<ActionResult<bool>> AddKey([FromBody] AddPublicKeyDto addKeyDto)
    {
        var command = new AddPublicKeyCommand(addKeyDto.Title, addKeyDto.PublicKey);
        var result = await Sender.Send(command);
        return result.Match(
            s => Ok(s),
            f => Problem(f));
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("ChangeKey")]
    public async Task<ActionResult<ChangeInspectorKeyResponse>> ChangeKey([FromBody] ChangeInspectorKeyDto changeKeyDto)
    {
        var command = new ChangeInspectorKeyCommand(
            changeKeyDto.PrivateKey,
            changeKeyDto.PublicKeyId,
            changeKeyDto.IsPolling);
        var result = await Sender.Send(command);
        return result.Match(
            s => Ok(s), 
            f => Problem(f));
    }

    [Authorize(Roles = "PowerUser")]
    [HttpPost("EditChartNames")]
    public async Task<ActionResult> EditChartNames()
    {
        var command = new EditChartNamesCommand();
        var result = await Sender.Send(command);
        return result.Match(
            s => Ok(s),
            f => Problem(f));
    }

}
