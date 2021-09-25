using System.Linq;
using System.Text;

namespace MultiValueDictionary.Command
{
    public class AllMembers : IStringCommand
    {
        public string Execute(IMultiValueDictionary<string, string> mvd, string[] args)
        {
            var result = new StringBuilder();
            var members = mvd.AllMembers;

            if (members.Count == 0)
                result.AppendLine($"{Constants.ResponseIndicator}{Constants.Messages.EmptySet}");

            foreach (var item in members
                .Select((member, idx) => new {member, idx}))
                result.AppendLine($"{item.idx + 1}{Constants.ResponseIndicator}{item.member}");

            return result.ToString();
        }
    }
}