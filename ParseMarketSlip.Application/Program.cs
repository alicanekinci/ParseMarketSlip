using System.Text.Json;
using ParseMarketSlip.Application.Helpers;
using ParseMarketSlip.Application.Models;

ParseJson parseJson = new();

using (StreamReader r = new StreamReader("response.json"))
{
    string json = r.ReadToEnd();
    var model = JsonSerializer.Deserialize<List<SlipModel>>(json);

    parseJson.ParseMarketSlip(model);
}