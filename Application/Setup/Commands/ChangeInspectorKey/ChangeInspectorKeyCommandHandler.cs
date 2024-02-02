using Application.Common.Interfaces.Maintenance;
using Application.Common.Interfaces.Persistence;

namespace Application.Setup.Commands.ChangeInspectorKey;

public sealed class ChangeInspectorKeyCommandHandler(
    IComplaintRepository complaintRepository,
    IMaintenanceService maintenanceService,
    IPublicKeyRepository publicKeyRepository) 
    : IRequestHandler<ChangeInspectorKeyCommand, Result<ChangeInspectorKeyResponse>>
{
    public async Task<Result<ChangeInspectorKeyResponse>> Handle(
        ChangeInspectorKeyCommand request,
        CancellationToken cancellationToken)
    {
        if(request.IsPolling)
        {
            if(await maintenanceService.IsMaintenanceEnabledAsync())
            {
                return new ChangeInspectorKeyResponse(
                    await maintenanceService.GetTotalAsync(),
                    await maintenanceService.GetDoneAsync(),
                    await maintenanceService.GetFailedAsync());
            }
            else
            {
                return new ChangeInspectorKeyResponse(
                    await maintenanceService.GetTotalAsync(),
                    await maintenanceService.GetDoneAsync(),
                    await maintenanceService.GetFailedAsync());
            }

        }
        else
        {
            if (await maintenanceService.IsMaintenanceEnabledAsync())
            {
                return PublicKeyErrors.UnderOperation;
            }
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

        var totalResult = await complaintRepository.CountAsync(c => c.PublicKeyId == fromKeyResult.Value.Id);
        if (totalResult.IsFailed)
        {
            return totalResult.ToResult();
        }


        var total = totalResult.Value;
        var p = new ChangeInspectorKeyParameters(
            fromKeyResult.Value.Id,
            fromPrivateKey,
            toKeyResult.Value.Key,
            toKeyResult.Value.Id,
            total);
        await maintenanceService.SetParametersAsync(p);
        await maintenanceService.SetTotalAsync(total);
        await maintenanceService.EnableMaitenanceModeAsync();

        return new ChangeInspectorKeyResponse(total, 0, 0);
    }
}
