using System;

namespace SuperCalculator
{
    public static class Parenthesis
    {
        public static bool HasWrappedParenthesis(string str)
        {
            // ( () () () ) == true
            // () () == false
            if (str[^1] != ')' || str[0] != '(') return false;

            var maxIndex = str.Length - 1;
            var openLefts = 0;
            for (var i = 0; i < str.Length; i++)
            {
                var c = str[i];
                if (c == '(') openLefts++;
                else if (c == ')')
                {
                    if (i != maxIndex && openLefts == 1)
                    {
                        return false;
                    }

                    openLefts--;
                }
            }

            return true;
        }

        public static bool HasEdgeParenthesis(string str) => str[0] == '(' || str[^1] == ')';
        public static int IndexOfFirstLeft(string str) => str.IndexOf('(', StringComparison.InvariantCulture);
        public static int IndexOfFirstRight(string str) => str.IndexOf(')', StringComparison.InvariantCulture);

        public static (string, int, int) Extract(string str)
        {
            var index = IndexOfFirstLeft(str);
            return index == 0
                ? ExtractFromLeftEdge(str)
                : ExtractFromRightEdge(str);
        }

        public static (string, int, int) ExtractFromRightEdge(string str)
        {
            // 1+(1-2)       len=7
            var length = 0;
            var openRights = 0;
            for (var i = str.Length - 1; i >= 0; i--)
            {
                var c = str[i];
                if (c == ')') openRights++;
                else if (c == '(')
                {
                    if (openRights == 1)
                    {
                        length = i;
                        break;
                    }

                    openRights--;
                }
            }

            var result = str[length..];
            return (result, length, str.Length - 1);
        }

        public static (string, int, int) ExtractFromLeftEdge(string str)
        {
            //((2+2)/2)*2
            var length = 0;
            var openLefts = 0;
            for (var i = 0; i < str.Length; i++)
            {
                var c = str[i];
                if (c == '(') openLefts++;
                else if (c == ')')
                {
                    if (openLefts == 1)
                    {
                        length = i + 1;
                        break;
                    }

                    openLefts--;
                }
            }

            var result = str[..length];
            return (result, 0, length);
        }
    }
}