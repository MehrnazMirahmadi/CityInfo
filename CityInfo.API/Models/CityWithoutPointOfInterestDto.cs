namespace CityInfo.API.Models
{
    public class CityWithoutPointOfInterestDto
    {
            public int Id { get; set; }
            public required string Name { get; set; } = string.Empty;
            public string? Description { get; set; }

    }
}
