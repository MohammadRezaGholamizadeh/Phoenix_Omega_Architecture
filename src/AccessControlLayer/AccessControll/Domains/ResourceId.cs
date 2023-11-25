using System.Runtime.InteropServices;

namespace AccessControlLayer.AccessControll.Domains
{
    [Guid("292c0719-35dd-4b37-92b4-1cdb8492ecb4")]
    public class ResourceId
    {
        public string Id { get; set; }
        public string UserResourceId { get; set; }
        public UserResource UserResource { get; set; }
        public string TargetResourceId { get; set; }
    }
}
