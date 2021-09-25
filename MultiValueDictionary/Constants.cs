namespace MultiValueDictionary
{
    public static class Constants
    {
        public const string Prompt = "> ";
        public const string InputSeparator = " ";
        public const string ResponseIndicator = ") ";

        public static class Commands
        {
            public const string Add = "ADD";
            public const string AllMembers = "ALLMEMBERS";
            public const string Clear = "CLEAR";
            public const string Items = "ITEMS";
            public const string KeyExists = "KEYEXISTS";
            public const string Keys = "KEYS";
            public const string MemberExists = "MEMBEREXISTS";
            public const string Members = "MEMBERS";
            public const string Remove = "REMOVE";
            public const string RemoveAll = "REMOVEALL";
            public const string Quit = "QUIT";
        }

        public static class Messages
        {
            public const string Added = "Added.";
            public const string Cleared = "Cleared.";
            public const string CommandNotRecognized = "Command not recognized.";
            public const string EmptySet = "Empty set.";
            public const string KeyNotFound = "ERROR, key does not exist.";
            public const string MemberDoesNotExist = "ERROR, member does not exist.";
            public const string MemberExists = "ERROR, member already exists for key.";
            public const string NotEnoughArguments = "ERROR, not enough arguments provided.";
            public const string Removed = "Removed.";
            public const string UnknownError = "Unknown error occurred.";
            public const string Welcome = "Welcome to MultiValue Dictionary.";
        }
    }
}