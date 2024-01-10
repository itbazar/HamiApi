using Api.Abstractions;
using Application.Authentication.Queries.CaptchaQuery;
using Application.ComplaintCategories.Queries.GetComplaintCategoriesQuery;
using Application.ComplaintOrganizations.Queries.GetComplaintOrganizationQuery;
using Application.NewsApp.Queries.GetNewsQuery;
using Application.Sliders.Queries.GetSlidersQuery;
using Application.WebContents.Queries.GetWebContentByTitleQuery;
using Application.WebContents.Queries.GetWebContentsQuery;
using Domain.Models.ComplaintAggregate;
using Domain.Models.News;
using Domain.Models.Sliders;
using Domain.Models.WebContents;
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

    [HttpGet("Sliders")]
    public async Task<ActionResult<List<Slider>>> GetSlidersList()
    {
        var query = new GetSlidersQuery();
        return await Sender.Send(query);
    }

    [HttpGet("News")]
    public async Task<ActionResult<List<News>>> GetNewsList()
    {
        var query = new GetNewsQuery();
        return await Sender.Send(query);
    }

    [HttpGet("Contents")]
    public async Task<ActionResult<List<WebContent>>> GetContentsList()
    {
        var query = new GetWebContentsQuery();
        return await Sender.Send(query);
    }

    [HttpGet("Contents/{title}")]
    public async Task<ActionResult<WebContent>> GetContentByTitle(string title)
    {
        var query = new GetWebContentByTitleQuery(title);
        return await Sender.Send(query);
    }
}
