using Application.Common.Interfaces.Persistence;
using Domain.Models.ComplaintAggregate;
using SharedKernel.ExtensionMethods;

namespace Application.Complaints.Queries.GetPossibleStatesCountQuery;

internal class GetPossibleStatesCountQueryHandler :
    IRequestHandler<GetPossibleStatesCountQuery, Result<List<GetPossibleStatesCountResponse>>>
{
    private readonly IComplaintRepository _complaintRepository;

    public GetPossibleStatesCountQueryHandler(IComplaintRepository complaintRepository)
    {
        _complaintRepository = complaintRepository;
    }

    public async Task<Result<List<GetPossibleStatesCountResponse>>> Handle(
        GetPossibleStatesCountQuery request, CancellationToken cancellationToken)
    {
        var statesHistogram = await _complaintRepository.GetStatesHistogram();
        if (statesHistogram.IsFailed)
            return statesHistogram.ToResult();

        var result = new List<GetPossibleStatesCountResponse>();
        foreach (var item in Enum.GetValues(typeof(ComplaintState)))
        {
            var e = (ComplaintState)item;
            var count = statesHistogram.Value.Where(x => x.State == e).Select(c => c.Count).SingleOrDefault(0);
            result.Add(new GetPossibleStatesCountResponse(
                e.GetDescription() ?? "",
                (int)item,
                count));
        }

        return result;
    }
}
