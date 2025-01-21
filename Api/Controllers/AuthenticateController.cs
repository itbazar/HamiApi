using Api.Abstractions;
using Api.Contracts.Authenticate;
using Api.Contracts.Patient;
using Api.ExtensionMethods;
using Application.Authentication.Commands.ChangePasswordCommand;
using Application.Authentication.Commands.ChangePhoneNumberCommand;
using Application.Authentication.Commands.LoginCommand;
using Application.Authentication.Commands.LogisterCitizenCommand;
using Application.Authentication.Commands.RefreshCommand;
using Application.Authentication.Commands.RevokeCommand;
using Application.Authentication.Queries.ChangePhoneNumberQuery;
using Application.Common.Interfaces.Security;
using Application.Users.Commands.RegisterPatient;
using Application.Users.Commands.UpdateUserProfile;
using Application.Users.Queries.GetUserProfile;
using Domain.Models.ComplaintAggregate;
using Domain.Models.Hami;
using Domain.Models.IdentityAggregate;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
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

    [HttpPost("RegisterPatient")]
    public async Task<IActionResult> RegisterPatient([FromBody] RegisterPatientDto dto)
    {
        if (!ModelState.IsValid)
        {
            // لاگ خطاها
            var errors = ModelState.Values.SelectMany(v => v.Errors)
                                           .Select(e => e.ErrorMessage)
                                           .ToList();
            return BadRequest(new { Errors = errors });
        }

        var command = new RegisterPatientCommand(

            dto.Username,
    dto.Password,
    dto.PhoneNumber,
    dto.NationalId,
    dto.FirstName,
    dto.LastName,
    dto.Title,
    dto.DateOfBirth,
    dto.Gender,
    dto.Education,
    dto.City,
    dto.Organ,
    dto.DiseaseType,
    dto.PatientStatus,
    dto.Stage,
    dto.PathologyDiagnosis,
    dto.InitialWeight,
    dto.SleepDuration,
    dto.AppetiteLevel,
    dto.GADScore,
   dto.MDDScore);

        var result = await Sender.Send(command);

        return result.Match(
            s => Ok(s),
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
    public async Task<ActionResult<GetProfileDto>> GetUserProfile([FromQuery] string? Mode = null)
    {
        var userId = User.GetUserId();
        if (userId == null)
            return Unauthorized();

        var query = new GetUserProfileQuery(userId,Mode);
        var result = await Sender.Send(query);


        if (Mode == "Patient")
        {
            return result.Match(
           user =>
           {
               // استخراج گروه بیمار و منتور
               var userGroup = user.UserGroupMemberships.FirstOrDefault();
               var patientGroup = userGroup?.PatientGroup;
               var mentor = patientGroup?.Mentor;

               // مقداردهی MentorName و PatientGroupName
               var dto = new GetProfileDto(
                   UserName: user.UserName,
                   FirstName: user.FirstName,
                   LastName: user.LastName,
                   NationalId: user.NationalId,
                   Title: user.Title,
                   PhoneNumber: user.PhoneNumber,
                   MentorName: mentor != null ? $"{mentor.FirstName} {mentor.LastName}" : "نامشخص",
                   PatientGroupName: patientGroup != null ? patientGroup.Description : "نامشخص"
               );

               return Ok(dto);
           },
           f => Problem(f));
        }
        else
        {
            return result.Match(
                s => Ok(s.Adapt<GetProfileDto>()),
                f => Problem(f));
        }
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

    [Authorize]
    [HttpPost("PhoneNumber")]
    public async Task<ActionResult> ChangePhoneNumberRequest(ChangePhoneNumberRequestDto phoneDto)
    {
        var username = User.GetUserName();
        if (username == null)
            return Unauthorized();

        var command = new ChangePhoneNumberQuery(
            username, phoneDto.NewPhoneNumber, phoneDto.Captcha);

        var result = await Sender.Send(command);
        return result.Match(
            s => Ok(s),
            f => Problem(f));
    }

    [Authorize]
    [HttpPut("PhoneNumber")]
    public async Task<ActionResult> ChangePhoneNumber(ChangePhoneNumberDto phoneDto)
    {
        var username = User.GetUserName();
        if (username == null)
            return Unauthorized();

        var command = new ChangePhoneNumberCommand(
            username,
            phoneDto.Token1,
            phoneDto.Code1,
            phoneDto.Token2,
            phoneDto.Code2);

        var result = await Sender.Send(command);
        return result.Match(
            s => NoContent(),
            f => Problem(f));
    }
}
