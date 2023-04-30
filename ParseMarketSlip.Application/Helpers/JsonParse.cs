using ParseMarketSlip.Application.Models;

namespace ParseMarketSlip.Application.Helpers;

public class ParseJson
{
    private List<ResultModel> result = new();

    public void ParseMarketSlip(List<SlipModel> models)
    {
        List<Vertice> lastVertices = new();
        int lastIndex = 0;
        result.Add(new ResultModel { Text = models[1].Description });
        lastVertices = models[1].BoundingPoly.Vertices;

        foreach (var model in models.Skip(2))
        {
            if (IsNextLine(lastVertices, model.BoundingPoly.Vertices) is false)
            {
                result.Add(new ResultModel
                {
                    FirstXCoordinat = model.BoundingPoly.Vertices.First().XCoordinat,
                    YCoordinat = model.BoundingPoly.Vertices.First().YCoordinat,
                    Text = model.Description
                });
                lastIndex += 1;
                lastVertices = model.BoundingPoly.Vertices;
            }
            else if (IsNextLine(lastVertices, model.BoundingPoly.Vertices) is null)
                result = FindLocation(models, result, model);

            else
            {
                result[lastIndex].LastXCoordinat = lastVertices.First().XCoordinat;
                result[lastIndex].YCoordinat = lastVertices.First().YCoordinat;
                result[lastIndex].Text = string.Concat(result[lastIndex].Text, $" {model.Description}");
                lastVertices = model.BoundingPoly.Vertices;
            }
        }
        ExportText(result);
    }
    ///<summary>
    ///It allows us to understand whether the description should be added next to that line or on the next or previous lines.
    ///</summary>
    private bool? IsNextLine(List<Vertice> last, List<Vertice> current)
    {
        int bigYCoordinatCount = 0;
        for (int i = 0; i < 4; i++)
        {
            if (last[i].XCoordinat >= current[i].XCoordinat && last[i].YCoordinat <= current[i].YCoordinat)
                return false;

            else if (last[i].XCoordinat < current[i].XCoordinat)
            {
                if (last[i].YCoordinat <= current[i].YCoordinat)
                    return true;

                else if (bigYCoordinatCount <= 4)
                {
                    bigYCoordinatCount += 1;

                    // to understand if the previous line is also
                    if (bigYCoordinatCount is 4)
                        return null;
                }
            }
        }
        return true;
    }

    ///<summary>
    /// Text positions it in the right place.
    /// To know which line the comment should be on, it must be greater than the last x coordinate, 
    ///greater than the first x coordinate of the next line, and less than the y coordinate.
    ///</summary>
    private List<ResultModel> FindLocation(List<SlipModel> models, List<ResultModel> result, SlipModel currentModel)
    {
        List<Vertice> lastVertices = new();
        lastVertices = models[1].BoundingPoly.Vertices;
        string lastText = string.Empty;
        foreach (var model in models.Skip(2))
        {
            if (model.BoundingPoly.Vertices.First().YCoordinat > currentModel.BoundingPoly.Vertices.First().YCoordinat)
            {
                for (int i = 0; i < result.Count; i++)
                {
                    if (result[i].LastXCoordinat < currentModel.BoundingPoly.Vertices.First().XCoordinat
                        && result[i + 1].YCoordinat > currentModel.BoundingPoly.Vertices.First().YCoordinat
                        && result[i + 1].FirstXCoordinat < currentModel.BoundingPoly.Vertices.First().XCoordinat)
                    {
                        result[i].Text = string.Concat(result[i].Text, $" {currentModel.Description}");
                        result[i].LastXCoordinat = currentModel.BoundingPoly.Vertices.First().XCoordinat;
                        result[i].YCoordinat = currentModel.BoundingPoly.Vertices.First().YCoordinat;
                        return result;
                    }
                }
            }

            lastVertices = model.BoundingPoly.Vertices;
            lastText = model.Description;
        }
        return result;
    }

    private void ExportText(List<ResultModel> result)
    {
        using (TextWriter tw = new StreamWriter("result.txt"))
        {
            foreach (var item in result)
                tw.WriteLine(item.Text);
        }
    }
}