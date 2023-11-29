namespace IdentityLayer.AspDotNetIdentity.Services.Contracts.Dtos
{
    public class GetApplicationUserDto
    {
        public string Id { get; set; }
        public string MobileNumber { get; set; }
        public string UserName { get; set; }
        public string CountryCallingCode { get; set; }
    }
}
