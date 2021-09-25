namespace MultiValueDictionary.Command
{
    public interface IStringCommand
    {
        static string FormatResult(string str)
        {
            return $"{Constants.ResponseIndicator}{str}\n";
        }

        string Execute(IMultiValueDictionary<string, string> mvd, string[] args);
    }
}