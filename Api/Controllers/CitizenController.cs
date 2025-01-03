using Api.Abstractions;
using Api.Contracts.Complaint;
using Api.Contracts.Patient;
using Api.ExtensionMethods;
using Application.Common.Interfaces.Persistence;
using Application.Complaints.Commands.AddComplaintCommand;
using Application.Complaints.Commands.ReplyComplaintCitizenCommand;
using Application.Complaints.Common;
using Application.Complaints.Queries.GetComplaintCitizenQuery;
using Application.Complaints.Queries.GetComplaintListQuery;
using Application.Users.Commands.RegisterPatient;
using Infrastructure.Options;
using MassTransit.Mediator;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Serilog;

namespace Api.Controllers;

public class CitizenController : ApiController
{
    private readonly StorageOptions _storageOptions;
    public CitizenController(ISender sender, IOptions<StorageOptions> storageOptions) : base(sender)
    {
        _storageOptions = storageOptions.Value;
    }

    //[HttpPost]
    //public async Task<IActionResult> AddComplaint([FromForm] ComplaintCreateDto createDto)
    //{
    //    var command = new AddComplaintCommand(
    //        null,
    //        createDto.Title,
    //        createDto.Text,
    //        createDto.CategoryId,
    //        createDto.Medias.GetMedia(_storageOptions),
    //        createDto.Captcha,
    //        createDto.Complaining,
    //        createDto.OrganizationId);
    //    var result = await Sender.Send(command);

    //    return result.Match(
    //        s => Ok(s),
    //        f => Problem(f));
    //}


    [HttpPost("RegisterPatient")]
    public async Task<IActionResult> RegisterPatient([FromForm] RegisterPatientDto patientDto)
    {
        var command = new RegisterPatientCommand(
            patientDto.Username,
            patientDto.Password,
            patientDto.PhoneNumber,
            patientDto.NationalId,
            patientDto.FirstName,
            patientDto.LastName,
            patientDto.DateOfBirth,
            patientDto.Gender,
            patientDto.Education,
            patientDto.City,
            patientDto.Organ,
            patientDto.DiseaseType,
            patientDto.PatientStatus,
            patientDto.Stage,
            patientDto.PathologyDiagnosis,
            patientDto.InitialWeight,
            patientDto.SleepDuration,
            patientDto.AppetiteLevel,
            patientDto.GADScore,
            patientDto.MDDScore);

        var result = await Sender.Send(command);

        return result.Match(
            s => Ok(s),
            f => Problem(f));
    }

    [Authorize]
    [HttpPost("Authorized")]
    public async Task<IActionResult> AddComplaintAuthorized([FromForm] ComplaintCreateDto createDto)
    {
        var command = new AddComplaintCommand(
            User.GetUserId(),
            createDto.Title,
            createDto.Text,
            createDto.CategoryId,
            createDto.Medias.GetMedia(_storageOptions),
            createDto.Captcha,
            createDto.Complaining,
            createDto.OrganizationId);
        var result = await Sender.Send(command);

        return result.Match(
            s => Ok(s),
            f => Problem(f));
    }

    [HttpPost("Operate")]
    public async Task<IActionResult> Operate([FromForm] ComplaintOperateCitizenDto operateDto)
    {
        var command = new ReplyComplaintCitizenCommand(
            operateDto.TrackingNumber,
            operateDto.Text,
            operateDto.Medias.GetMedia(_storageOptions),
            operateDto.Operation,
            operateDto.Password);
        var result = await Sender.Send(command);

        return result.Match(
            s => Ok(s),
            f => Problem(f));
    }

    [HttpPost("Get")]
    public async Task<ActionResult<ComplaintCitizenResponse>> GetComplaintCitizen([FromBody] ComplaintCitizenGetDto complaintDto)
    {
        var query = new GetComplaintCitizenQuery(complaintDto.TrackingNumber, complaintDto.Password);
        var result = await Sender.Send(query);
        return result.Match(
            s => Ok(s),
            f => Problem(f));
    }

    [Authorize]
    [HttpGet("List")]
    public async Task<ActionResult<List<ComplaintListCitizenResponse>>> List(
        [FromQuery] PagingInfo pagingInfo,
        [FromQuery] ComplaintListFilters filters)
    {
        var userId = User.GetUserId();
        var query = new GetComplaintListCitizenQuery(pagingInfo, filters, userId);
        var result = await Sender.Send(query);
        if (result.IsFailed)
            return Problem(result.ToResult());

        Response.AddPaginationHeaders(result.Value.Meta);
        return Ok(result.Value);
    }
}
