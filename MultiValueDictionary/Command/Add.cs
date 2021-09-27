namespace MultiValueDictionary.Command
{
    public class Add : IStringCommand
    {
        public string Execute(IMultiValueDictionary<string, string> mvd, string[] args)
        {
            try
            {
                var member = args[2];
                var i = 0;
                while (mvd.MemberExists(args[1], member))
                {
                    i++;
                    member = args[2] + "i";
                }
                mvd.Add(args[i], member);
            }
            catch (MemberExistsException e)
            {
                return IStringCommand.FormatResult(e.Message);
            }

            return IStringCommand.FormatResult(Constants.Messages.Added);
        }
    }
}