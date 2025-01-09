using Api.Abstractions;
using Api.Contracts.KeyManagement;
using Api.Contracts.Patient;
using Api.ExtensionMethods;
using Application.Common.Interfaces.Persistence;
using Application.Complaints.Common;
using Application.Setup.Commands.AddPublicKey;
using Application.Setup.Commands.ChangeInspectorKey;
using Application.Setup.Commands.EditChartNames;
using Application.Setup.Commands.GenerateKeyPair;
using Application.Setup.Commands.Init;
using Application.Setup.Queries.GetPublicKeys;
using Application.Users.Commands.ApprovedRegisterPatient;
using Application.Users.Queries.GetPatients;
using Application.Users.Queries.GetUserMedicalInfoById;
using Domain.Models.ComplaintAggregate.Encryption;
using Domain.Models.Hami;
using Domain.Models.IdentityAggregate;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
    [HttpGet("Patients")]
    public async Task<ActionResult<List<PatientResponse>>> GetPatientList([FromQuery] PagingInfo pagingInfo, [FromQuery] RegistrationStatus? Status)
    {
        var command = new GetPatientsQuery(pagingInfo, Status);
        //var result = await Sender.Send(command);
        //return result.Match(
        //s => Ok(s),
        //    f => Problem(f));

        var result = await Sender.Send(command);
        if (result.IsFailed)
            return Problem(result.ToResult());

        Response.AddPaginationHeaders(result.Value.Meta);
        return Ok(result.Value);

        //// مپ کردن مقادیر به TestPeriodResultListItemDto
        //var dtoList = result.Value.Select(x => new PatientResponse(
        //    x.Id,
        //    x.FirstName,
        //    x.LastName,
        //    x.UserName,
        //    x..Description ?? "نامشخص",
        //    x.MentorId,
        //    x.Mentor.FirstName + " " + x.Mentor.LastName ?? "نامشخص", // نام کاربر
        //    x.ScheduledDate,
        //    x.Topic,
        //    x.MeetingLink,
        //    x.MentorNote
        //)).ToList();

        //return Ok(dtoList);
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
