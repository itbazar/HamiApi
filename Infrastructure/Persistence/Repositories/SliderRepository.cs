using Application.Common.Interfaces.Persistence;
using Domain.Models.Sliders;

namespace Infrastructure.Persistence.Repositories;

public class SliderRepository : GenericRepository<Slider>, ISliderRepository
{
    public SliderRepository(ApplicationDbContext context) : base(context)
    {
    }
}
