using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MultiValueDictionary.Command
{
    public class Members : IStringCommand
    {
        public string Execute(IMultiValueDictionary<string, string> mvd, string[] args)
        {
            var result = new StringBuilder();

            try
            {
                foreach (var item in mvd.Members(args[1])
                    .Select((member, idx) => new {member, idx}))
                    result.AppendLine($"{item.idx + 1}{Constants.ResponseIndicator}{item.member}");
            }
            catch (KeyNotFoundException)
            {
                result.AppendLine($"{Constants.ResponseIndicator}{Constants.Messages.KeyNotFound}");
            }

            return result.ToString();
        }
    }
}