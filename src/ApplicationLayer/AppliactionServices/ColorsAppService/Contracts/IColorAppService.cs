using ApplicationLayer.AppliactionServices.ColorsAppService.Contracts.Dtos;
using ApplicationLayer.ApplicationServiceInterface;

namespace ApplicationLayer.AppliactionServices.ColorsAppService.Contracts
{
    public interface IColorAppService : IApplicationService
    {
        Task<int> Add(AddColorDto addColorDto);
    }
}
