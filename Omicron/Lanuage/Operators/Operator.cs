namespace Omicron.Lanuage.Operators
{
    public enum Operator
    {
        [Operator(Symbol = "*")]
        Multiplication,
        [Operator(Symbol = "/")]
        Division,
        [Operator(Symbol = "%")]
        Modulo,
        [Operator(Symbol = "+")]
        Addition,
        [Operator(Symbol = "-")]
        Subtraction,
        [Operator(Symbol = "<")]
        LessThan,
        [Operator(Symbol = ">")]
        GreaterThan,
        [Operator(Symbol = "<=")]
        LessThanOrEqual,
        [Operator(Symbol = ">=")]
        GreaterThanOrEqual,
        [Operator(Symbol = "==")]
        Equality,
        [Operator(Symbol = "!=")]
        Inequality,
        [Operator(Symbol = "&")]
        LogicalAnd,
        [Operator(Symbol = "^")]
        LogicalXor,
        [Operator(Symbol = "|")]
        LogicalOr,
        [Operator(Symbol = "&&")]
        ConditionalAnd,
        [Operator(Symbol = "||")]
        ConditionalOr,
        [Operator(Symbol = "=")]
        Assignment,
        [Operator(Symbol = "*=")]
        MultiplyAssignment,
        [Operator(Symbol = "/=")]
        DivideAssignment,
        [Operator(Symbol = "+=")]
        AddAssignment,
        [Operator(Symbol = "-=")]
        SubtractAssignment,
        [Operator(Symbol = "&=")]
        AndAssignment,
        [Operator(Symbol = "|=")]
        OrAssignment,
        [Operator(Symbol = "^=")]
        XorAssignment
    }
}