using Application.Common.Interfaces.Maintenance;
using Application.Common.Interfaces.Persistence;
using Domain.Models.ComplaintAggregate;

namespace Application.Setup.Commands.ChangeInspectorKey;

public sealed class ChangeInspectorKeyCommandHandler(
    IComplaintRepository complaintRepository,
    IMaintenanceService maintenanceService,
    IPublicKeyRepository publicKeyRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<ChangeInspectorKeyCommand, Result<ChangeInspectorKeyResponse>>
{
    public async Task<Result<ChangeInspectorKeyResponse>> Handle(
        ChangeInspectorKeyCommand request,
        CancellationToken cancellationToken)
    {
        if(request.IsPolling || await maintenanceService.IsMaintenanceEnabledAsync())
        {
            return new ChangeInspectorKeyResponse(
                await maintenanceService.GetTotalAsync(), 
                await maintenanceService.GetDoneAsync());
        }

        var fromKeyResult = await publicKeyRepository.GetActive();
        if (fromKeyResult.IsFailed)
            return fromKeyResult.ToResult();

        var toKeyResult = await publicKeyRepository.Get(request.ToKeyId);
        if(toKeyResult.IsFailed)
            return toKeyResult.ToResult();

        if (fromKeyResult.Value.Id == toKeyResult.Value.Id)
            return PublicKeyErrors.ThisKeyIsActiveAlready;
        var fromPrivateKey = request.PrivateKey;
        var toPublicKey = toKeyResult.Value.Key;

        long total = 0;
        try
        {
            await maintenanceService.EnableMaitenanceModeAsync();

            var totalResult = await complaintRepository.CountAsync();
            if (totalResult.IsFailed)
            {
                throw new Exception();
            }
            total = totalResult.Value;
            await maintenanceService.SetTotalAsync(total);

            List<Complaint> complaints;
            int skip = 0, take = 5;
            do
            {
                var result = await complaintRepository.GetAsync(
                    c => c.PublicKeyId == fromKeyResult.Value.Id, skip, take, true);
                if (result.IsFailed)
                {
                    throw new Exception();
                }
                complaints = result.Value;

                foreach (var complaint in complaints)
                {
                    complaint.ChangeInspectorKey(fromPrivateKey, toPublicKey);
                }
                await unitOfWork.SaveAsync();

                await maintenanceService.AddDoneAsync(complaints.Count);

            } while (complaints.Any());

            await maintenanceService.DisableMaitenanceModeAsync();
        }
        catch
        {
            await maintenanceService.DisableMaitenanceModeAsync();
            return PublicKeyErrors.ChangeKeyProblem;
        }
        return new ChangeInspectorKeyResponse(total, total);
    }
}
