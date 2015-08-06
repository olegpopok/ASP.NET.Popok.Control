using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    public class HashTable<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>>
    {
        private const int DefaultCapasity = 3;
        private struct Entry
        {
            public TKey key;
            public TValue value;
            public int hashCode;
            public int next;
        }

        private int[] _buckets;
        private Entry[] _entries;
        private int _count;
        private int _freeList;
        private int _freeCount;
        private IEqualityComparer<TKey> _comparer;

        public HashTable() : this(0, null) { }

        public HashTable(int campasity) : this(campasity, null) { }

        public HashTable(int capasity, IEqualityComparer<TKey> comparer)
        {
            if(capasity < 0)
            {
                throw new ArgumentException();
            }
            Init(capasity);
            this._comparer = comparer ?? EqualityComparer<TKey>.Default;
        }

        public int Count()
        {
            return _count - _freeCount;
        }

        public void Add(TKey key, TValue value)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }

            int hash = _comparer.GetHashCode(key);
            int targetBucket = hash % _buckets.Length;
            int collisionCount = 0;
            for (int i = _buckets[targetBucket]; i >= 0; i = _entries[i].next)
            {
                if (hash == _entries[i].hashCode && _comparer.Equals(key, _entries[i].key))
                {
                    _entries[i].value = value;
                    return;
                }
                collisionCount++;
            }

            int index;
            if (_freeCount > 0)
            {
                index = _freeList;
                _freeList = _entries[index].next;
                _freeCount--;
            }
            else
            {
                if (_count == _entries.Length)
                {
                    Resize();
                    targetBucket = hash % _buckets.Length;
                }
                index = _count;
                _count++;
            }

            _entries[index].hashCode = hash;
            _entries[index].key = key;
            _entries[index].value = value;
            _entries[index].next = _buckets[targetBucket];
            _buckets[targetBucket] = index;

            if (collisionCount > HashTableHelper.MaxColisionCount)
            {
                Resize();
            }

        }
            
        public bool Remove(TKey key)
        {
            if(key == null)
            {
                throw new ArgumentNullException();
            }

            int hashCode = _comparer.GetHashCode(key);
            int targetBucket = hashCode % _buckets.Length;
            int last = -1;
            for(int i = _buckets[targetBucket]; i >=0 ; last = i, i = _entries[i].next)
            {
                if(hashCode == _entries[i].hashCode && _comparer.Equals(key, _entries[i].key))
                {
                    _freeCount++;
                    _entries[i].next = _freeList;
                    _freeList = i;
                    _entries[i].hashCode = -1;
                    _entries[i].key = default(TKey);
                    _entries[i].value = default(TValue);
                    return true;
                }
            }
            return false;
        }

        public TValue Get(TKey key)
        {
            if(key == null)
            {
                throw new ArgumentNullException();
            }

            int hashCode = _comparer.GetHashCode(key);
            int bucket = hashCode % _buckets.Length;
            for(int index = _buckets[bucket]; index >= 0; index = _entries[index].next)
            {
                if(hashCode == _entries[index].hashCode && _comparer.Equals(key, _entries[index].key))
                {
                    return _entries[index].value;
                }
            }
            throw new InvalidOperationException();
        }

        public IEnumerator<KeyValuePair<TKey,TValue>> GetEnumerator()
        {
            int bucket = 0;
            while(bucket < _buckets.Length)
            {
                for(int i = _buckets[bucket]; i >= 0; i = _entries[i].next)
                {
                    if (_entries[i].hashCode >= 0)
                    {
                        yield return new KeyValuePair<TKey, TValue>(_entries[i].key, _entries[i].value);
                    }
                }
                bucket++;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private void Init(int capasity)
        {

            int newCapasity = capasity == 0 ? DefaultCapasity : HashTableHelper.GetPrime(capasity);
            _buckets = new int[newCapasity];
            for(int index = 0; index < _buckets.Length; index++)
            {
                _buckets[index] = -1;
            }
            _entries = new Entry[newCapasity];
            _freeList = -1;
        }

        private void Resize()
        {
            int newSize = HashTableHelper.GetPrime(_buckets.Length*2);
            int[] newBuckets = new int[newSize];
            for (int i = 0; i < newBuckets.Length; i++)
            {
                newBuckets[i] = -1;
            }
            Entry[] newEntries = new Entry[newSize];
            Array.Copy(_entries, 0, newEntries, 0, _count);
            for (int i = 0; i < _count; i++)
            {
                if (newEntries[i].hashCode > 0)
                {
                    int bucket = newEntries[i].hashCode % newSize;
                    newEntries[i].next = newBuckets[bucket];
                    newBuckets[bucket] = i;
                }
            }
            _entries = newEntries;
            _buckets = newBuckets;
        }

        private static class HashTableHelper
        {
            public const int MaxColisionCount = 10;
            private static int[] _primes = { 3, 5, 7, 13, 17, 23, 29 }; // и т.д.

            public static int GetPrime(int min)
            {
                for(int i = 0; i < _primes.Length; i++)
                {
                    if(_primes[i] > min)
                    {
                        return _primes[i];
                    }
                }
                return min;
            }
        }
    }
}