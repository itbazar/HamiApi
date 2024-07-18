using Application.Common.Interfaces.Persistence;

namespace Application.Setup.Commands.EditChartNames;

public sealed class EditChartNamesCommandHandler(
    IChartRepository chartRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<EditChartNamesCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(EditChartNamesCommand request, CancellationToken cancellationToken)
    {
        var charts = await chartRepository.GetAsync();
        foreach (var chart in charts)
        {
            var title = chart.Title;
            title.Replace("درخواست", "گزارش");
            chart.Title = title;
            chartRepository.Update(chart);
        }
        await unitOfWork.SaveAsync();
        return true;
    }
}
