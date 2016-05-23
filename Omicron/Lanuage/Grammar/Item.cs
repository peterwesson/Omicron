namespace Omicron.Lanuage.Grammar
{
    public class Item
    {
        public Rule Rule { get; set; }

        public int Position { get; set; }

        public string LookAhead { get; set; }

        public override bool Equals(object obj)
        {
            var item = obj as Item;
            if (item != null)
            {
                return Equals(item);
            }
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (this.Rule != null ? this.Rule.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ this.Position;
                hashCode = (hashCode * 397) ^ (this.LookAhead != null ? this.LookAhead.GetHashCode() : 0);
                return hashCode;
            }
        }

        protected bool Equals(Item other)
        {
            return Rule.Equals(other.Rule) && Position == other.Position && LookAhead == other.LookAhead;
        }
    }
}
