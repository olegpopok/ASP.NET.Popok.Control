using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    public class DoublyLinkedList<T> : IEnumerable<T>
    {
        private DoublyLinkedListNode<T> _head;
        private int _count;

        public void  Add(T value)
        {
            var node = new DoublyLinkedListNode<T>(value); 
            Add(node);
        }

        public void Add(DoublyLinkedListNode<T> node)
        {
            node.next = _head;
            if (_head != null)
            {
                _head.prev = node;
            }
            _head = node;
            node.prev = null;
            _count++;
        }

        public DoublyLinkedListNode<T> Find(T value)
        {
            var comparer = EqualityComparer<T>.Default;
            foreach(var node in this)
            {
                if (comparer.Equals(node.value, value)) 
                {
                    return node;
                }
            }
            return null;
        }

        public bool Remove(T value)
        {
            var node = Find(value);
            if(node == null)
            {
                return false;
            }
            Remove(node);
            return true;
        }

        public IEnumerator<DoublyLinkedListNode<T>> GetEnumerator()
        {
            var current = _head;
            while(current != null)
            {
                yield return current;
                current = current.next;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private void Remove(DoublyLinkedListNode<T> node)
        {
            if (node.prev != null)
            {
                node.prev.next = node.next;
            }
            else
            {
                _head = node.next;
            }
            if (node.next != null)
            {
                node.next.prev = node.prev;
            }
        }
    }

    public class DoublyLinkedListNode<T>
    {
        internal  DoublyLinkedListNode<T> next;
        internal  DoublyLinkedListNode<T> prev;
        internal T value;

        public DoublyLinkedListNode(T value)
        {
            this.value = value;
        }

        public DoublyLinkedListNode<T> Next
        {
            get { return next; }
        }

        public DoublyLinkedListNode<T> Previous
        {
            get { return prev; }
        }

        public T Value
        {
            get { return this.value; }
            set { this.value = value; }
        }

        internal void Invalidate()
        {
            next = null;
            prev = null;
            value = default(T);
        }   
    }
}
