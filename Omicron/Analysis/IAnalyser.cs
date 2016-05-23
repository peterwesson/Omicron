namespace Omicron.Analysis
{
    public interface IAnalyser<in TIn, out TOut> : IAnalyser<TOut>
    {
        TOut Parse(TIn input);
    }

    public interface IAnalyser<out TOut>
    {
        TOut Parse(object input);
    }
}