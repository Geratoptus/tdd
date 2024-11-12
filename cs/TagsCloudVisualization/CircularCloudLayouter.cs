using System.Drawing;

namespace TagsCloudVisualization;

public class CircularCloudLayouter : ICircularCloudLayouter
{
    private readonly Point center;
    private  IEnumerator<Point> pointEnumerator;
    private List<Rectangle> rectangles = new List<Rectangle>();
    
    public CircularCloudLayouter(Point center, double radius, double angleOffset)
    {
        this.center = center;
        pointEnumerator = new FermatSpiralPointsGenerator(radius, angleOffset)
            .GeneratePoints(center)
            .GetEnumerator();
    }

    public Rectangle PutNextRectangle(Size rectangleSize)
    {
        return new Rectangle(rectangleSize.Width / 2, rectangleSize.Height / 2, rectangleSize.Width, rectangleSize.Height);
    }
    
    
}