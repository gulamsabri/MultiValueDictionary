namespace MultiValueDictionary.Command
{
    public class Clear : IStringCommand
    {
        public string Execute(IMultiValueDictionary<string, string> mvd, string[] args)
        {
            mvd.Clear();
            return IStringCommand.FormatResult(Constants.Messages.Cleared);
        }
    }
}