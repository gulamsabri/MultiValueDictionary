using System.Collections.Generic;

namespace MultiValueDictionary.Command
{
    public class MemberExists : IStringCommand
    {
        public string Execute(IMultiValueDictionary<string, string> mvd, string[] args)
        {
            try
            {
                return IStringCommand.FormatResult(mvd.MemberExists(args[1], args[2]).ToString());
            }
            catch (KeyNotFoundException)
            {
                return IStringCommand.FormatResult(false.ToString());
            }
        }
    }
}