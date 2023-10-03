namespace IGBot.Configuration
{
    public static class Commands
    {
        public static Dictionary<string, string> commands = new Dictionary<string, string>()
        {
            { "saved", "Saved accounts" },
            { "getinfo", "General account information" },
            { "getpost", "Recent account posts" },
            { "language", "Language settings" },
            { "addaccount", "Add account for quick access" }
        };
    }
}
