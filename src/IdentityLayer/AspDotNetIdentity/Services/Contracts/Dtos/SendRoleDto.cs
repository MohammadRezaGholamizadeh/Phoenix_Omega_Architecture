using System.ComponentModel.DataAnnotations;

namespace IdentityLayer.AspDotNetIdentity.Services.Contracts.Dtos
{
    public class SendRoleDto
    {
        [Required]
        public string Role { get; set; }
    }
}
