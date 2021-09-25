using System.Linq;
using System.Text;

namespace MultiValueDictionary.Command
{
    public class Items : IStringCommand
    {
        public string Execute(IMultiValueDictionary<string, string> mvd, string[] args)
        {
            var result = new StringBuilder();

            if (mvd.Items.Count == 0)
                result.AppendLine($"{Constants.ResponseIndicator}{Constants.Messages.EmptySet}");

            foreach (var item in mvd.Items
                .Select((kvp, idx) => new {kvp, idx}))
                result.AppendLine(
                    $"{item.idx + 1}{Constants.ResponseIndicator}{item.kvp.Key}: {string.Join(" ", item.kvp.Value)}");

            return result.ToString();
        }
    }
}