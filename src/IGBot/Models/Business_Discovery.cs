namespace IGBot.Clients;

public class Business_Discovery
{
    public string username { get; set; } 
    
    public string website { get; set; }
    public string name { get; set; } 
    public string ig_id { get; set; } 
    public string id { get; set; }
    public string profile_picture_url { get; set; }
    public string biography { get; set; }
    public int follows_count { get; set; } 
    public int followers_count { get; set; }
    public int media_count { get; set; }
    public Media media { get; set; }
}
