using System.Collections.Generic;

using Omicron.Analysis.LexicalAnalysis.Tokens;

namespace Omicron.Lanuage.Grammar
{
    public static class Rules
    {
        public static Rule Rule1
        {
            get
            {
                return new Rule
                {
                    RuleId = 1,
                    Reduction = SymbolType.Sums,
                    Symbols = new List<Symbol>
                    {
                        new Symbol { Type = SymbolType.Sums },
                        new Symbol { Type = SymbolType.TerminalSymbol, TerminalSymbol = "+" },
                        new Symbol { Type = SymbolType.Products }
                    }
                };
            }
        }

        public static Rule Rule2
        {
            get
            {
                return new Rule
                {
                    RuleId = 2,
                    Reduction = SymbolType.Sums,
                    Symbols = new List<Symbol>
                    {
                        new Symbol { Type = SymbolType.Products }
                    }
                };
            }
        }

        public static Rule Rule3
        {
            get
            {
                return new Rule
                {
                    RuleId = 3,
                    Reduction = SymbolType.Products,
                    Symbols = new List<Symbol>
                    {
                        new Symbol { Type = SymbolType.Products },
                        new Symbol { Type = SymbolType.TerminalSymbol, TerminalSymbol = "*" },
                        new Symbol { Type = SymbolType.Value }
                    }
                };
            }
        }

        public static Rule Rule4
        {
            get
            {
                return new Rule
                {
                    RuleId = 4,
                    Reduction = SymbolType.Products,
                    Symbols = new List<Symbol>
                    {
                        new Symbol { Type = SymbolType.Value }
                    }
                };
            }
        }

        public static Rule Rule5
        {
            get
            {
                return new Rule
                {
                    RuleId = 5,
                    Reduction = SymbolType.Value,
                    Symbols = new List<Symbol>
                    {
                        new Symbol { Type = SymbolType.TerminalSymbol, TerminalSymbol = TokenType.IntConst.ToString() }
                    }
                };
            }
        }

        public static Rule Rule6
        {
            get
            {
                return new Rule
                {
                    RuleId = 6,
                    Reduction = SymbolType.Value,
                    Symbols = new List<Symbol>
                    {
                        new Symbol{ Type = SymbolType.TerminalSymbol, TerminalSymbol = TokenType.Identifier.ToString() }
                    }
                };
            }
        }

        public static Rule Rule7
        {
            get
            {
                return new Rule
                {
                    RuleId = 7,
                    Reduction = SymbolType.Expression,
                    Symbols = new List<Symbol>
                    {
                        new Symbol { Type = SymbolType.Sums }
                    }
                };
            }
        }

        public static Rule Rule8
        {
            get
            {
                return new Rule
                {
                    RuleId = 8,
                    Reduction = SymbolType.Products,
                    Symbols = new List<Symbol>
                    {
                        new Symbol { Type = SymbolType.Products },
                        new Symbol { Type = SymbolType.TerminalSymbol, TerminalSymbol = "/" },
                        new Symbol { Type = SymbolType.Value }
                    }
                };
            }
        }

        public static Rule Rule9
        {
            get
            {
                return new Rule
                {
                    RuleId = 9,
                    Reduction = SymbolType.Sums,
                    Symbols = new List<Symbol>
                    {
                        new Symbol { Type = SymbolType.Sums },
                        new Symbol { Type = SymbolType.TerminalSymbol, TerminalSymbol = "-" },
                        new Symbol { Type = SymbolType.Products }
                    }
                };
            }
        }

        public static Rule Rule10
        {
            get
            {
                return new Rule
                {
                    RuleId = 10,
                    Reduction = SymbolType.Value,
                    Symbols = new List<Symbol>
                    {
                        new Symbol { Type = SymbolType.TerminalSymbol, TerminalSymbol = "(" },
                        new Symbol { Type = SymbolType.Expression },
                        new Symbol { Type = SymbolType.TerminalSymbol, TerminalSymbol = ")" }
                    }
                };
            }
        }
    }
}
