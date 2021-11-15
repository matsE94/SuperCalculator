using FluentAssertions;
using Xunit;

namespace SuperCalculator.Test
{
    [Trait("Category", "Unit")]
    public class ParenthesisTests
    {
        [Theory]
        [InlineData("( () () )")]
        [InlineData("( () () () )")]
        [InlineData("(1+1)")]
        public void HasWrappedParenthesis_ShouldBeTrue(string input) =>
            Parenthesis.HasWrappedParenthesis(input).Should().BeTrue();


        [Theory]
        [InlineData("() ()")]
        [InlineData("( ) (())")]
        [InlineData("(1+1)()")]
        public void HasWrappedParenthesis_ShouldBeFalse(string input) =>
            Parenthesis.HasWrappedParenthesis(input).Should().BeFalse();
    }
}