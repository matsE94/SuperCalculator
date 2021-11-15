using System;

namespace SuperCalculator.Expressions
{
    public class Expression : IExpression
    {
        public Expression(IExpression left, IExpression right, Operator @operator)
        {
            Left = left;
            Right = right;
            Operator = @operator;
        }

        private IExpression Left { get; }
        private IExpression Right { get; }
        private Operator Operator { get; }

        public decimal Compute()
        {
            var value = Operator switch
            {
                Operator.Add => Left.Compute() + Right.Compute(),
                Operator.Sub => Left.Compute() - Right.Compute(),
                Operator.Mul => Left.Compute() * Right.Compute(),
                Operator.Div => Left.Compute() / Right.Compute(),
                _ => throw new NotSupportedException()
            };
            return value;
        }
    }
}