using Application.Common.Interfaces.Persistence;
using Domain.Models.Hami;

namespace Application.CounselingSessions.Queries.GetMentorCounselingSessionsQuery;

public record GetMentorCounselingSessionsQuery(string MentorId,Guid? PatientGroupId) : IRequest<Result<List<CounselingSession>>>;
