using System;

namespace SuperCalculator
{
    public static class OperatorHelper
    {
        public static Operator ToOperator(char c)
        {
            return c switch
            {
                '+' => Operator.Add,
                '-' => Operator.Sub,
                '*' => Operator.Mul,
                '/' => Operator.Div,
                _ => throw new NotSupportedException()
            };
        }

        public static Operator FirstOperator(string str)
        {
            foreach (char c in str)
            {
                switch (c)
                {
                    case '+': return Operator.Add;
                    case '-': return Operator.Sub;
                    case '*': return Operator.Mul;
                    case '/': return Operator.Div;
                }
            }

            return Operator.Mul;
        }

        public static bool IsOperator(char c)
        {
            return c switch
            {
                '+' => true,
                '-' => true,
                '*' => true,
                '/' => true,
                _ => false
            };
        }
    }
}