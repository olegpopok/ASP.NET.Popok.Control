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
            Initialize(capasity);
            this._comparer = comparer ?? EqualityComparer<TKey>.Default;
        }

        public int Count()
        {
            return _count - _freeCount ;
        }

        public void Add(TKey key, TValue value)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }

            int hash = _comparer.GetHashCode(key) & 0x7FFFFFFF;
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

            int hashCode = _comparer.GetHashCode(key) & 0x7FFFFFFF;
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
            int hashCode = _comparer.GetHashCode(key) & 0x7FFFFFFF;
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

        private void Initialize(int capasity)
        {
            int newCapasity = capasity == 0 ? DefaultCapasity : HashTableHelper.GetPrime(capasity);
            _buckets = new int[newCapasity];
            DefaultInitBuckets(_buckets);
            _entries = new Entry[newCapasity];
            _freeList = -1;
        }

        private void Resize()
        {
            int newSize = HashTableHelper.GetPrime(_buckets.Length*2);
            int[] newBuckets = new int[newSize];
            DefaultInitBuckets(newBuckets);
            Entry[] newEntries = new Entry[newSize];
            Array.Copy(_entries, 0, newEntries, 0, _count);

            for (int i = 0; i < _count; i++)
            {
                if (newEntries[i].hashCode >= 0)
                {
                    int bucket = newEntries[i].hashCode % newSize;
                    newEntries[i].next = newBuckets[bucket];
                    newBuckets[bucket] = i;
                }
            }
            _entries = newEntries;
            _buckets = newBuckets;
        }

        private void DefaultInitBuckets(int[] buckets)
        {
            for(int index = 0; index < buckets.Length; index++)
            {
                buckets[index] = -1;
            }
        }

        private static class HashTableHelper
        {
            public const int MaxColisionCount = 50;
            public static readonly int[] _primes = {
            3, 7, 11, 17, 23, 29, 37, 47, 59, 71, 89, 107, 131, 163, 197, 239, 293, 353, 431, 521, 631, 761, 919,
            1103, 1327, 1597, 1931, 2333, 2801, 3371, 4049, 4861, 5839, 7013, 8419, 10103, 12143, 14591,
            17519, 21023, 25229, 30293, 36353, 43627, 52361, 62851, 75431, 90523, 108631, 130363, 156437,
            187751, 225307, 270371, 324449, 389357, 467237, 560689, 672827, 807403, 968897, 1162687, 1395263,
            1674319, 2009191, 2411033, 2893249, 3471899, 4166287, 4999559, 5999471, 7199369};

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