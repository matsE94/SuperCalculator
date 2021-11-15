using System;
using SuperCalculator.Expressions;

namespace SuperCalculator
{
    public class Parser
    {
        public IExpression Parse(string expression)
        {
            if (string.IsNullOrWhiteSpace(expression)) throw new ArgumentNullException(nameof(expression));
            expression = expression.Trim();

            if (int.TryParse(expression, out var x)) return new ValueExpression(x);


            var hasEdgeParenthesis = Parenthesis.HasEdgeParenthesis(expression);
            if (hasEdgeParenthesis) // devide in to two expressions
            {
                if (Parenthesis.HasWrappedParenthesis(expression))
                {
                    expression = expression.Substring(1, expression.Length - 2);
                    return Parse(expression);
                }

                //((2+2)/2)*2
                var (extracted, lower, upper) = Parenthesis.Extract(expression);
                var rest = lower == 0
                    ? expression[upper..]
                    : expression[..lower];
                var otherExp = ParserUtils.RemoveEdgeOperator(rest);
                return new Expression(
                    Parse(extracted),
                    Parse(otherExp),
                    GetNextOperatorOrDefaultInContext(rest).Item1);
            }

            // 1+1
            // 40(1+1)
            // 40(0-100)+1000
            var (@operator, indexOfOperator) = GetNextOperatorOrDefaultInContext(expression);

            if (indexOfOperator == -1)
            {
                var positionOfContextCloser = Parenthesis.IndexOfFirstRight(expression);
                return new Expression(
                    Parse(expression[..positionOfContextCloser]),
                    Parse(expression[positionOfContextCloser..]),
                    @operator);
            }

            if (@operator is Operator.Mul or Operator.Div)
            {
                var nextContext = expression[(indexOfOperator + 1)..];
                var prevIndex = indexOfOperator;
                (@operator, indexOfOperator) = GetNextOperatorOrDefaultInContext(nextContext);
                indexOfOperator += indexOfOperator == -1 ? ++prevIndex : 2; //correct for length
            }

            return new Expression(
                Parse(expression[..indexOfOperator]),
                Parse(expression[(indexOfOperator + 1)..]),
                @operator);
        }

        private (Operator, int) GetNextOperatorOrDefaultInContext(string str)
        {
            var closingBracket = ')';
            var defaultValue = (Operator.Mul, -1);
            for (var i = 0; i < str.Length; i++)
            {
                var c = str[i];
                var isEndOfScope = c == closingBracket;
                if (isEndOfScope) return defaultValue;
                switch (c)
                {
                    case '*': return (Operator.Mul, i);
                    case '/': return (Operator.Div, i);
                    case '+': return (Operator.Add, i);
                    case '-': return (Operator.Sub, i);
                }
            }

            return defaultValue;
        }
    }
}