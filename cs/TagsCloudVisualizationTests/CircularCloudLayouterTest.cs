using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using TagsCloudVisualization.CloudLayouters;
using TagsCloudVisualization.Extensions;
using TagsCloudVisualization.Savers;
using TagsCloudVisualization.Visualizers;

namespace TagsCloudVisualizationTests;

[TestFixture]
[TestOf(typeof(CircularCloudLayouter))]
public class CircularCloudLayouterTest
{
    private readonly Random randomizer = new();
    private Rectangle[] testRectangles = null!;
    private const string ImagesDirectory = "testImages";
    private const string RectanglesDirectory = "testRectangles";
    
    [TearDown]
    public void TearDown()
    {
        var currentContext = TestContext.CurrentContext;
        if (currentContext.Result.Outcome.Status != TestStatus.Failed)
            return;
        
        var visualizer = new DefaultVisualizer(GetLayoutSize(testRectangles));
        
        using var bitmap = visualizer.CreateBitmap(testRectangles);
        var saver = new DefaultBitmapSaver(ImagesDirectory);
        saver.SaveBitmap(bitmap, currentContext.Test.Name + ".jpg");

        SaveInformationAboutTestRectangles(currentContext);
        
        TestContext.Out
            .WriteLine($"Tag cloud visualization saved to file " +
                       $"{Path.Combine(ImagesDirectory, currentContext.Test.Name + ".jpg")}");
        TestContext.Out
            .WriteLine($"Information about testRectangles saved to file " +
                       $"{Path.Combine(RectanglesDirectory, currentContext.Test.Name + "txt")}");
    }
    
    [Test]
    public void PutNextRectangle_ShouldReturnRectangle()
    {
        var circularCloudLayouter = SetupLayouterWithRandomParameters();
        var rectangleSize = randomizer.RandomSize();

        var rectangle = circularCloudLayouter.PutNextRectangle(rectangleSize);
        
        testRectangles = [rectangle];
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
        
        testRectangles = [actualRectangle];
        actualRectangle.Should().BeEquivalentTo(expectedRectangle);
    }

    [Test]
    public void PutNextRectangle_ShouldReturnRectangleWithRightSize()
    {
        var rectangleSize = randomizer.RandomSize();
        var circularCloudLayouter = SetupLayouterWithRandomParameters();

        var actualRectangle = circularCloudLayouter.PutNextRectangle(rectangleSize);
        
        testRectangles = [actualRectangle];
        actualRectangle.Size.Should().Be(rectangleSize);
    }
    
    [Test]
    [Repeat(10)]
    public void PutNextRectangle_ShouldReturnRectanglesWithoutIntersections()
    {
        var rectanglesNumber = randomizer.Next(100, 250);
        var circularCloudLayouter = SetupLayouterWithRandomParameters();
        
        testRectangles = PutRectanglesInLayouter(rectanglesNumber, circularCloudLayouter);
        
        IsIntersectionBetweenRectangles(testRectangles).Should().BeFalse();
    }

    [Test]
    [Repeat(10)]
    public void ShouldGenerateLayoutThatHasHighTightnessAndShapeOfCircularCloud_WhenOptimalParametersAreUsed()
    {
        const double allowableDelta = 0.35;

        var circularCloudLayouter = SetupLayouterWithOptimalParameters(); 
        testRectangles = PutRectanglesInLayouter(randomizer.Next(800, 1200), circularCloudLayouter);

        var layoutSize = GetLayoutSize(testRectangles);
        
        var circumcircleDiameter = Math.Max(layoutSize.Height, layoutSize.Width);
        var circumcircleArea = Math.PI * Math.Pow(circumcircleDiameter, 2) / 4;
        var rectanglesArea = (double)testRectangles
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

    private void SaveInformationAboutTestRectangles(TestContext currentContext)
    {
        var rectanglesPath = Path.Combine(RectanglesDirectory, 
            currentContext.Test.Name + ".txt");
        Directory.CreateDirectory(RectanglesDirectory);
        File.WriteAllLines(rectanglesPath, 
            testRectangles.Select(rectangle => rectangle.ToString()).ToArray());
    }
    
    private Size GetLayoutSize(IEnumerable<Rectangle> rectangles)
    {
        var layoutWidth = testRectangles.Max(rectangle => rectangle.Right) - 
                          testRectangles.Min(rectangle => rectangle.Left);
        var layoutHeight = testRectangles.Max(rectangle => rectangle.Top) -
                           testRectangles.Min(rectangle => rectangle.Bottom);
        return new Size(layoutWidth, layoutHeight);
    }
    
}