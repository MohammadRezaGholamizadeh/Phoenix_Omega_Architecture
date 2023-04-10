using ApplicationLayer.ServiceInterface;
using DomainLayer.Entities.Color;

namespace ApplicationLayer.AppliactionServices.ColorsAppService.Contracts
{
    public interface IColorService : IService
    {
        void Add(Color color);
    }
}
