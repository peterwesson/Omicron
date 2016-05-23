using System.Collections.Generic;

namespace Omicron.Lanuage.Grammar
{
    public class Rule
    {
        public int RuleId { get; set; }

        public SymbolType Reduction { get; set; }

        public List<Symbol> Symbols { get; set; }

        public override bool Equals(object obj)
        {
            var rule = obj as Rule;
            if (rule != null)
            {
                return Equals(rule);
            }
            return base.Equals(obj);
        }

        protected bool Equals(Rule other)
        {
            return RuleId == other.RuleId;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return RuleId * 397;
            }
        }
    }
}