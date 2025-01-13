using Application.Common.Interfaces.Persistence;
using Domain.Models.Hami;

namespace Application.Questions.Queries.GetMentorPatientGroupsQuery;

internal class GetMentorPatientGroupsQueryHandler(IPatientGroupRepository patientGroupRepository) : IRequestHandler<GetMentorPatientGroupsQuery, Result<List<PatientGroup>>>
{
    public async Task<Result<List<PatientGroup>>> Handle(GetMentorPatientGroupsQuery request, CancellationToken cancellationToken)
    {
        var groups = await patientGroupRepository
            .GetAsync(q => q.MentorId == request.MentorId && !q.IsDeleted,
            includeProperties: "Mentor,Members");
        return groups.ToList();
    }
}