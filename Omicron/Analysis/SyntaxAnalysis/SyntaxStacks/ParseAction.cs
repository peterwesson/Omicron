using Omicron.Lanuage.Grammar;

namespace Omicron.Analysis.SyntaxAnalysis.SyntaxStacks
{
    public class ParseAction
    {
        public ParseActionType Type { get; set; }

        public ParserState NextState { get; set; }

        public Rule Rule { get; set; }
    }
}