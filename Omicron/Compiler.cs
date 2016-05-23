using System;
using System.Collections.Generic;
using System.Linq;

using Omicron.Analysis;

namespace Omicron
{
    public class Compiler<TIn, TOut>
    {
        private readonly ICollection<IAnalyser<object>> _analysers = new List<IAnalyser<object>>();
        
        public Compiler<TIn, TOut> AddAnalyser<TAnalyserIn, TAnalyserOut>(IAnalyser<TAnalyserIn, TAnalyserOut> analyser)
        {
            _analysers.Add(analyser as IAnalyser<object>);

            return this;
        }

        public TOut Parse(TIn input)
        {
            if (!_analysers.Any())
            {
                throw new Exception("No pipeline stages detected");
            }

            object tempVal = input;

            foreach (var analyser in _analysers.Where(a => a != _analysers.Last()))
            {
                var newInput = tempVal;

                tempVal = analyser.Parse(newInput);
            }

            var lastAnalyser = _analysers.Last();

            if (lastAnalyser is IAnalyser<TOut>)
            {
                return (lastAnalyser as IAnalyser<TOut>).Parse(tempVal);
            }

            throw new Exception("Unable to construct pipeline");
        }
    }
}