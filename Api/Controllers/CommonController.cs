using Api.Abstractions;
using Api.Contracts.NewsContract;
using Api.ExtensionMethods;
using Application.Authentication.Queries.CaptchaQuery;
using Application.Common.Interfaces.Persistence;
using Application.ComplaintCategories.Queries.GetComplaintCategoriesQuery;
using Application.ComplaintOrganizations.Queries.GetComplaintOrganizationQuery;
using Application.NewsApp.Queries.GetNewsByIdQuery;
using Application.NewsApp.Queries.GetNewsQuery;
using Application.Sliders.Queries.GetSlidersQuery;
using Application.WebContents.Queries.GetWebContentByTitleQuery;
using Application.WebContents.Queries.GetWebContentsQuery;
using Domain.Models.ComplaintAggregate;
using Domain.Models.News;
using Domain.Models.Sliders;
using Domain.Models.WebContents;
using Infrastructure.Options;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Api.Controllers;

public class CommonController : ApiController
{
    private readonly StorageOptions _storageOptions;
    public CommonController(ISender sender, IOptions<StorageOptions> storageOptions) : base(sender)
    {
        _storageOptions = storageOptions.Value;
    }

    [HttpGet("Categories")]
    public async Task<ActionResult<List<ComplaintCategory>>> GetCategories()
    {
        var query = new GetComplaintCategoriesQuery();
        var result = await Sender.Send(query);
        return result.Match(
            s => Ok(s),
            f => Problem(f));
    }

    [HttpGet("Organizations")]
    public async Task<ActionResult<List<ComplaintOrganization>>> GetOrganizations()
    {
        var query = new GetComplaintOrganizationsQuery();
        var result = await Sender.Send(query);
        return result.Match(
            s => Ok(s),
            f => Problem(f));
    }

    [HttpGet("Captcha")]
    public async Task<ActionResult<string>> GetCaptcha()
    {
        var query = new CaptchaQuery();
        var result = await Sender.Send(query);
        if (result is null)
            throw new Exception();
        Response.Headers.Append("Captcha-Key", result.Value.Key.ToString());
        //return "data:image/jpg;base64," + Convert.ToBase64String(result.Data);
        return result.Match(
            s => File(s.Data, "image/jpg"),
            f => Problem(f));
    }

    [HttpGet("Sliders")]
    public async Task<ActionResult<List<Slider>>> GetSlidersList()
    {
        var query = new GetSlidersQuery();
        var result = await Sender.Send(query);
        return result.Match(
            s => Ok(s),
            f => Problem(f));
    }

    [HttpGet("News")]
    public async Task<ActionResult<List<NewsListItemDto>>> GetNewsList([FromQuery] PagingInfo pagingInfo)
    {
        var query = new GetNewsQuery(pagingInfo);
        var result = await Sender.Send(query);
        if (result.IsFailed)
            return Problem(result.ToResult());

        Response.AddPaginationHeaders(result.Value.Meta);
        return Ok(result.Value.Adapt<List<NewsListItemDto>>());
    }

    [HttpGet("News/{id:guid}")]
    public async Task<ActionResult<News>> GetNews(Guid id)
    {
        var query = new GetNewsByIdQuery(id);
        var result = await Sender.Send(query);
        return result.Match(
            s => Ok(s),
            f => Problem(f));
    }

    [HttpGet("Contents")]
    public async Task<ActionResult<List<WebContent>>> GetContentsList()
    {
        var query = new GetWebContentsQuery();
        var result = await Sender.Send(query);
        return result.Match(
            s => Ok(s),
            f => Problem(f));
    }

    [HttpGet("Contents/{title}")]
    public async Task<ActionResult<WebContent>> GetContentByTitle(string title)
    {
        var query = new GetWebContentByTitleQuery(title);
        var result = await Sender.Send(query);
        return result.Match(
            s => Ok(s),
            f => Problem(f));
    }

    [HttpGet("Options")]
    public ActionResult<OptionsGetDto> GetOptions()
    {
        return _storageOptions.Adapt<OptionsGetDto>();
    }
}

public record OptionsGetDto(string AllowedExtensions, int MaxFileCount, long MaxFileSize, int MaxTextLength);
