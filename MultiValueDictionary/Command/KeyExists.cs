namespace MultiValueDictionary.Command
{
    public class KeyExists : IStringCommand
    {
        public string Execute(IMultiValueDictionary<string, string> mvd, string[] args)
        {
            return IStringCommand.FormatResult(mvd.KeyExists(args[1]).ToString());
        }
    }
}