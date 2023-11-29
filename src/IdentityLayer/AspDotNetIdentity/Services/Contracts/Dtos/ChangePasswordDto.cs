using System.ComponentModel.DataAnnotations;

namespace IdentityLayer.AspDotNetIdentity.Services.Contracts.Dtos
{
    public class ChangePasswordDto
    {
        [Required]
        public string OldPassword { get; set; }
        [Required]
        public string NewPassword { get; set; }
    }
}
