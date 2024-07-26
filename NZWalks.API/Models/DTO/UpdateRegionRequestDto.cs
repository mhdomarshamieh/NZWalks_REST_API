using System.ComponentModel.DataAnnotations;

namespace NZWalks.API.Models.DTO
{
    public class UpdateRegionRequestDto
    {
        [Required]
        [MaxLength(100, ErrorMessage = "Name has to be maximum 100 characters")]
        public string Name { get; set; }
        [Required]
        [MinLength(3, ErrorMessage = "Code has to be minimum 3 characters")]
        [MaxLength(3, ErrorMessage = "Code has to be maximum 3 characters")]
        public string Code { get; set; }
        public string? RegionImageUrl { get; set; }
    }
}
