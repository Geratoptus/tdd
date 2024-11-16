using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using TagsCloudVisualization.CloudLayouters;
using TagsCloudVisualization.Extensions;
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
    private const double LayoutAngleOffset = 0.5;

    private const string ImagesDirectory = "images";
    
    public static void Main()
    {
        CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
        
        var center = new Point(ImageWidth / 2, ImageHeight / 2);
        var cloudLayouter = new CircularCloudLayouter(center, LayoutRadius, LayoutAngleOffset);
        var random = new Random();
        var rectangles = Enumerable
            .Range(0, RectanglesNumber)
            .Select(_ => cloudLayouter.PutNextRectangle(random.RandomSize
                (MinRectangleSize, MaxRectangleSize)))
            .ToArray();
        
        var visualizer = new DefaultVisualizer(new Size(ImageWidth, ImageHeight));
        var bitmap = visualizer.CreateBitmap(rectangles);
        Directory.CreateDirectory(ImagesDirectory);
        
        bitmap.Save(GetPathToImages(), ImageFormat.Jpeg);
    }
    
    private static string GetPathToImages()
    {
        var filename = $"{RectanglesNumber}_{LayoutRadius}_{LayoutAngleOffset}_TagCloud.jpg";
        return Path.Combine(ImagesDirectory, filename);
    }
}