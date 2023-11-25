using AccessControlLayer.AccessControll.Contracts;
using AccessControlLayer.AccessControll.Contracts.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;

namespace AccessControlLayer.Infrastructure.MiddleWares
{
    public class AccessControlMiddleWare : IFilterMetadata
    {
        private readonly IHttpContextAccessor _accessor;
        private readonly AccessControlService _accessControl;

        public AccessControlMiddleWare(
            IHttpContextAccessor accessor,
            AccessControlService accessControl)
        {
            _accessor = accessor;
            _accessControl = accessControl;
            Invoke().Wait();
        }

        public async Task Invoke()
        {
            var actionTypesId =
                _accessor.HttpContext?.Request.Headers
                         .Where(_ => _.Key == "itemaccessrequest")
                         .SelectMany(_ => _.Value.ToString().Split(","))
                         .ToList();

            if (actionTypesId!.Any())
            {
                var dto = new AllActionTypeIdDto()
                {
                    ActionTypesId = actionTypesId!
                };
                var accessResults =
                    await _accessControl
                          .GetAllUserAccessResultByActionTypeId(dto);

                var accessJsonResult =
                    accessResults.Select(_ => JsonConvert.SerializeObject(_))
                    .ToList();

                _accessor
                    .HttpContext?
                    .Response
                    .Headers
                    .AppendList("itemaccessresponse", accessJsonResult);
            }
        }
    }
}
