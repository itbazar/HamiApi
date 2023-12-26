﻿using Application.Common.Interfaces.Security;

namespace Api.Contracts.Complaint;

public record ComplaintCreateDto(
        string Title,
        string Text,
        Guid CategoryId,
        List<IFormFile>? Medias,
        CaptchaValidateModel Captcha);