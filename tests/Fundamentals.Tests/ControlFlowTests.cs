using Fundamentals.Exercises.ControlFlow;

namespace Fundamentals.Tests;

public class ControlFlowTests
{
    [Theory]
    [InlineData(2024, true)]
    [InlineData(2023, false)]
    [InlineData(1900, false)]
    [InlineData(2000, true)]
    public void IsLeapYear_FollowsTheGregorianRules(int year, bool expected)
    {
        Assert.Equal(expected, LeapYear.IsLeapYear(year));
        Assert.Equal(DateTime.IsLeapYear(year), LeapYear.IsLeapYear(year));
    }

    [Fact]
    public void Normalize_CarriesOverflowingUnits()
    {
        (long days, int hours, int minutes, int seconds) = TimeNormalizer.Normalize(1, 25, 61, 3661);

        Assert.Equal((2L, 3, 2, 1), (days, hours, minutes, seconds));
    }

    [Fact]
    public void Normalize_LeavesSmallValuesUntouched()
    {
        // A do-while loop would subtract 60 seconds even when there are fewer than 60.
        Assert.Equal((0L, 0, 0, 30), TimeNormalizer.Normalize(0, 0, 0, 30));
    }

    [Theory]
    [InlineData(0, "zero")]
    [InlineData(7, "seven")]
    [InlineData(13, "thirteen")]
    [InlineData(40, "forty")]
    [InlineData(99, "ninety-nine")]
    public void ToWords_WritesNumbersInEnglish(int number, string expected)
    {
        Assert.Equal(expected, NumberToWords.ToWords(number));
    }

    [Fact]
    public void ToWords_RejectsNumbersOutOfRange()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => NumberToWords.ToWords(100));
    }

    [Fact]
    public void HollowRectangle_DrawsOnlyTheBorder()
    {
        Assert.Equal("XXXX\nX  X\nXXXX\n", HollowRectangle.Draw(4, 3));
        Assert.Equal("X\n", HollowRectangle.Draw(1, 1));
    }

    [Fact]
    public void MaxGap_FindsTheBiggestStep()
    {
        Assert.Equal(6, IncreasingSeries.MaxGap([1, 3, 9, 10]));
    }

    [Fact]
    public void MaxGap_RejectsSeriesThatDoNotIncrease()
    {
        Assert.Throws<ArgumentException>(() => IncreasingSeries.MaxGap([1, 5, 5]));
    }

    [Theory]
    [InlineData(10, "01")]
    [InlineData(47, "74")]
    public void ReverseTwoDigits_SwapsTheDigits(int number, string expected)
    {
        Assert.Equal(expected, ReverseTwoDigits.Reverse(number));
    }

    [Theory]
    [InlineData(10, 50, "too low")]
    [InlineData(80, 50, "too high")]
    [InlineData(50, 50, "Correct")]
    public void GuessTheNumber_GivesTheRightHint(int guess, int secret, string expectedFragment)
    {
        Assert.Contains(expectedFragment, GuessTheNumber.Hint(guess, secret));
    }

    [Fact]
    public void DescribeAboveThreshold_CountsNumbersAbove50()
    {
        Assert.Equal("None of them is above 50.", BooleanLogic.DescribeAboveThreshold([1, 2, 50]));
        Assert.Equal("2 of them are above 50.", BooleanLogic.DescribeAboveThreshold([51, 2, 99]));
        Assert.Equal("All of them are above 50.", BooleanLogic.DescribeAboveThreshold([51, 52, 53]));
    }

    [Fact]
    public void PopulationStats_ComputesPercentages()
    {
        PopulationStats.Group[] people =
        [
            PopulationStats.Group.Boy, PopulationStats.Group.Boy,
            PopulationStats.Group.Woman, PopulationStats.Group.Man,
        ];

        var summary = PopulationStats.Summarize(people).ToDictionary(s => s.Group);

        Assert.Equal(2, summary[PopulationStats.Group.Boy].Count);
        Assert.Equal(50.0, summary[PopulationStats.Group.Boy].Percentage, 3);
        Assert.Equal(0.0, summary[PopulationStats.Group.Girl].Percentage, 3);
    }

    [Theory]
    [InlineData("b", PopulationStats.Group.Boy)]
    [InlineData(" W ", PopulationStats.Group.Woman)]
    public void TryParseGroup_AcceptsLettersInAnyCase(string text, PopulationStats.Group expected)
    {
        Assert.True(PopulationStats.TryParseGroup(text, out PopulationStats.Group group));
        Assert.Equal(expected, group);
    }
}
