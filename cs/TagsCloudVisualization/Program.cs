using System.Drawing;
using System.Drawing.Imaging;
using TagsCloudVisualization.CloudLayouters;
using TagsCloudVisualization.Visualizers;

namespace TagsCloudVisualization;

public static class Program
{
    private const int ImageWidth = 1920;
    private const int ImageHeight = 1080;
    
    private const int RectanglesNumber = 1000;
    private const int MinRectangleSize = 10;
    private const int MaxRectangleSize = 50;
    
    private const double LayoutRadius = 1;
    private const double LayoutAngleOffset = 10;

    private const string ImagesDirectory = "images";
    
    public static void Main()
    {
        var center = new Point(ImageWidth / 2, ImageHeight / 2);
        var cloudLayouter = new CircularCloudLayouter(center, LayoutRadius, LayoutAngleOffset);
        var random = new Random();
        var rectangles = new Rectangle[RectanglesNumber];

        rectangles = rectangles
            .Select(_ => cloudLayouter.PutNextRectangle(new Size(
                random.Next(MinRectangleSize, MaxRectangleSize),
                random.Next(MinRectangleSize, MaxRectangleSize))))
            .ToArray();
        
        var visualizer = new DefaultVisualizer();
        var bitmap = visualizer.CreateBitmap(rectangles, new Size(ImageWidth, ImageHeight));
        Directory.CreateDirectory(ImagesDirectory);
        
        bitmap.Save(GetPathToImages(), ImageFormat.Jpeg);
    }

    private static string GetPathToImages()
    {
        var filename = $"{RectanglesNumber}_{LayoutRadius}_{LayoutAngleOffset}_TagCloud.jpg";
        return Path.Combine(ImagesDirectory, filename);
    }
}