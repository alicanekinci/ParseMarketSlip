using System.Text.Json.Serialization;

namespace ParseMarketSlip.Application.Models;

public class SlipModel
{
    [JsonPropertyName("locale")]
    public string Locale { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("boundingPoly")]
    public BoundingPoly BoundingPoly { get; set; }
}