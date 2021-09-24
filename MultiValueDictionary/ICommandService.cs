namespace MultiValueDictionary
{
    public interface ICommandService<TKey, TMember>
    {
        string Add(TKey key, TMember member);
        string Clear();
        string Items();
        string Keys();
        string KeyExists(TKey key);
    }
}