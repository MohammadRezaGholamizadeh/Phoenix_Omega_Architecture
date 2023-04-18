using ApplicationLayer.AppliactionServices.ColorsAppService.Contracts;
using ApplicationLayer.AppliactionServices.ColorsAppService.Contracts.Dtos;
using Hangfire;
using Microsoft.AspNetCore.Mvc;

namespace PresentationLayer.Controllers.Colors
{
    [Route("api/v{version:apiVersion}/colors")]
    [ApiController]
    [ApiVersion("1.0")]
    public class ColorsController : ControllerBase
    {
        private readonly IColorAppService _colorAppService;
        private readonly ILogger<ColorsController> _logger;
        private readonly IBackgroundJobClient _backgroundJobClient;

        public ColorsController(
            IColorAppService colorAppService,
            ILogger<ColorsController> logger,
            IBackgroundJobClient backgroundJobClient)
        {
            _colorAppService = colorAppService;
            _logger = logger;
            _backgroundJobClient = backgroundJobClient;
        }

        [HttpPost]
        public async Task<int> Add(AddColorDto addColorDto)
        {
            return await _colorAppService.Add(addColorDto);
        }

        [HttpPost("HangFireTest")]
        public async Task<int> TestHangfireJob(AddColorDto addColorDto)
        {
            //_backgroundJobClient.Enqueue(() => _logger.LogInformation("sss"));
            var jobId =
                _backgroundJobClient.Schedule(() => _logger.LogInformation("sss"),TimeSpan.FromMilliseconds(1));
            return await _colorAppService.Add(addColorDto);
        }
    }
}
