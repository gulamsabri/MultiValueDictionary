using System.Collections.Generic;

namespace MultiValueDictionary.Command
{
    public class Remove : IStringCommand
    {
        public string Execute(IMultiValueDictionary<string, string> mvd, string[] args)
        {
            try
            {
                return mvd.Remove(args[1], args[2])
                    ? IStringCommand.FormatResult(Constants.Messages.Removed)
                    : IStringCommand.FormatResult(Constants.Messages.UnknownError);
            }
            catch (MemberDoesNotExistException e)
            {
                return IStringCommand.FormatResult(e.Message);
            }
            catch (KeyNotFoundException)
            {
                return IStringCommand.FormatResult(Constants.Messages.KeyNotFound);
            }
        }
    }
}