using System.Drawing;

namespace TagsCloudVisualization;

public interface IPointsGenerator
{
    public IEnumerable<Point> GeneratePoints(Point startPoint);
}