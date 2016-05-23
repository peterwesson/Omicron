using System;
using System.Collections.Generic;
using System.Linq;

using Omicron.Analysis.LexicalAnalysis.Tokens;
using Omicron.Analysis.SyntaxAnalysis.SyntaxStacks;
using Omicron.Lanuage.Grammar;
using Omicron.LexicalAnalysis.Tokens;

namespace Omicron.Analysis.SyntaxAnalysis
{
    public class SyntaxAnalyser : BaseAnalyser<IEnumerable<Token>, IEnumerable<SyntaxStackItem>>
    {
        private readonly SyntaxStack _stack;

        private readonly ParserState _initialState;

        private ICollection<Token> _input;

        private ParserState _currentState;

        private bool _done;

        public SyntaxAnalyser(ParserState initialState)
        {
            _stack = new SyntaxStack();

            _initialState = initialState;
        }

        public override IEnumerable<SyntaxStackItem> Parse(IEnumerable<Token> input)
        {
            _input = input.ToList();
            _currentState = _initialState;

            while (!_done)
            {
                Advance();
            }

            return _stack.ToList();
        }

        private void Advance()
        {
            var lookAhead = NextSymbol();
            var action = _currentState.LookAhead(lookAhead);

            Console.WriteLine("Current State: {0}, Look Ahead: {1}", _currentState.StateId, lookAhead);

            switch (action.Type)
            {
                case ParseActionType.Shift:
                    Console.WriteLine("Shift: {0}", action.NextState.StateId);
                    Shift(action.NextState);
                    _currentState = action.NextState;
                    break;
                case ParseActionType.Reduce:
                    Console.WriteLine("Reduce: {0}", action.Rule.RuleId);
                    Reduce(action.Rule);
                    break;
                case ParseActionType.End:
                    _done = true;
                    break;
            }
        }

        private string NextSymbol()
        {
            if (_input.Any())
            {
                var nextToken = _input.First();

                switch (nextToken.Type)
                {
                    case TokenType.Identifier:
                    case TokenType.IntConst:
                    case TokenType.StrConst:
                        return nextToken.Type.ToString();
                    case TokenType.Bracket:
                    case TokenType.SemiColon:
                    case TokenType.Operator:
                    case TokenType.Keyword:
                        return nextToken.Symbol;
                }
            }

            return Symbol.End;
        }

        private void Shift(ParserState state)
        {
            var top = _input.First();

            _stack.Push(new SyntaxStackItem { Token = top, Type = SymbolType.TerminalSymbol, State = state });

            _input = _input.Skip(1).ToList();
        }

        private void Reduce(Rule rule)
        {
            var syntaxStackItem = new SyntaxStackItem { ChildItems = _stack.Pop(rule.Symbols.Count).ToList(), Type = rule.Reduction, State = _currentState };

            if (_stack.Empty())
            {
                _currentState = _initialState.ReduceTo(rule.Reduction);
            }
            else
            {
                var top = _stack.Peek();

                _currentState = top.State.ReduceTo(rule.Reduction);
            }

            _stack.Push(syntaxStackItem);
        }
    }
}