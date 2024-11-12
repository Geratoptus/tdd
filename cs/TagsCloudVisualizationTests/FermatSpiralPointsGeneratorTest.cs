using System;
using NUnit.Framework;
using TagsCloudVisualization;
using FluentAssertions;

namespace TagsCloudVisualizationTests;

[TestFixture]
[TestOf(typeof(FermatSpiralPointsGenerator))]
public class FermatSpiralPointsGeneratorTest
{

    [TestCase(-1, 1, Description = "Negative radius"),
     TestCase(0, 1, Description = "Radius is zero"),
     TestCase(1, 0, Description = "AngleOffset is zero"),
     TestCase(1, -1, Description = "Negative angleOffset")]
    public void ShouldThrowArgumentException_AfterWrongCreation(double radius, double angleOffset)
    {
        var creation = () => new FermatSpiralPointsGenerator(radius, angleOffset);
        creation.Should().Throw<ArgumentException>();
        
    }
}