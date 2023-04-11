using ApplicationLayer.AppliactionServices.ColorsAppService.Contracts;
using ApplicationLayer.AppliactionServices.ColorsAppService.Contracts.Dtos;
using ApplicationLayer.InfraInterfaces.UnitOfWorks;
using DomainLayer.Entities.Color;

namespace ApplicationLayer.AppliactionServices.ColorsAppService
{
    public class ColorAppService : IColorAppService
    {
        private readonly IColorService _colorService;
        private readonly UnitOfWork _unitOfWork;

        public ColorAppService(
            IColorService colorService,
            UnitOfWork unitOfWork)
        {
            _colorService = colorService;
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Add(AddColorDto addColorDto)
        {
            var color = new Color()
            {
                Title = addColorDto.Title,
                ColorHex = addColorDto.ColorHex
            };

            _colorService.Add(color);
            await _unitOfWork.SaveAllChangesAsync();

            return color.Id;
        }
    }
}
