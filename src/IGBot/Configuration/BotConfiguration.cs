using System;
namespace IGBot.Configuration;

internal class BotConfiguration
{
    public Uri PirateApiBaseUri { get; set; }
    public Uri InstagramBaseAPIUri { get; set; }
    public string BotToken { get; init; }
    public string FaceBookAppSecret { get; set; }
    public string FaceBookAppId { get; set; }
    public string BusinessAccount { get; set; }
    public string PageID { get; set; }
    public string AccessToken { get; set; }
}