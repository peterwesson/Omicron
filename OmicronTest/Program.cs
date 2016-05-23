using System.Collections.Generic;
using System.IO;
using System.Linq;

using Omicron;
using Omicron.Analysis.LexicalAnalysis;
using Omicron.Analysis.SyntaxAnalysis;
using Omicron.Analysis.SyntaxAnalysis.SyntaxStacks;
using Omicron.Lanuage.Grammar;

namespace OmicronTest
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string code;

            using (var sr = new StreamReader(args[0]))
            {
                code = sr.ReadToEnd();
            }

            var parseTableFactory = new ParseTableFactory();

            var states = parseTableFactory.GetStates().ToList();

            var stateExporter = new ParserStateExporter(states);

            stateExporter.Export("C:\\output.csv");

            var compiler = new Compiler<string, IEnumerable<SyntaxStackItem>>()
                .AddAnalyser(new LexicalAnalyser())
                .AddAnalyser(new SyntaxAnalyser(states.First()));

            var syntax = compiler.Parse(code);
        }
    }
}