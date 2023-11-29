using System.ComponentModel.DataAnnotations;

namespace IdentityLayer.AspDotNetIdentity.Services.Contracts.Dtos
{
    public class ApplicationUserIdDto
    {
        [Required]
        public Guid UserId { get; set; }
    }
}
