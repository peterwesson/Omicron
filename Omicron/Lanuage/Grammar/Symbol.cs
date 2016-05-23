namespace Omicron.Lanuage.Grammar
{
    public class Symbol
    {
        public const string Epsilon = "$epsilon";

        public const string End = "$end";

        public SymbolType Type { get; set; }

        public string TerminalSymbol { get; set; }

        public override bool Equals(object obj)
        {
            var symbol = obj as Symbol;
            if (symbol != null)
            {
                return Equals(symbol);
            }
            return base.Equals(obj);
        }

        protected bool Equals(Symbol other)
        {
            if (other.Type == SymbolType.TerminalSymbol)
            {
                return other.TerminalSymbol == TerminalSymbol;
            }

            return other.Type == Type;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                if (Type == SymbolType.TerminalSymbol)
                {
                    return TerminalSymbol.GetHashCode();
                }
                return (int)Type * 397;
            }
        }
    }
}