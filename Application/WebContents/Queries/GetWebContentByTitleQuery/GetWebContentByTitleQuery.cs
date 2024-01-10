using Domain.Models.WebContents;
using MediatR;

namespace Application.WebContents.Queries.GetWebContentByTitleQuery;

public sealed record GetWebContentByTitleQuery(string Title) : IRequest<WebContent>;
