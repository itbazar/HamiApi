﻿using System.ComponentModel.DataAnnotations;

namespace Api.Contracts.Authenticate;

public record RegisterAppDto(
    [Required] [MaxLength(11)] [Phone]
    string Username,
    [Required] [MinLength(6)] [MaxLength(512)]
    string Password);