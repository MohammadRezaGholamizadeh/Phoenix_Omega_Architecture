using System.ComponentModel.DataAnnotations;

namespace IdentityLayer.AspDotNetIdentity.Services.Contracts.Dtos
{
    public class UpdateApplicationUserDto
    {
        [Required]
        [MaxLength(30)]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(30)]
        public string LastName { get; set; }
        [Required]
        public string MobileNumber { get; set; }
        [Required]
        public string CountryCallingCode { get; set; }
        [Required]
        [MaxLength(10)]
        public string NationalCode { get; set; }
        [Required]
        public Guid AvatarId { get; set; }
        [Required]
        [MaxLength(10)]
        public string AvatarExtension { get; set; }
    }
}
