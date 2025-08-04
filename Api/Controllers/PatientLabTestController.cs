using Api.Abstractions;
using Api.Contracts.PatientLabTestContract;
using Api.ExtensionMethods;
using Application.Common.Interfaces.Persistence;
using Application.PatientLabTests.Queries.GetPatientLabTestByIdQuery;
using Application.PatientLabTests.Commands.AddPatientLabTestCommand;
using Application.PatientLabTests.Queries.GetPatientLabTestQuery;
using Domain.Models.Hami;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Infrastructure.Persistence;
using System.Security.Claims;

namespace Api.Controllers;

[Authorize]
public class PatientLabTestController : ApiController
{
    public PatientLabTestController(ISender sender) : base(sender)
    {
    }

    /// <summary>
    /// دریافت لیست آزمایش‌های بیماران
    /// </summary>
    [Authorize(Roles = "Admin,Patient,Mentor")]
    [HttpGet]
    public async Task<ActionResult<List<PatientLabTestListItemDto>>> GetPatientLabTestList([FromQuery] PagingInfo pagingInfo)
    {
        var currentUserId = User.GetUserId(); // گرفتن آی‌دی کاربر لاگین‌شده

        var query = new GetPatientLabTestQuery(pagingInfo, currentUserId);
        var result = await Sender.Send(query);
        if (result.IsFailed)
            return Problem(result.ToResult());

        Response.AddPaginationHeaders(result.Value.Meta);

        // مپ کردن مقادیر به PatientLabTestListItemDto
        var dtoList = result.Value.Select(x => new PatientLabTestListItemDto(
            x.Id,
            x.UserId,
            x.User?.UserName ?? "نامشخص", // نام کاربر
            x.TestType, // نوع آزمایش
            x.TestValue, // مقدار آزمایش
            x.Unit, // واحد آزمایش
            x.CreatedAt,
            x.TestDate

        )).ToList();

        return Ok(dtoList);
    }

    /// <summary>
    /// دریافت آزمایش بیمار بر اساس ID
    /// </summary>
    [Authorize(Roles = "Admin,Patient,Mentor")]
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<PatientLabTest>> GetPatientLabTest(Guid id)
    {
        var query = new GetPatientLabTestByIdQuery(id);
        var result = await Sender.Send(query);
        return result.Match(
            s => Ok(s),
            f => Problem(f));
    }

    /// <summary>
    /// ثبت آزمایش جدید توسط بیمار یا ادمین
    /// </summary>
    [Authorize(Roles = "Admin,Patient,Mentor")]
    [HttpPost]
    public async Task<ActionResult> AddPatientLabTest([FromBody] AddPatientLabTestDto patientLabTestDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        // اگر UserId در ورودی خالی باشد، از User.GetUserId() مقدار بگیر
        var userId = string.IsNullOrEmpty(patientLabTestDto.UserId)
            ? User.GetUserId()
            : patientLabTestDto.UserId;

        if (string.IsNullOrEmpty(userId)) // بررسی اینکه آیا UserId معتبر است
            return Unauthorized("UserId is required.");

        // ایجاد Command با UserId مناسب
        var command = new AddPatientLabTestCommand(
            userId,
            patientLabTestDto.TestType,
            patientLabTestDto.TestValue,
            patientLabTestDto.Unit
        );

        // ارسال Command به Handler
        var result = await Sender.Send(command);

        // بررسی نتیجه
        return result.Match(
            s => CreatedAtAction(nameof(GetPatientLabTest), new { id = s.Id }, s),
            f => Problem(f));
    }
}
