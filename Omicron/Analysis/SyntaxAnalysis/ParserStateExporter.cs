using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Omicron.Analysis.SyntaxAnalysis.SyntaxStacks;
using Omicron.Lanuage.Grammar;

namespace Omicron.Analysis.SyntaxAnalysis
{
    public class ParserStateExporter
    {
        private readonly List<string> _terminals;

        private readonly List<SymbolType> _nonTerminals;

        private readonly List<ParserState> _states;

        private string _outputFile;

        public ParserStateExporter(List<ParserState> states)
        {
            _terminals = states.SelectMany(state => state.GetTerminals()).Distinct().ToList();
            _nonTerminals = states.SelectMany(state => state.GetNonTerminals()).Distinct().ToList();

            _states = states;
        }

        public void Export(string path)
        {
            ConstructCSV();

            using (var sw = new StreamWriter(path))
            {
                sw.Write(_outputFile);
            }
        }

        private void ConstructCSV()
        {
             _outputFile = string.Empty;

            _outputFile += "State,";

            foreach (var terminal in _terminals)
            {
                _outputFile += terminal + ",";
            }

            foreach (var nonTerminal in _nonTerminals)
            {
                _outputFile += nonTerminal.ToString();

                if (nonTerminal != _nonTerminals.Last())
                {
                    _outputFile += ",";
                }
            }

            _outputFile += Environment.NewLine;

            foreach (var state in _states)
            {
                _outputFile += state.StateId + ",";

                foreach (var terminal in _terminals)
                {
                    var action = state.LookAhead(terminal);

                    if (action != null)
                    {
                        switch (action.Type)
                        {
                            case ParseActionType.Shift:
                                _outputFile += "s" + action.NextState.StateId;
                                break;
                            case ParseActionType.Reduce:
                                _outputFile += "r" + action.Rule.RuleId;
                                break;
                            case ParseActionType.End:
                                _outputFile += "end";
                                break;
                        }
                    }

                    _outputFile += ",";
                }

                foreach (var nonTerminal in _nonTerminals)
                {
                    var reduction = state.ReduceTo(nonTerminal);

                    if (reduction != null)
                    {
                        _outputFile += reduction.StateId;
                    }

                    if (nonTerminal != _nonTerminals.Last())
                    {
                        _outputFile += ",";
                    }
                }

                _outputFile += Environment.NewLine;
            }
        }
    }
}
