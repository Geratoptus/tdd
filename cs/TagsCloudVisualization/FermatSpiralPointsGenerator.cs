using System.Drawing;

namespace TagsCloudVisualization;

public class FermatSpiralPointsGenerator : IPointsGenerator
{
    public FermatSpiralPointsGenerator(double radius, double angleOffset)
    {
        if (radius <= 0)
            throw new ArgumentException("radius must be greater than 0", nameof(radius));
        if (angleOffset <= 0)
            throw new ArgumentException("angleOffset must be greater than 0", nameof(angleOffset));
        
    }

    public IEnumerable<Point> GeneratePoints(Point spiralCenter)
    {
        yield return spiralCenter;
    }
}