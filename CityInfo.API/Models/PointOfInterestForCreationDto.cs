using System.ComponentModel.DataAnnotations;

namespace CityInfo.API.Models
{
    //fluent validation
    public class PointOfInterestForCreationDto
    {
        [Required(ErrorMessage ="Name Ra  Vared Konid")]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(200)]
        public string? Description { get; set; }
    }
}
