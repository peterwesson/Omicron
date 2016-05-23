using System.Collections.Generic;
using System.Linq;

using Omicron.Analysis.SyntaxAnalysis.SyntaxStacks;

namespace Omicron.Lanuage.Grammar
{
    public class 
        ParseTableFactory
    {
        private readonly Queue<SymbolType> _nonTerminalQueue;

        private readonly Dictionary<SymbolType, List<string>> _first;

        private readonly List<ItemSet> _itemSets;

        private readonly List<Transition> _transitions;

        private int _stateId;

        public ParseTableFactory()
        {
            _itemSets  = new List<ItemSet>();
            _nonTerminalQueue = new Queue<SymbolType>();
            _first = new Dictionary<SymbolType, List<string>>();
            _transitions = new List<Transition>();

            CreateFirstSets();
        }

        public IEnumerable<ParserState> GetStates()
        {
            var itemSet0 = GetInitialItemSet();

            _itemSets.Add(itemSet0);

            GenerateItemSets(itemSet0);

            RemoveDuplicates();

            var states = new List<ParserState>();

            foreach (var itemSet in _itemSets)
            {
                var state = new ParserState
                {
                    StateId = _stateId++
                };

                itemSet.State = state;


                states.Add(state);
            }

            foreach (var itemSet in _itemSets)
            {
                foreach (var transition in itemSet.Transitions)
                {
                    if (transition.Symbol.Type == SymbolType.TerminalSymbol)
                    {
                        itemSet.State.RegisterLookAheadAction(transition.Symbol.TerminalSymbol, 
                            transition.End
                            ? new ParseAction { Type = ParseActionType.End } :
                            new ParseAction { NextState = transition.To.State, Type = ParseActionType.Shift });
                    }
                    else
                    {
                        itemSet.State.RegisterGotoState(transition.Symbol.Type, transition.To.State);
                    }
                }

                foreach (var reduction in itemSet.Reductions)
                {
                    itemSet.State.RegisterLookAheadAction(reduction.Terminal, new ParseAction { Rule = reduction.Rule, Type = ParseActionType.Reduce });
                }
            }

            return states;
        }

        private void GenerateItemSets(ItemSet itemSet)
        {
            var symbols = itemSet.Where(item => item.Rule.Symbols.Count > item.Position)
                        .Select(item => item.Rule.Symbols[item.Position])
                        .Distinct().ToList();

            var reductions = itemSet.Where(item => item.Rule.Symbols.Count == item.Position)
                .SelectMany(item => Follow(_itemSets.First(), item.Rule.Reduction)).Distinct()
                .Select(terminal => new Symbol() { Type = SymbolType.TerminalSymbol, TerminalSymbol = terminal }).ToList();

            symbols = symbols.Concat(reductions).Distinct().ToList();

            var nonTerminals = symbols.Where(symbol => symbol.Type != SymbolType.TerminalSymbol).Select(symbol => symbol.Type);

            var firstSymbols = nonTerminals.SelectMany(First).ToList();

            symbols.AddRange(firstSymbols);

            symbols = symbols.Distinct().ToList();

            var itemSets = symbols.Select(symbol =>
                new ItemSet(itemSet
                    .Where(item => item.Rule.Symbols.Count > item.Position)
                    .Where(item => item.Rule.Symbols[item.Position].Equals(symbol))
                    .Select(item => new Item
                    {
                         LookAhead = item.LookAhead,
                         Rule = item.Rule,
                         Position = item.Position + 1
                    }).Distinct().ToList())
                {
                    TransitionSymbol = symbol,
                })
                .Where(items => items.Any()).ToList();

            var itemSetsExtra = symbols.Select(symbol =>
                new ItemSet(_itemSets.First()
                    .Where(item => item.Rule.Symbols.Count > item.Position)
                    .Where(item => item.Rule.Symbols[item.Position].Equals(symbol))
                    .Select(item => new Item
                    {
                        LookAhead = item.LookAhead,
                        Rule = item.Rule,
                        Position = item.Position + 1
                    }).Distinct().ToList())
                {
                    TransitionSymbol = symbol,
                })
                .Where(items => items.Any()).ToList();

            foreach (var items in itemSetsExtra)
            {
                var matchedSet = itemSets.FirstOrDefault(set => set.TransitionSymbol.Equals(items.TransitionSymbol));
                if (matchedSet == null)
                {
                    itemSets.Add(items);
                }
                else
                {
                    matchedSet.AddRange(items.Where(item => !matchedSet.Contains(item)).Where(item => item.Rule.Symbols.Count > 1));
                }
            }

            foreach (var items in itemSets)
            {
                AddTransitions(itemSet, items);
            }

            itemSets.RemoveAll(items => items.TransitionSymbol.TerminalSymbol == Symbol.End);

            foreach (var items in itemSets)
            {
                if (!_itemSets.Contains(items))
                {
                    _itemSets.Add(items);

                    GenerateItemSets(items);
                }
            }
        }

        private void RemoveDuplicates()
        {
            var keepList = new List<ItemSet>();
            foreach (var itemSet in _itemSets.ToList())
            {
                var original = keepList.FirstOrDefault(i => i.Equals(itemSet));

                if (original == null)
                {
                    keepList.Add(itemSet);
                }
            }

            foreach (var transition in _transitions.Where(transition => !transition.End))
            {
                var state = keepList.Last(itemSet => itemSet.Equals(transition.To));

                transition.To = state;
            }

            _itemSets.Clear();
            _itemSets.AddRange(keepList);
        }

        private void AddTransitions(ItemSet parentItemSet, ItemSet childItemSet)
        {
            if (parentItemSet.Transitions.All(transition => !transition.Symbol.Equals(childItemSet.TransitionSymbol)))
            {
                if (childItemSet.TransitionSymbol.TerminalSymbol != Symbol.End)
                {
                    var transition = new Transition
                    {
                        To = childItemSet,
                        Symbol = childItemSet.TransitionSymbol
                    };

                    parentItemSet.Transitions.Add(transition);
                    _transitions.Add(transition);
                }
                else
                {
                    var transition = new Transition
                    {
                        End = true,
                        Symbol = childItemSet.TransitionSymbol
                    };

                    parentItemSet.Transitions.Add(transition);
                    _transitions.Add(transition);
                }
            }

            foreach (var item in childItemSet)
            {
                if (item.Position == item.Rule.Symbols.Count)
                {
                    childItemSet.Reductions.Add(new Reduction { Rule = item.Rule, Terminal = item.LookAhead });
                }
            }
        }

        private IEnumerable<Rule> GetAllRules()
        {
            return typeof(Rules).GetProperties()
                    .Where(p => p.PropertyType == typeof(Rule))
                    .Select(p => (Rule)p.GetGetMethod().Invoke(null, null)).ToList();
        }

        private IEnumerable<Rule> GetAugmentedGrammar()
        {
            var augmentedGrammar = new List<Rule>
            {
                new Rule
                {
                    RuleId = 0,
                    Reduction = SymbolType.Goal,
                    Symbols = new List<Symbol>
                    {
                        new Symbol { Type = SymbolType.Expression },
                        new Symbol { Type = SymbolType.TerminalSymbol, TerminalSymbol = Symbol.End }
                    }
                }
            };

            var rules = GetAllRules();

            augmentedGrammar.AddRange(rules);

            return augmentedGrammar;

        }

        private void CreateFirstSets()
        {
            var nonTerminals = GetAugmentedGrammar().Select(r => r.Reduction).Distinct();

            foreach (var nonTerminal in nonTerminals)
            {
                _nonTerminalQueue.Enqueue(nonTerminal);
                _first[nonTerminal] = new List<string>();
            }

            while (_nonTerminalQueue.Count > 1)
            {
                var nonTerminal = _nonTerminalQueue.Dequeue();

                var firstCount = _first[nonTerminal].Count;

                this.CreateFirstSetsIteration(nonTerminal);

                if (_first[nonTerminal].Count > firstCount || _first[nonTerminal].Count == 0)
                {
                    _nonTerminalQueue.Enqueue(nonTerminal);
                }
            }
        }

        private void CreateFirstSetsIteration(SymbolType nonTerminal)
        {
            var rules = GetAugmentedGrammar().Where(rule => rule.Reduction == nonTerminal);

            foreach (var rule in rules)
            {
                if (rule.Symbols.Count == 0)
                {
                    if (!_first[nonTerminal].Contains(Symbol.Epsilon))
                    {
                        _first[nonTerminal].Add(Symbol.Epsilon);
                    }
                }
                else if (rule.Symbols.First().Type == SymbolType.TerminalSymbol && rule.Symbols.First().TerminalSymbol != Symbol.Epsilon)
                {
                    if (!_first[nonTerminal].Contains(rule.Symbols.First().TerminalSymbol))
                    {
                        _first[nonTerminal].Add(rule.Symbols.First().TerminalSymbol);
                    }
                }
                else if (rule.Symbols.First().Type != SymbolType.TerminalSymbol && rule.Symbols.First().Type != nonTerminal)
                {
                    foreach (var terminal in _first[rule.Symbols.First().Type].Where(terminal => terminal != Symbol.Epsilon))
                    {
                        if (!_first[nonTerminal].Contains(terminal))
                        {
                            _first[nonTerminal].Add(terminal);
                        }
                    }

                    if (rule.Symbols.Count > 1 && _first[rule.Symbols.First().Type].Contains(Symbol.Epsilon))
                    {
                        this.CreateFirstSetsIteration(rule.Symbols[1].Type);
                    }
                }
            }
        }

        public ItemSet GetInitialItemSet()
        {
            var initialRule = GetAugmentedGrammar().First();

            var lr0Set = new ItemSet(GetAugmentedGrammar().Select(rule => new Item { Rule = rule, LookAhead = Symbol.End }).ToList());

            var closure = GetAugmentedGrammar().SelectMany(rule => Follow(lr0Set, rule.Reduction).Select(terminal => new Item { Rule = rule, Position = 0, LookAhead = terminal })).ToList();

            var itemSet = new ItemSet { new Item { Rule = initialRule, LookAhead = Symbol.End } };

            itemSet.AddRange(closure);

            return itemSet;
        }

        private IEnumerable<string> Follow(ItemSet itemSet, SymbolType nonTerminal)
        {
            var nonTerminals = itemSet.Where(item => item.Rule.Symbols.Last().Type == nonTerminal).Select(item => item.Rule.Reduction).ToList();

            var symbols =
                itemSet.Where(item => item.Rule.Symbols.Select(symbol => symbol.Type).Contains(nonTerminal))
                    .Where(
                        item =>
                        item.Rule.Symbols.Count
                        > item.Rule.Symbols.Select(symbol => symbol.Type).ToList().IndexOf(nonTerminal) + 1)
                    .Select(item => item.Rule.Symbols[item.Rule.Symbols.Select(symbol => symbol.Type).ToList().IndexOf(nonTerminal) + 1]).ToList();

            nonTerminals.AddRange(symbols.Where(symbol => symbol.Type != SymbolType.TerminalSymbol).Select(symbol => symbol.Type));

            var terminals = symbols.Where(symbol => symbol.Type == SymbolType.TerminalSymbol).Select(symbol => symbol.TerminalSymbol).ToList();

            terminals.AddRange(nonTerminals.SelectMany(n => Follow(itemSet, n)));

            return terminals.Distinct();
        }

        private IEnumerable<Symbol> First(SymbolType nonTerminal)
        {
            var rules = GetAugmentedGrammar().Where(rule => rule.Reduction == nonTerminal);

            var symbols = rules.Select(rule => rule.Symbols.First()).Where(symbol => symbol.Type != nonTerminal).Distinct().ToList();

            var newSymbols = symbols.Where(symbol => symbol.Type != SymbolType.TerminalSymbol).SelectMany(symbol => First(symbol.Type)).ToList();
            
            symbols.AddRange(newSymbols);

            return symbols.Distinct();
        }
    }
}