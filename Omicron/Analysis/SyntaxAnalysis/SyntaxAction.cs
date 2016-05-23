namespace Omicron.Analysis.SyntaxAnalysis
{
    public enum SyntaxAction
    {
        Shift,
        Reduce,
        End,
        Error,
        Conflict
    }
}