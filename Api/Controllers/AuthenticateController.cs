using Api.Abstractions;
using Api.Contracts.Authenticate;
using Api.ExtensionMethods;
using Application.Authentication.Commands.ChangePasswordCommand;
using Application.Authentication.Commands.LoginCommand;
using Application.Authentication.Commands.VerifyPhoneNumberCommand;
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
        var result = await Sender.Send(command);
        if (result.UserNotConfirmed)
        {
            return StatusCode(StatusCodes.Status428PreconditionRequired, "");
        }
        else
        {
            return Ok(result.JwtToken);
        }
    }

    [HttpPost("Verify")]
    public async Task<ActionResult> Verify([FromBody] VerificationDto verificationDto)
    {
        var command = new VerifyPhoneNumberCommand(verificationDto.Username, verificationDto.VerificationCode);
        var result = await Sender.Send(command);
        if (result)
        {
            var loginCommand = new LoginCommand(verificationDto.Username, verificationDto.Password);
            var loginResult = await Sender.Send(loginCommand);
            if (loginResult.UserNotConfirmed)
            {
                return Unauthorized();
            }
            else
            {
                return Ok(loginResult.JwtToken);
            }
        }
        else
        {
            return Unauthorized();
        }
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
    public async Task<ActionResult<GetStaffProfileDto>> GetUserProfile()
    {
        var userId = User.GetUserId();
        if (userId == null)
        {
            return Unauthorized();
        }
        var query = new GetUserProfileQuery(userId);
        var result = await Sender.Send(query);
        var mappedUser = result.Adapt<GetStaffProfileDto>();
        return Ok(mappedUser);
    }

    [Authorize]
    [HttpPut("Profile")]
    public async Task<ActionResult> UpdateProfile(UpdateStaffProfileDto updateDto)
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


