using System.Linq;
using System.Text;

namespace MultiValueDictionary.Command
{
    public class Keys : IStringCommand
    {
        public string Execute(IMultiValueDictionary<string, string> mvd, string[] args)
        {
            var result = new StringBuilder();

            if (mvd.Keys.Count == 0)
                result.AppendLine($"{Constants.ResponseIndicator}{Constants.Messages.EmptySet}");

            foreach (var item in mvd.Keys
                .Select((key, idx) => new {key, idx}))
                result.AppendLine($"{item.idx + 1}{Constants.ResponseIndicator}{item.key}");

            return result.ToString();
        }
    }
}