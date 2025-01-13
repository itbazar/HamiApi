using Application.Common.Interfaces.Persistence;
using Domain.Models.Hami;

namespace Application.Questions.Queries.GetMentorPatientGroupsQuery;

public record GetMentorPatientGroupsQuery(string MentorId) : IRequest<Result<List<PatientGroup>>>;
