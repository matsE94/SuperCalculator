namespace SuperCalculator.Expressions
{
    public class ValueExpression : IExpression
    {
        private readonly decimal _value;

        public ValueExpression(decimal value) => _value = value;

        public decimal Compute() => _value;
    }
}