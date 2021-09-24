using System.Collections;
using System.Collections.Generic;

namespace MultiValueDictionary
{
    public interface IMultiValueDictionary<TKey, TMember>
    {
        IReadOnlyCollection<TMember> AllMembers { get; }
        IReadOnlyDictionary<TKey, ICollection<TMember>> Items { get; }
        IReadOnlyCollection<TKey> Keys { get; }
        
        void Add(TKey key, TMember member);

        void Clear();

        bool KeyExists(TKey key);

        IReadOnlyCollection<TMember> Members(TKey key);

        bool MemberExists(TKey key, TMember member);

        bool Remove(TKey key, TMember member);

        bool RemoveAll(TKey key);
    }
}