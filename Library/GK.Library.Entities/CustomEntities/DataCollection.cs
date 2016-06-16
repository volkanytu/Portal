using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace GK.Library.Entities.CustomEntities
{
    public class DataCollection<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>>, IEnumerable
    {
        private IDictionary<TKey, TValue> _innerDictionary = new Dictionary<TKey, TValue>();

        public int Count
        {
            get
            {
                return _innerDictionary.Count;
            }
        }

        public bool Contains(TKey key)
        {
            return _innerDictionary.ContainsKey(key);
        }

        public void Clear()
        {
            _innerDictionary.Clear();
        }

        public void Add(TKey key, TValue value)
        {
            _innerDictionary.Add(key, value);
        }

        public void Add(KeyValuePair<TKey, TValue> keyValue)
        {
            _innerDictionary.Add(keyValue);
        }

        public void AddRange(IEnumerable<KeyValuePair<TKey, TValue>> collection)
        {
            foreach (var item in collection)
            {
                _innerDictionary.Add(item);
            }
        }

        public TValue this[TKey key]
        {
            get
            {
                return _innerDictionary[key];
            }
            set
            {
                _innerDictionary[key] = value;

            }
        }

        public ICollection<TKey> Keys
        {
            get
            {
                return this._innerDictionary.Keys;
            }
        }

        public ICollection<TValue> Values
        {
            get
            {
                return this._innerDictionary.Values;
            }
        }

        public IEnumerator GetEnumerator()
        {
            return this._innerDictionary.GetEnumerator();
        }

        IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
        {
            return this._innerDictionary.GetEnumerator();
        }
    }
}
