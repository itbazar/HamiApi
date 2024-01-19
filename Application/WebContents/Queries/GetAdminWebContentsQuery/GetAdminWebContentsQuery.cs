using Domain.Models.WebContents;
using MediatR;

namespace Application.WebContents.Queries.GetAdminWebContentsQuery;

public sealed record GetAdminWebContentsQuery() : IRequest<Result<List<WebContent>>>;
