namespace IGBot.Configuration
{
    public static class Constants
    {
        public const string WelcomeMessage = "Welcome to ConnectIG Bot! Let`s start!";
        public const string ErrorUsingBot = "Cannot use it right now, please try later";
        public const string ErrorEmptyContent = "Instagram content is null or empty";

        public const string FunctionalDesc = "ConnectIG Bot - follow IG from Telegram." +
            "Get information about public accounts, their posts, followers count, " +
            "-choose accounts you want to get updates here and save it"+
            "-good quality of photos" +
            "Don`t lose actual info from IG without using it." +
            "Your lates news and updates from IG." +
            "Commands:" +
            "/start" +
            "@user - get a full information about account" +
            "/savedusers"+
            "/posts/@user - get recent posts from the account" +
            "/language" +
            "Premium functions:" +
            "-get users from not bussiness type of accounts" +
            "-get stories" +
            "-get regular update from your saved accounts" +
            "-get stories info";

        public const string GetPostsReply = "Write the account name";
    }
}
