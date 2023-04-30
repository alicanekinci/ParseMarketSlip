using System.Text.Json.Serialization;

namespace ParseMarketSlip.Application.Models;

public class BoundingPoly
{
    [JsonPropertyName("vertices")]
    public List<Vertice> Vertices { get; set; }
}

public class Vertice
{
    [JsonPropertyName("x")]
    public int XCoordinat { get; set; }

    [JsonPropertyName("y")]
    public int YCoordinat { get; set; }
}