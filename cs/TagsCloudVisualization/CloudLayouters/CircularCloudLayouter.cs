using System.Drawing;

namespace TagsCloudVisualization;

public class CircularCloudLayouter : ICircularCloudLayouter
{
    private readonly Point layoutCenter;
    private readonly  IEnumerator<Point> pointEnumerator;
    private List<Rectangle> layoutRectangles = new List<Rectangle>();
    
    public CircularCloudLayouter(Point layoutCenter, double radius, double angleOffset)
    {
        this.layoutCenter = layoutCenter;
        pointEnumerator = new FermatSpiralPointsGenerator(radius, angleOffset)
            .GeneratePoints(layoutCenter)
            .GetEnumerator();
    }

    public Rectangle PutNextRectangle(Size rectangleSize)
    {
        Rectangle rectangle;
        do
        {
            pointEnumerator.MoveNext();
            var rectanglePos = pointEnumerator.Current;
            rectangle = CreateRectangleWithCenter(rectanglePos, rectangleSize);
            
        } while (layoutRectangles.Any(rectangle.IntersectsWith));
        
        layoutRectangles.Add(rectangle);

        return rectangle;
    }

    private static Rectangle CreateRectangleWithCenter(Point center, Size rectangleSize)
    {
        var x = center.X - rectangleSize.Width / 2;
        var y = center.Y - rectangleSize.Height / 2;
        return new Rectangle(x, y, rectangleSize.Width, rectangleSize.Height);
    }
    
    
}