using System.Collections.Generic;

namespace MultiValueDictionary.Command
{
    public class RemoveAll : IStringCommand
    {
        public string Execute(IMultiValueDictionary<string, string> mvd, string[] args)
        {
            try
            {
                mvd.RemoveAll(args[1]);
                return IStringCommand.FormatResult(Constants.Messages.Removed);
            }
            catch (KeyNotFoundException)
            {
                return IStringCommand.FormatResult(Constants.Messages.KeyNotFound);
            }
        }
    }
}