using FluentAssertions;
using Xunit;

namespace SuperCalculator.Test
{
    [Trait("Category", "EndToEnd")]
    public class ParserTests
    {
        [Theory]
        [InlineData("2+2", 4)]
        [InlineData("8-2", 6)]
        [InlineData("2*2-1", 3)]
        [InlineData("2+20", 22)]
        [InlineData("2+2+2", 6)]
        [InlineData("2*2+1", 5)]
        [InlineData("(2+2)", 4)]
        [InlineData("1+3*2", 7)]
        [InlineData("3*3+2", 11)]
        [InlineData("10+20", 30)]
        [InlineData("(1+1)2", 4)]
        [InlineData("(1+2)*3", 9)]
        [InlineData("3/(1+2)", 1)]
        [InlineData("2*2+2-2", 4)]
        [InlineData("((2+2)/2)*2", 4)]
        [InlineData("((2+4)/2)*2", 6)]
        [InlineData("(1+1) * (1+1)", 4)]
        [InlineData("(1+1) * (1+1) / (1*1)", 4)]
        [InlineData("(((((3+3)+1)+1)+1)+1)+1", 11)]
        public void Parse_ComputeShouldReturnExpected(string input, decimal expected) =>
            new Parser().Parse(input).Compute().Should().Be(expected);
    }
}