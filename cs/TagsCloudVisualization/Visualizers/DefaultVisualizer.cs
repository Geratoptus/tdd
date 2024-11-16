using System.Drawing;

namespace TagsCloudVisualization.Visualizers;

public class DefaultVisualizer(Size bitmapSize) : IVisualizer
{
    public Bitmap CreateBitmap(IEnumerable<Rectangle> rectangles)
    {
        var bitmap = new Bitmap(bitmapSize.Width, bitmapSize.Height);
        
        using var graphics = Graphics.FromImage(bitmap);
        foreach (var rectangle in rectangles)
        {
            var pen = new Pen(Color.DeepPink);
            graphics.DrawRectangle(pen, rectangle);
        }

        return bitmap;
    }
}