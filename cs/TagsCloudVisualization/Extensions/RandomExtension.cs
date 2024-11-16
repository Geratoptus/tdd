using System.Drawing;

namespace TagsCloudVisualization.Extensions;

public static class RandomExtension
{
    public static Size RandomSize(this Random random, int minValue, int maxValue)
    {
        if (minValue <= 0)
            throw new ArgumentOutOfRangeException(nameof(minValue), "minValue must be positive");
        if (minValue > maxValue)
            throw new ArgumentOutOfRangeException(nameof(minValue), "minValue must be less than maxValue");
        
        return new Size(random.Next(minValue, maxValue), random.Next(minValue, maxValue));
    }
}