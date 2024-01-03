using Application.Common.Interfaces.Security;

namespace Api.Contracts.Complaint;

public record ComplaintCreateDto(
        string Title,
        string Text,
        Guid CategoryId,
        List<IFormFile>? Medias,
        CaptchaValidateModel Captcha,
        string Complaining,
        Guid OrganizationId);

public record ComplaintAuthorizedCreateDto(
        string FirstName,
        string LastName,
        string NationalId,
        string PhoneNumber,
        string Title,
        string Text,
        Guid CategoryId,
        List<IFormFile>? Medias,
        CaptchaValidateModel Captcha,
        string? Complaining,
        Guid? OrganizationId);