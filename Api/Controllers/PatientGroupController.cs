using Api.Abstractions;
using Api.Contracts.CounselingSessionContract;
using Api.Contracts.PatientGroupContract;
using Api.Contracts.QuestionContract;
using Api.ExtensionMethods;
using Application.Common.Interfaces.Persistence;
using Application.PatientGroupApp.Queries.GetPatientGroupByIdQuery;
using Application.PatientGroups.Commands.AddPatientGroupCommand;
using Application.PatientGroups.Commands.DeletePatientGroupCommand;
using Application.PatientGroups.Commands.UpdatePatientGroupCommand;
using Application.PatientGroups.Queries.GetPatientGroupQuery;
using Application.Questions.Queries.GetMentorPatientGroupsQuery;
using Application.Questions.Queries.GetQuestionByTestPeriodIdQuery;
using Domain.Models.Hami;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Authorize(Roles = "Admin,Mentor")]
public class PatientGroupController : ApiController
{
    public PatientGroupController(ISender sender) : base(sender)
    {
    }

    [HttpGet]
    public async Task<ActionResult<List<PatientGroupListItemDto>>> GetPatientGroupList([FromQuery] PagingInfo pagingInfo)
    {
        var query = new GetPatientGroupQuery(pagingInfo);
        var result = await Sender.Send(query);
        if (result.IsFailed)
            return Problem(result.ToResult());

        Response.AddPaginationHeaders(result.Value.Meta);
        var dtoList = result.Value.Select(x => new PatientGroupListItemDto(
            x.Id,
            x.Organ,
            x.DiseaseType,
            x.Stage.Value,
            x.Description,
            x.MentorId,
            x.Mentor.FirstName + " " + x.Mentor.LastName ?? "نامشخص"
        )).ToList();

        return Ok(dtoList);
    }

    [Authorize(Roles = "Mentor")]
    [HttpGet("MentorGroups")]
    public async Task<ActionResult<List<PatientGroupListItemDto>>> GetMentorGroups()
    {
        var userId = User.GetUserId();
        var query = new GetMentorPatientGroupsQuery(userId);
        var result = await Sender.Send(query);
        if (result.IsFailed)
            return Problem(result.ToResult());

        return Ok(result.Value.Adapt<List<PatientGroupListItemDto>>());
    }


    [HttpGet("{id:guid}")]
    public async Task<ActionResult<PatientGroup>> GetPatientGroup(Guid id)
    {
        var query = new GetPatientGroupByIdQuery(id);
        var result = await Sender.Send(query);
        return result.Match(
            s => Ok(s),
            f => Problem(f));
    }

    [HttpPost]
    public async Task<ActionResult> AddPatientGroup([FromForm] AddPatientGroupDto patientGroupDto)
    {
        var command = new AddPatientGroupCommand(
            patientGroupDto.Organ,
            patientGroupDto.DiseaseType,
            patientGroupDto.Stage,
            patientGroupDto.Description,
            patientGroupDto.MentorId);

        var result = await Sender.Send(command);
        return result.Match(
            s => CreatedAtAction(nameof(GetPatientGroup), new { id = s.Id }, s),
            f => Problem(f));
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult> UpdatePatientGroup(Guid id, [FromForm] UpdatePatientGroupDto patientGroupDto)
    {
        var command = new UpdatePatientGroupCommand(
            id,
            patientGroupDto.Organ,
            patientGroupDto.DiseaseType,
            patientGroupDto.Stage,
            patientGroupDto.Description,
            patientGroupDto.MentorId);

        var result = await Sender.Send(command);
        return result.Match(
            s => NoContent(),
            f => Problem(f));
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> DeletePatientGroup(Guid id)
    {
        var command = new DeletePatientGroupCommand(id);

        var result = await Sender.Send(command);
        return result.Match(
            s => NoContent(),
            f => Problem(f));
    }
}
