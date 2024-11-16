using System;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudVisualization.Extensions;

namespace TagsCloudVisualizationTests.Extensions;

[TestFixture]
[TestOf(typeof(RandomExtension))]
public class RandomExtensionTest
{
    private const int Seed = 123452;
    private readonly Random random = new(Seed);

    [TestCase(0, 5, Description = "Min value is zero")]
    [TestCase(-1, 5, Description = "Min value is negative")]
    [TestCase(10, 5, Description = "Min value is greater than max")]
    public void RandomSize_ShouldThrowArgumentOutOfRangeException_IfWrongParameters(int minValue, int maxValue)
    {
        var randomSizeInvoke = () => random.RandomSize(minValue, maxValue);
        randomSizeInvoke.Should().Throw<ArgumentOutOfRangeException>();
    }
}