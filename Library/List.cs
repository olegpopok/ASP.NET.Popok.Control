using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    public class List<T> : IEnumerable<T>
    {
        private const int DefaultCapasity = 4; 
        private T[] _items;
        private int _count;
        private IEqualityComparer<T> comparer;

        public List() : this(0) { }

        public List(int capasity)
        {
            _items = new T[capasity];
            comparer = EqualityComparer<T>.Default;
        }

        public int Capasity
        {
            get
            {
                return _items.Length;
            }
            set
            {
                if (value < _items.Length)
                {
                    throw new ArgumentOutOfRangeException();
                }
                int newCapasity = value;
                if(newCapasity == 0)
                {
                    newCapasity = DefaultCapasity;
                }
                T[] newItems = new T[newCapasity];
                Array.Copy(_items, 0, newItems, 0, _count);
                _items = newItems;
            }
        }

        public int Count
        {
            get { return _count; }
        }

        public void Add(T item)
        {
            if(_count == _items.Length)
            {
                int newCapasity = _items.Length * 2;
                Capasity = newCapasity;
            }
            _items[_count] = item;
            _count++;
        }

        public IEnumerator<T> GetEnumerator() 
        {
            for(int index =0; index < _count; index++)
            {
                yield return _items[index];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public bool Remove(T item)
        {
            for(int index = 0; index < _count; index++)
            {
                if(comparer.Equals(_items[index], item))
                {
                    Array.Copy(_items, index+1, _items, index, _count - index - 1);
                    _items[_count] = default(T);
                    _count--;
                    return true;
                }
            }
            return false;
        }
    }
}
