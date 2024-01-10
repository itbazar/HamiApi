using Domain.Models.WebContents;
using MediatR;

namespace Application.WebContents.Queries.GetWebContentByIdQuery;

public sealed record GetWebContentByIdQuery(Guid Id) : IRequest<WebContent>;
