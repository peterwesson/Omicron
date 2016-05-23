using Omicron.Analysis.LexicalAnalysis.Tokens;

namespace Omicron.LexicalAnalysis.Tokens
{
    public class Token
    {
        public TokenType Type { get; private set; }

        public string Symbol { get; private set; }

        public int LineNumber { get; private set; }

        public Token(TokenType type, string symbol, int lineNumber)
        {
            Type = type;
            Symbol = symbol;
            LineNumber = lineNumber;
        }
    }
}
