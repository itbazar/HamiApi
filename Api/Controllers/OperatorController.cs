using Api.Abstractions;
using Api.Contracts.Patient;
using Api.ExtensionMethods;
using Application.Common.Interfaces.Persistence;
using Application.Users.Commands.ApprovedRegisterPatient;
using Application.Users.Queries.GetPatients;
using Application.Users.Queries.GetUserMedicalInfoById;
using Domain.Models.Hami;
using Domain.Models.IdentityAggregate;
using MassTransit.Mediator;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class OperatorController : ApiController
{
    public OperatorController(ISender sender) : base(sender)
    {
    }

    [Authorize(Roles = "Operator")]
    [HttpGet("Patients")]
    public async Task<ActionResult<List<ApplicationUser>>> GetPatientList([FromQuery] PagingInfo pagingInfo)
    {
        var command = new GetPatientsQuery(pagingInfo);
        var result = await Sender.Send(command);
        return result.Match(
            s => Ok(s),
            f => Problem(f));
    }

    [Authorize(Roles = "Operator")]
    [HttpPost("approve-patient")]
    public async Task<IActionResult> ApprovedPatientGroup([FromForm] ApprovedPatientDto dto)
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

    [Authorize(Roles = "Operator")]
    [HttpGet("patients/{userId}/medical-info")]
    public async Task<ActionResult<UserMedicalInfo>> GetUserMedicalInfoById(string userId)
    {
        // ایجاد Query
        var query = new GetUserMedicalInfoByIdQuery(userId);
        //if (userMedicalInfo == null)
        //    return NotFound();

        //var dto = new UserMedicalInfoDto
        //{
        //    UserId = userMedicalInfo.UserId,
        //    Organ = userMedicalInfo.Organ.ToString(), // یا به صورت رشته ثابت
        //    DiseaseType = userMedicalInfo.DiseaseType.GetDescription(),
        //    PatientStatus = userMedicalInfo.PatientStatus.GetDescription()
        //};
        // ارسال Query به MediatR
        var result = await Sender.Send(query);

        // مدیریت نتیجه
        return result.Match(
            s => Ok(s),        // در صورت موفقیت، اطلاعات را برمی‌گرداند
            f => Problem(f));  // در صورت شکست، خطا را برمی‌گرداند
    }

    //public async Task<ActionResult<List<CounselingSessionListItemDto>>> GetCounselingSessionList([FromQuery] PagingInfo pagingInfo)
    //{
    //    var query = new GetCounselingSessionQuery(pagingInfo);
    //    var result = await Sender.Send(query);
    //    if (result.IsFailed)
    //        return Problem(result.ToResult());

    //    Response.AddPaginationHeaders(result.Value.Meta);
    //    return Ok(result.Value.Adapt<List<CounselingSessionListItemDto>>());
    //}

    //[Authorize(Roles = "Admin")]
    //[HttpGet("{id:guid}")]
    //public async Task<ActionResult<Disease>> Get(Guid id)
    //{
    //    var command = new GetDiseaseByIdQuery(id);
    //    var result = await Sender.Send(command);
    //    return result.Match(
    //        s => Ok(s),
    //        f => Problem(f));
    //}
}
