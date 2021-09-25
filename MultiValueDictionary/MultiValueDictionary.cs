using System.Collections.Generic;
using System.Linq;

namespace MultiValueDictionary
{
    public class MultiValueDictionary<TKey, TValues> : IMultiValueDictionary<TKey, TValues>
    {
        private readonly IDictionary<TKey, ICollection<TValues>> _dictionary;

        public MultiValueDictionary(IDictionary<TKey, ICollection<TValues>> dictionary)
        {
            _dictionary = dictionary;
        }

        public IReadOnlyCollection<TValues> AllMembers =>
            _dictionary
                .Values
                .SelectMany(v => v)
                .ToList();

        public IReadOnlyDictionary<TKey, ICollection<TValues>> Items =>
            (IReadOnlyDictionary<TKey, ICollection<TValues>>) _dictionary;

        public IReadOnlyCollection<TKey> Keys => (IReadOnlyCollection<TKey>) _dictionary.Keys;

        public void Add(TKey key, TValues member)
        {
            if (_dictionary.ContainsKey(key))
            {
                if (_dictionary[key].Contains(member))
                    throw new MemberExistsException(Constants.Messages.MemberExists);
                _dictionary[key].Add(member);
            }
            else
            {
                _dictionary.Add(key, new HashSet<TValues> {member});
            }
        }

        public void Clear()
        {
            _dictionary.Clear();
        }

        public bool KeyExists(TKey key)
        {
            return _dictionary.ContainsKey(key);
        }

        public IReadOnlyCollection<TValues> Members(TKey key)
        {
            return (IReadOnlyCollection<TValues>) _dictionary[key];
        }

        public bool MemberExists(TKey key, TValues member)
        {
            return _dictionary[key].Contains(member);
        }

        public bool Remove(TKey key, TValues member)
        {
            if (_dictionary[key].Contains(member))
                return _dictionary[key].Count > 1 ? _dictionary[key].Remove(member) : _dictionary.Remove(key);
            throw new MemberDoesNotExistException(Constants.Messages.MemberDoesNotExist);
        }

        public bool RemoveAll(TKey key)
        {
            if (!_dictionary.Keys.Contains(key))
                throw new KeyNotFoundException();
            return _dictionary.Remove(key);
        }
    }
}