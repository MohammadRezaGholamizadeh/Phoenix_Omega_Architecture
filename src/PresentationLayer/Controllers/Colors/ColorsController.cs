using ApplicationLayer.AppliactionServices.ColorsAppService.Contracts;
using ApplicationLayer.AppliactionServices.ColorsAppService.Contracts.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace PresentationLayer.Controllers.Colors
{
    [Route("api/v1/colors")]
    [ApiController]
    public class ColorsController : ControllerBase
    {
        private readonly IColorAppService _colorAppService;

        public ColorsController(IColorAppService colorAppService)
        {
            _colorAppService = colorAppService;
        }

        [HttpPost]
        public async Task<int> Add(AddColorDto addColorDto)
        {
            return await _colorAppService.Add(addColorDto);
        }
    }
}
