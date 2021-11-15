using System;
using System.Text;

namespace SuperCalculator
{
    static class ParserUtils
    {
        public static string RemoveEdgeOperator(string str)
        {
            if (string.IsNullOrWhiteSpace(str)) throw new ArgumentNullException(nameof(str));
            var trimmed = str.Trim();

            var leftIsOperator = OperatorHelper.IsOperator(trimmed[0]);
            var rightIsOperator = OperatorHelper.IsOperator(trimmed[^1]);
            if (!leftIsOperator && !rightIsOperator)
            {
                return trimmed;
            }

            var indexToRemove = leftIsOperator
                ? 0
                : trimmed.Length - 1;
            var sb = new StringBuilder();
            for (var i = 0; i < trimmed.Length; i++)
            {
                if (i == indexToRemove) continue;
                sb.Append(trimmed[i]);
            }

            return sb.ToString();
        }
    }
}