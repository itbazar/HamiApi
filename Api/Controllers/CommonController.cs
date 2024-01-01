using Api.Abstractions;
using Application.Authentication.Queries.CaptchaQuery;
using Application.ComplaintCategories.Queries.GetComplaintCategoriesQuery;
using Application.ComplaintOrganizations.Queries.GetComplaintOrganizationQuery;
using Domain.Models.ComplaintAggregate;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class CommonController : ApiController
{
    public CommonController(ISender sender) : base(sender)
    {
    }

    [HttpGet("Categories")]
    public async Task<ActionResult<List<ComplaintCategory>>> GetCategories()
    {
        var query = new GetComplaintCategoriesQuery();
        var result = await Sender.Send(query);
        return Ok(result);
    }

    [HttpGet("Organizations")]
    public async Task<ActionResult<List<ComplaintOrganization>>> GetOrganizations()
    {
        var query = new GetComplaintOrganizationsQuery();
        var result = await Sender.Send(query);
        return Ok(result);
    }

    [HttpGet("Captcha")]
    public async Task<ActionResult<string>> GetCaptcha()
    {
        var query = new CaptchaQuery();
        var result = await Sender.Send(query);
        if (result is null)
            throw new Exception();
        Response.Headers.Append("Captcha-Key", result.Key.ToString());
        //return "data:image/jpg;base64," + Convert.ToBase64String(result.Data);
        return File(result.Data, "image/jpg");
    }
}
