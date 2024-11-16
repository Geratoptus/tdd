using System;
using System.Drawing;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudVisualization.CloudLayouters;
using TagsCloudVisualization.Extensions;

namespace TagsCloudVisualizationTests;

[TestFixture]
[TestOf(typeof(CircularCloudLayouter))]
public class CircularCloudLayouterTest
{

    [Test]
    [Repeat(10)]
    public void PutNextRectangle_ShouldReturnRectanglesWithoutIntersections()
    {
        var randomizer = new Random();
        var rectanglesNumber = randomizer.Next(100, 250);
        var rectangles = new Rectangle[rectanglesNumber];
        var circularCloudLayouter = new CircularCloudLayouter(new Point(0, 0), 1, 0.5);
        
        for (var i = 0; i < rectanglesNumber; i++)
            rectangles[i] = circularCloudLayouter.PutNextRectangle(randomizer.RandomSize(10, 25));

        IsIntersectionBetweenRectangles(rectangles).Should().BeFalse();
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