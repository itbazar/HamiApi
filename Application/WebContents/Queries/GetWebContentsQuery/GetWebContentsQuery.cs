using Domain.Models.WebContents;
using MediatR;

namespace Application.WebContents.Queries.GetWebContentsQuery;

public sealed record GetWebContentsQuery() : IRequest<List<WebContent>>;
