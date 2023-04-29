using System.Text.Json;
using ParseMarketSlip.Application.Models;

List<string> result = new();

using (StreamReader r = new StreamReader("response.json"))
{
    string json = r.ReadToEnd();
    var model = JsonSerializer.Deserialize<List<SlipModel>>(json);
}

