using System.Collections.Generic;

using Omicron.Analysis.SyntaxAnalysis.SyntaxStacks;

namespace Omicron.Analysis.SemanticAnalysis
{
    public class SemanticAnalyser : BaseAnalyser<IEnumerable<SyntaxStackItem>, IEnumerable<SyntaxStackItem>>
    {
        public override IEnumerable<SyntaxStackItem> Parse(IEnumerable<SyntaxStackItem> input)
        {
            return input;
        }
    }
}