using Api.Abstractions;
using Api.Contracts.Storage;
using Api.ExtensionMethods;
using Application.Uploads.Commands.CreateUpload;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FilesController : ApiController
{
    
    public FilesController(ISender sender): base(sender)
    {
    }

    //[HttpGet("{id}")]
    //public async Task<ActionResult<MediaDto>> GetFile(Guid id)
    //{
    //    //var command = new GetMediaQuery(id);
    //    //var media = await Sender.Send(command);
    //    //var mappedMedia = media.Adapt<MediaDto>();
    //    //return Ok(mappedMedia);
    //    await Task.CompletedTask;
    //    throw new NotImplementedException();
    //}

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> PostFile([FromForm] UploadDto upload)
    {
        var userId = User.GetUserId();
        if (userId is null)
            return Unauthorized();
        var command = new AddUploadCommand(userId, upload.File, upload.AttachmentType);
        var result = await Sender.Send(command);
        return result.Match(
            s => StatusCode(StatusCodes.Status201Created, new {id = s.Id}),
            f => Problem(f));
    }
}


