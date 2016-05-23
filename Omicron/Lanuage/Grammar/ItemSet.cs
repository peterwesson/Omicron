using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Omicron.Analysis.SyntaxAnalysis.SyntaxStacks;

namespace Omicron.Lanuage.Grammar
{
    public class ItemSet : IList<Item>
    {
        private readonly List<Item> _items;

        public Symbol TransitionSymbol { get; set; }

        public List<Transition> Transitions { get; set; }

        public List<Reduction> Reductions { get; set; }

        public ParserState State { get; set; }

        public int StateId { get; set; }

        public ItemSet() : this(new List<Item>())
        {
        }

        public ItemSet(List<Item> itemSet)
        {
            _items = itemSet;

            Transitions = new List<Transition>();
            Reductions = new List<Reduction>();
        }

        public IEnumerator<Item> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(Item item)
        {
            _items.Add(item);
        }

        public void Clear()
        {
            _items.Clear();
        }

        public bool Contains(Item item)
        {
            return _items.Contains(item);
        }

        public void CopyTo(Item[] array, int arrayIndex)
        {
            _items.CopyTo(array, arrayIndex);
        }

        public bool Remove(Item item)
        {
            return _items.Remove(item);
        }

        public int Count
        {
            get
            {
                return _items.Count;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public int IndexOf(Item item)
        {
            return _items.IndexOf(item);
        }

        public void Insert(int index, Item item)
        {
            _items.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            _items.RemoveAt(index);
        }

        public Item this[int index]
        {
            get
            {
                return _items[index];
            }
            set
            {
                _items[index] = value;
            }
        }

        public void AddRange(IEnumerable<Item> items)
        {
            _items.AddRange(items);
        }

        public override bool Equals(object obj)
        {
            var itemSet = obj as ItemSet;

            return itemSet != null ? Equals(itemSet) : base.Equals(obj);
        }

        protected bool Equals(ItemSet other)
        {
            return _items.SequenceEqual(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (this._items != null ? this._items.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}
