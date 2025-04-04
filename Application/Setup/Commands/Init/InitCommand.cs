﻿using MediatR;

namespace Application.Setup.Commands.Init;

public record InitCommand() : IRequest<Result<string>>;
