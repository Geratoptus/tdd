using System.Drawing;
using TagsCloudVisualization.Extensions;
using TagsCloudVisualization.PointsGenerators;

namespace TagsCloudVisualization.CloudLayouters;

public class CircularCloudLayouter(Point layoutCenter, IPointsGenerator pointsGenerator) : ICircularCloudLayouter
{
    public Point LayoutCenter { get; } = layoutCenter;

    private readonly IEnumerator<Point> pointEnumerator = pointsGenerator
        .GeneratePoints(layoutCenter)
        .GetEnumerator();
    private readonly List<Rectangle> layoutRectangles = [];

    public CircularCloudLayouter(Point layoutCenter, double radius, double angleOffset) :
        this(layoutCenter, new FermatSpiralPointsGenerator(radius, angleOffset))
    {
        
    }

    public Rectangle PutNextRectangle(Size rectangleSize)
    {
        var rectangles = pointEnumerator
            .ToIEnumerable()
            .Select(point => CreateRectangleWithCenter(point, rectangleSize));
        var rectangle = new Rectangle();
        try
        {
            rectangle = rectangles
                .First(rectangle => !layoutRectangles.Any(rectangle.IntersectsWith));
        }
        catch (InvalidOperationException e)
        {
            throw new InvalidOperationException("Был передан конечный генератор точек");
        }

        layoutRectangles.Add(rectangle);
        return rectangle;
    }

    public Rectangle CreateRectangleWithCenter(Point center, Size rectangleSize)
    {
        var x = center.X - rectangleSize.Width / 2;
        var y = center.Y - rectangleSize.Height / 2;
        return new Rectangle(x, y, rectangleSize.Width, rectangleSize.Height);
    }

}