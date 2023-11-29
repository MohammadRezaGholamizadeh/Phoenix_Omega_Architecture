using System.ComponentModel.DataAnnotations;

namespace IdentityLayer.AspDotNetIdentity.Services.Contracts.Dtos
{
    public class UpdateUserAvatarDto
    {
        [Required]
        public Guid AvatarId { get; set; }
        [Required]
        [MaxLength(10)]
        public string AvatarExtension { get; set; }
    }
}
