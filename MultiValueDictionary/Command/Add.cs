namespace MultiValueDictionary.Command
{
    public class Add : IStringCommand
    {
        public string Execute(IMultiValueDictionary<string, string> mvd, string[] args)
        {
            try
            {
                mvd.Add(args[1], args[2]);
            }
            catch (MemberExistsException e)
            {
                return IStringCommand.FormatResult(e.Message);
            }

            return IStringCommand.FormatResult(Constants.Messages.Added);
        }
    }
}