using System.ComponentModel.DataAnnotations;

namespace IdentityLayer.AspDotNetIdentity.Services.Contracts.Dtos
{
    public class ToggleActivationDto
    {
        [Required]
        public bool Active { get; set; }
    }
}
