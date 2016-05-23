namespace Omicron.Analysis
{
    public abstract class BaseAnalyser<TIn, TOut> : IAnalyser<TIn, TOut>
    {
        public abstract TOut Parse(TIn input);

        public TOut Parse(object input)
        {
            return Parse((TIn)input);
        }
    }
}