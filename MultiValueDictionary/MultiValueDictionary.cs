using System.Collections.Generic;
using System.Linq;
using MultiValueDictionary.Exceptions;

namespace MultiValueDictionary
{
    public class MultiValueDictionary<TKey, TValues> : IMultiValueDictionary<TKey, TValues>
    {
        private readonly IDictionary<TKey, ICollection<TValues>> _dictionary;

        public MultiValueDictionary()
        {
            _dictionary = new Dictionary<TKey, ICollection<TValues>>();
        }

        public MultiValueDictionary(IDictionary<TKey, ICollection<TValues>> dictionary)
        {
            _dictionary = dictionary;
        }

        public IReadOnlyCollection<TValues> AllMembers => 
            (IReadOnlyCollection<TValues>) _dictionary
                .Values
                .SelectMany(v => v)
                .ToHashSet();
        
        public IReadOnlyDictionary<TKey, ICollection<TValues>> Items => 
            (IReadOnlyDictionary<TKey, ICollection<TValues>>) _dictionary;
        
        public IReadOnlyCollection<TKey> Keys => (IReadOnlyCollection<TKey>)_dictionary.Keys;

        public void Add(TKey key, TValues member)
        {
            if (_dictionary.ContainsKey(key))
            {
                if (_dictionary[key].Contains(member))
                    throw new MemberExistsException(Constants.Messages.MemberExists);
                _dictionary[key].Add(member);
            }
            else
                _dictionary.Add(key, new HashSet<TValues> {member});
        }

        public void Clear() => _dictionary.Clear();

        public bool KeyExists(TKey key) => _dictionary.ContainsKey(key);

        public IReadOnlyCollection<TValues> Members(TKey key) => 
            (IReadOnlyCollection<TValues>) _dictionary[key];

        public bool MemberExists(TKey key, TValues member) => _dictionary[key].Contains(member);

        public bool Remove(TKey key, TValues member) => _dictionary[key].Remove(member); 
        
        public bool RemoveAll(TKey key) => _dictionary.Remove(key);
    }
}