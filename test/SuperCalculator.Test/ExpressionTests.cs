using FluentAssertions;
using SuperCalculator.Expressions;
using Xunit;

namespace SuperCalculator.Test
{
    [Trait("Category", "Unit")]
    public class ExpressionTests
    {
        [Fact]
        public void Compute_ShouldReturnExpected() =>
            new Expression(
                    new Expression(
                        new ValueExpression(1),
                        new ValueExpression(2),
                        Operator.Add
                    ), //1+2 IExpression
                    new ValueExpression(3), // 3 IExpression
                    Operator.Mul
                )
                .Compute()
                .Should()
                .Be(9);
    }
}