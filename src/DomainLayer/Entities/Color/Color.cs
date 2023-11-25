using DomainLayer.Entities.Organizations;
using System.Runtime.InteropServices;

namespace DomainLayer.Entities.Color
{
    [Guid("09CAD257-2FAF-4A77-9FF5-6FA57B840C8E")]
    public class Color : ITenant
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ColorHex { get; set; }
        public string TenantId { get; set; }
    }
}
