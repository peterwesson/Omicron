using System.Collections.Generic;

using Omicron.Lanuage.Grammar;

namespace Omicron.Analysis.SyntaxAnalysis.SyntaxStacks
{
    public class ParserState
    {
        private readonly IDictionary<string, ParseAction> _lookahead;

        private readonly IDictionary<SymbolType, ParserState> _goto;

        public int StateId { get; set; }

        public ParserState()
        {
            _lookahead = new Dictionary<string, ParseAction>();
            _goto = new Dictionary<SymbolType, ParserState>();
        }

        public void RegisterLookAheadAction(string terminalSymbol, ParseAction parserAction)
        {
            _lookahead.Add(terminalSymbol, parserAction);
        }

        public void RegisterGotoState(SymbolType nonTerminalSymbol, ParserState parserState)
        {
            _goto.Add(nonTerminalSymbol, parserState);
        }

        public ParseAction LookAhead(string terminalSymbol)
        {
            return _lookahead.ContainsKey(terminalSymbol) ? _lookahead[terminalSymbol] : null;
        }

        public ParserState ReduceTo(SymbolType nonTerminalSymbol)
        {
            return _goto.ContainsKey(nonTerminalSymbol) ? _goto[nonTerminalSymbol] : null;
        }

        public IEnumerable<string> GetTerminals()
        {
            return _lookahead.Keys;
        }

        public IEnumerable<SymbolType> GetNonTerminals()
        {
            return _goto.Keys;
        }
    }
}