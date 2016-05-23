using System.Collections.Generic;
using System.Linq;

namespace Omicron.Analysis.SyntaxAnalysis.SyntaxStacks
{
    public class SyntaxStack
    {
        private readonly Stack<SyntaxStackItem> _stack;

        public SyntaxStack()
        {
            _stack = new Stack<SyntaxStackItem>();
        }

        public SyntaxStackItem Pop()
        {
            return _stack.Pop();
        }

        public SyntaxStackItem Peek()
        {
            return _stack.Any() ? _stack.Peek() : null;
        }

        public IEnumerable<SyntaxStackItem> Pop(int count)
        {
            for (var i = 0; i < count; i++)
            {
                yield return _stack.Pop();
            }
        }

        public void Push(SyntaxStackItem item)
        {
            _stack.Push(item);
        }

        public List<SyntaxStackItem> ToList()
        {
            return _stack.ToList();
        }

        public bool Empty()
        {
            return !_stack.Any();
        }
    }
}
