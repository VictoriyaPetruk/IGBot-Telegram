using Newtonsoft.Json;
using IGBot.Configuration;
using Microsoft.AspNetCore.Mvc.Formatters;
using System;

namespace IGBot.Clients;
public interface IInstagramClient
{
    Task<ResponseIG> GetInfo(string username, CancellationToken cancellationToken = default);
    Task<ResponseIG> GetPosts(string username, CancellationToken cancellationToken = default);
}
internal class InstagramClient : IInstagramClient
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerSettings _settings;
    private readonly BotConfiguration _botConfig;
    public InstagramClient(HttpClient httpClient, JsonSerializerSettings settings, BotConfiguration botConfiguration)
    {
        _httpClient = httpClient;
        _settings = settings;
        _botConfig = botConfiguration;
    }
    string filters = "username,website,name,ig_id,id,profile_picture_url,biography,follows_count,followers_count,media_count,media{id,caption,like_count,comments_count,timestamp,username,media_product_type,media_type,owner,permalink,media_url,children{media_url}}";
    public async Task<ResponseIG> GetInfo(string username, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetAsync($"/{_botConfig.BusinessAccount}?fields=business_discovery.username({username}){{username,website,name,ig_id,id,profile_picture_url,biography,follows_count,followers_count,media_count,media}}&access_token={_botConfig.AccessToken}");
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadAsStringAsync(cancellationToken);
        return JsonConvert.DeserializeObject<ResponseIG>(result, _settings);
    }
 
    public async Task<ResponseIG> GetPosts(string username, CancellationToken cancellationToken = default)
    {
        //{ id,caption,like_count,comments_count,timestamp,username,media_product_type,media_type,owner,permalink,media_url,children{ media_url} }
        string media = "media{id, caption, like_count, media_url}";
        var response = await _httpClient.GetAsync($"/{_botConfig.BusinessAccount}?fields=business_discovery.username({username}){{username,website,name,ig_id,id,profile_picture_url,biography,follows_count,followers_count,media_count,media{{id,like_count,media_type,media_url,caption}}}}&access_token={_botConfig.AccessToken}");
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadAsStringAsync(cancellationToken);
        //try
        //{
        //    var json = JsonConvert.DeserializeObject<ResponseIG>(result, _settings);
        //}
        //catch(Exception ex)
        //{
        //    throw new Exception("Can`t deserialize");
        //}

        return JsonConvert.DeserializeObject<ResponseIG>(result, _settings);
    }
}



/* 
1. get token 
2. get id of user by name 
3. get content by user id 
4. separate this content by template: photo, text
5. add get 10 latest posts 
6. button get more
by notofication and updates -> hook 

*/