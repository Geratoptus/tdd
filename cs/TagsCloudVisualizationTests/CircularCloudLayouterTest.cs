using System;
using System.Drawing;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudVisualization.CloudLayouters;
using TagsCloudVisualization.Extensions;

namespace TagsCloudVisualizationTests;

[TestFixture]
[TestOf(typeof(CircularCloudLayouter))]
public class CircularCloudLayouterTest
{
    private readonly Random randomizer = new();

    [Test]
    public void PutNextRectangle_ShouldReturnRectangle()
    {
        var circularCloudLayouter = SetupLayouterWithRandomParameters();
        var rectangleSize = randomizer.RandomSize();

        var rectangle = circularCloudLayouter.PutNextRectangle(rectangleSize);
        
        rectangle.Should().BeOfType<Rectangle>();
    }
    
    [Test]
    [Repeat(10)]
    public void PutNextRectangle_ShouldReturnRectangleInCenter_IfFirstInvoke()
    {
        var rectangleSize = randomizer.RandomSize();
        var circularCloudLayouter = SetupLayouterWithRandomParameters();
        
        var actualRectangle = circularCloudLayouter
            .PutNextRectangle(rectangleSize);
        var expectedRectangle = circularCloudLayouter
            .CreateRectangleWithCenter(circularCloudLayouter.LayoutCenter, rectangleSize);
        
        actualRectangle.Should().BeEquivalentTo(expectedRectangle);
    }

    [Test]
    public void PutNextRectangle_ShouldReturnRectangleWithRightSize()
    {
        var rectangleSize = randomizer.RandomSize();
        var circularCloudLayouter = SetupLayouterWithRandomParameters();

        var actualRectangle = circularCloudLayouter.PutNextRectangle(rectangleSize);
        
        actualRectangle.Size.Should().Be(rectangleSize);
    }
    
    [Test]
    [Repeat(10)]
    public void PutNextRectangle_ShouldReturnRectanglesWithoutIntersections()
    {
        var rectanglesNumber = randomizer.Next(100, 250);
        var circularCloudLayouter = SetupLayouterWithRandomParameters();
        
        var rectangles = PutRectanglesInLayouter(rectanglesNumber, circularCloudLayouter);
        
        IsIntersectionBetweenRectangles(rectangles).Should().BeFalse();
    }

    [Test]
    [Repeat(10)]
    public void ShouldGenerateLayoutThatHasHighTightnessAndShapeOfCircularCloud_WhenOptimalParametersAreUsed()
    {
        const double allowableDelta = 0.35;

        var circularCloudLayouter = SetupLayouterWithOptimalParameters();
        var rectangles = PutRectanglesInLayouter(randomizer.Next(800, 1200), circularCloudLayouter);

        var layoutWidth = rectangles.Max(rectangle => rectangle.Right) - 
                          rectangles.Min(rectangle => rectangle.Left);
        var layoutHeight = rectangles.Max(rectangle => rectangle.Top) -
                           rectangles.Min(rectangle => rectangle.Bottom);
        
        var circumcircleDiameter = Math.Max(layoutHeight, layoutWidth);
        var circumcircleArea = Math.PI * Math.Pow(circumcircleDiameter, 2) / 4;
        var rectanglesArea = (double)rectangles
            .Select(rectangle => rectangle.Height * rectangle.Width)
            .Sum();
        
        var areasFraction = circumcircleArea / rectanglesArea;
        areasFraction.Should().BeApproximately(1, allowableDelta);        
    }

    private Rectangle[] PutRectanglesInLayouter(int rectanglesNumber, 
        CircularCloudLayouter circularCloudLayouter)
    {
        return Enumerable
            .Range(0, rectanglesNumber)
            .Select(_ => randomizer.RandomSize(10, 25))
            .Select(circularCloudLayouter.PutNextRectangle)
            .ToArray();
    }

    private CircularCloudLayouter SetupLayouterWithOptimalParameters()
    {
        return new CircularCloudLayouter(new Point(0, 0), 1, 0.5);
    }
    
    private CircularCloudLayouter SetupLayouterWithRandomParameters()
    {
        var radius = randomizer.Next(1, 10);
        var angleOffset = randomizer.Next(1, 10);
        var layoutCenter = randomizer.RandomPoint(-10, 10);
        
        return new CircularCloudLayouter(layoutCenter, radius, angleOffset);
    }

    private bool IsIntersectionBetweenRectangles(Rectangle[] rectangles)
    {
        for (var i = 0; i < rectangles.Length; i++)
        {
            for (var j = i + 1; j < rectangles.Length; j++)
            {
                if (rectangles[i].IntersectsWith(rectangles[j]))
                    return true;
            }
        }

        return false;
    }
}