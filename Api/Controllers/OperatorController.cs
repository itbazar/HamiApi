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
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Api.Controllers;

public class OperatorController : ApiController
{
    public OperatorController(ISender sender) : base(sender)
    {
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
