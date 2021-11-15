using System;
using SuperCalculator.Expressions;

namespace SuperCalculator
{
    public class Parser
    {
        public IExpression Parse(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) throw new ArgumentNullException(nameof(input));
            var trimmed = input.Trim();

            if (int.TryParse(trimmed, out var x)) return new ValueExpression(x);


            var hasEdgeParenthesis = Parenthesis.HasEdgeParenthesis(trimmed);
            if (hasEdgeParenthesis) // devide in to two expressions
            {
                if (Parenthesis.HasWrappedParenthesis(trimmed))
                {
                    trimmed = trimmed.Substring(1, trimmed.Length - 2);
                    return Parse(trimmed);
                }

                //((2+2)/2)*2
                var (pString, lower, upper) = Parenthesis.Extract(trimmed);

                var rest = lower == 0
                    ? trimmed[(upper)..]
                    : trimmed[..(lower)];
                var otherExp = ParserUtils.RemoveEdgeOperator(rest);
                var left = Parse(pString.Trim());
                var right = Parse(otherExp.Trim());
                var mod = OperatorHelper.FirstOperator(rest);

                return new Expression(left, right, mod);
            }

            // 1+1
            //40(1+1)
            //40(0-100)+1000

            var isLeftContext = true;
            var (@operator, indexOfOperator) = GetNextOperatorOrDefaultInContext(trimmed, isLeftContext);

            if (indexOfOperator == -1)
            {
                var positionOfContextCloser = isLeftContext
                    ? Parenthesis.IndexOfFirstRight(trimmed)
                    : Parenthesis.IndexOfFirstLeft(trimmed);
                return new Expression(
                    Parse(trimmed[..positionOfContextCloser]),
                    Parse(trimmed[positionOfContextCloser..]),
                    @operator);
            }

            if (@operator is Operator.Mul or Operator.Div)
            {
                var str = trimmed[(indexOfOperator + 1)..];
                var prevIndex = indexOfOperator;
                (@operator, indexOfOperator) = GetNextOperatorOrDefaultInContext(str, isLeftContext);
                indexOfOperator += indexOfOperator == -1 ? (prevIndex + 1) : 2; //correct for length
            }

            var l = trimmed[..(indexOfOperator)];
            var r = trimmed[(indexOfOperator + 1)..];
            return new Expression(
                Parse(l),
                Parse(r),
                @operator);
        }

        private (Operator, int) GetNextOperatorOrDefaultInContext(string str, bool isLeftContext)
        {
            var closingBracket = isLeftContext ? ')' : '(';
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