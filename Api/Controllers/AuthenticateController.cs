using Api.Abstractions;
using Api.Contracts.Authenticate;
using Api.ExtensionMethods;
using Application.Authentication.Commands.ChangePasswordCommand;
using Application.Authentication.Commands.LoginCommand;
using Application.Authentication.Commands.LogisterCitizenCommand;
using Application.Authentication.Commands.RefreshCommand;
using Application.Authentication.Commands.RevokeCommand;
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
        return result.Match(
            s => Ok(s),
            f => Problem(f));
    }

    [HttpPost("VerifyStaff")]
    public async Task<ActionResult<LoginResultDto>> Verify([FromBody] StaffVerificationDto verificationDto)
    {
        var command = new TwoFactorLoginCommand(
            verificationDto.OtpToken,
            verificationDto.VerificationCode);
        var result = await Sender.Send(command);
        return result.Match(
            s => Ok(s.Adapt<LoginResultDto>()), 
            f => Problem(f));
    }

    [HttpPost("Refresh")]
    public async Task<ActionResult<LoginResultDto>> Refresh([FromBody] RefreshDto refreshDto)
    {
        var command = new RefreshCommand(
            refreshDto.Token,
            refreshDto.RefreshToken);
        var result = await Sender.Send(command);
        return result.Match(
            s => Ok(s.Adapt<LoginResultDto>()), 
            f => Problem(f));
    }

    [Authorize]
    [HttpPost("Revoke")]
    public async Task<ActionResult<bool>> Revoke([FromBody] RevokeDto revokeDto)
    {
        var command = new RevokeCommand(
            User.GetUserId(),
            revokeDto.RefreshToken);
        var result = await Sender.Send(command);
        return result.Match(
            s => Ok(s),
            f => Problem(f));
    }

    [HttpPost("LogisterCitizen")]
    public async Task<ActionResult> LogisterCitizen([FromBody] LogisterCitizenDto logisterDto)
    {
        var command = new LogisterCitizenCommand(logisterDto.PhoneNumber, logisterDto.Captcha);
        var result = await Sender.Send(command);
        return result.Match(
            s => Ok(s),
            f => Problem(f));
    }

    [HttpPost("VerifyCitizen")]
    public async Task<ActionResult<LoginResultDto>> VerifyCitizen([FromBody] CitizenVerificationDto logisterDto)
    {
        var command = new TwoFactorLoginCommand(logisterDto.OtpToken, logisterDto.VerificationCode);
        var result = await Sender.Send(command);
        return result.Match(
            s => Ok(s.Adapt<LoginResultDto>()), 
            f => Problem(f));
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
        return result.Match(
            s => NoContent(), 
            f => Problem(f));
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
        return result.Match(
            s => Ok(s.Adapt<GetProfileDto>()),
            f => Problem(f));
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
        return result.Match(
            s => NoContent(),
            f => Problem(f));
    }
}


