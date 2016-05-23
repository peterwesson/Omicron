using System.Collections.Generic;

using Omicron.Lanuage.Grammar;
using Omicron.LexicalAnalysis.Tokens;

namespace Omicron.Analysis.SyntaxAnalysis.SyntaxStacks
{
    public class SyntaxStackItem
    {
        public SymbolType Type { get; set; }

        public Token Token { get; set; }

        public List<SyntaxStackItem> ChildItems { get; set; }

        public ParserState State { get; set; }
}
}