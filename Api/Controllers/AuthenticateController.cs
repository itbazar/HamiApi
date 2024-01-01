using Api.Abstractions;
using Api.Contracts.Authenticate;
using Api.ExtensionMethods;
using Application.Authentication.Commands.ChangePasswordCommand;
using Application.Authentication.Commands.LoginCommand;
using Application.Authentication.Commands.LogisterCitizenCommand;
using Application.Users.Commands.UpdateUserProfile;
using Application.Users.Queries.GetUserProfile;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Api.Controllers;

public class AuthenticateController : ApiController
{
    public AuthenticateController(ISender sender) : base(sender)
    {
    }


    [HttpPost("LoginStaff")]
    public async Task<ActionResult> LoginStaff(LoginStaffDto loginDto)
    {
        var command = new LoginCommand(loginDto.Username, loginDto.Password, loginDto.Captcha);
        await Sender.Send(command);
        return StatusCode(StatusCodes.Status428PreconditionRequired, "");
    }

    [HttpPost("VerifyStaff")]
    public async Task<ActionResult> Verify([FromBody] StaffVerificationDto verificationDto)
    {
        var command = new LoginCommand(verificationDto.Username, verificationDto.Password, null, verificationDto.VerificationCode);
        var result = await Sender.Send(command);
        return Ok(result.JwtToken);
    }

    [HttpPost("LogisterCitizen")]
    public async Task<ActionResult> LogisterCitizen([FromBody] LogisterCitizenDto logisterDto)
    {
        var command = new LogisterCitizenCommand(logisterDto.PhoneNumber, null, logisterDto.Captcha);
        await Sender.Send(command);
        return StatusCode(StatusCodes.Status428PreconditionRequired, "");
    }

    [HttpPost("VerifyCitizen")]
    public async Task<ActionResult> VerifyCitizen([FromBody] CitizenVerificationDto logisterDto)
    {
        var command = new LogisterCitizenCommand(logisterDto.PhoneNumber, logisterDto.VerificationCode, null);
        var result = await Sender.Send(command);
        return Ok(result.JwtToken);
    }

    [Authorize]
    [HttpPut("ChangePassword")]
    public async Task<ActionResult> ChangePassword(ChangePasswordAppDto changePasswordDto)
    {
        var userName = User.FindFirstValue(ClaimTypes.Name);
        if (userName is null)
        {
            return Unauthorized();
        }
        var command = new ChangePasswordCommand(userName, changePasswordDto.OldPassword, changePasswordDto.NewPassword);
        var result = await Sender.Send(command);
        if (result)
        {
            return NoContent();
        }
        else
        {
            return Problem();
        }
    }


    [Authorize]
    [HttpGet("Profile")]
    public async Task<ActionResult<GetProfileDto>> GetUserProfile()
    {
        var userId = User.GetUserId();
        if (userId == null)
        {
            return Unauthorized();
        }
        var query = new GetUserProfileQuery(userId);
        var result = await Sender.Send(query);
        var mappedUser = result.Adapt<GetProfileDto>();
        return Ok(mappedUser);
    }

    [Authorize]
    [HttpPut("Profile")]
    public async Task<ActionResult> UpdateProfile(UpdateProfileDto updateDto)
    {
        var userId = User.GetUserId();
        if (userId == null)
            return Unauthorized();

        var command = new UpdateUserProfileCommand(
            userId,
            updateDto.FirstName,
            updateDto.LastName,
            updateDto.Title,
            updateDto.NationalId,
            updateDto.PhoneNumber2);

        var result = await Sender.Send(command);
        if (result == null)
            return Problem();

        return NoContent();
    }
}


