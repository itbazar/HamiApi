using Domain.Models.WebContents;
using MediatR;

namespace Application.WebContents.Commands.EditWebContent;

public sealed record EditWebContentCommand(
    Guid Id,
    string? Title,
    string? Description,
    string? Content) : IRequest<WebContent>;

