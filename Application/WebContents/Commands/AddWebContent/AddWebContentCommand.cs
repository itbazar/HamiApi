using Domain.Models.WebContents;
using MediatR;

namespace Application.WebContents.Commands.AddWebContent;

public sealed record AddWebContentCommand(
    string Title,
    string Description,
    string Content) : IRequest<WebContent>;

