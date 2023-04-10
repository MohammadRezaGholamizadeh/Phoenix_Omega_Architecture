using ApplicationLayer.AppliactionServices.ColorsAppService.Contracts;
using DomainLayer.Entities.Color;
using ServiceLayer.Services.ColorService.Contracts;

namespace ServiceLayer.Services.ColorService
{
    public class ColorService : IColorService
    {
        private readonly IColorRepository _colorRepository;

        public ColorService(IColorRepository colorRepository)
        {
            _colorRepository = colorRepository;
        }

        public void Add(Color color)
        {
            _colorRepository.Add(color);
        }
    }
}
