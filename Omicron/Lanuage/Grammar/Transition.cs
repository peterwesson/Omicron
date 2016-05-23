namespace Omicron.Lanuage.Grammar
{
    public class Transition
    {
        public Symbol Symbol { get; set; }

        public ItemSet To { get; set; }

        public ItemSet From { get; set; }

        public bool End { get; set; }
    }
}