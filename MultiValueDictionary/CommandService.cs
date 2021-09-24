using System.Linq;
using System.Text;
using MultiValueDictionary.Exceptions;

namespace MultiValueDictionary
{
    public class CommandService<TKey, TMember> : ICommandService<TKey, TMember>
    {
        private readonly IMultiValueDictionary<TKey, TMember> _multiValueDictionary;

        public CommandService(IMultiValueDictionary<TKey, TMember> multiValueDictionary)
        {
            _multiValueDictionary = multiValueDictionary;
        }

        public string Add(TKey key, TMember member)
        {
            try
            {
                _multiValueDictionary.Add(key, member);
            }
            catch (MemberExistsException e)
            {
                return e.Message;
            }

            return Constants.Messages.Added;
        }

        public string Clear()
        {
            _multiValueDictionary.Clear();
            return Constants.Messages.Cleared;
        }

        public string Items()
        {
            var result = new StringBuilder();
            foreach (var item in _multiValueDictionary.Items
                .Select((kvp, idx) => new { kvp, idx }))
            {
                result.AppendLine($"{item.idx+1}) {item.kvp.Key}: {string.Join(" ", item.kvp.Value)}");
            }

            return result.ToString();
        }

        public string Keys()
        {
            var result = new StringBuilder();
            foreach (var item in _multiValueDictionary.Keys
                .Select((key, idx) => new { key, idx }))
            {
                result.AppendLine($"{item.idx+1}) {item.key}");
            }

            return result.ToString();
        }

        public string KeyExists(TKey key) => _multiValueDictionary.KeyExists(key).ToString();
        
    }
}