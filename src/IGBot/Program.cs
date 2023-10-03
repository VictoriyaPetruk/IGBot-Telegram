using IGBot.Clients;
using IGBot.Configuration;
using IGBot.Services;
using Microsoft.Extensions.Options;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

using Telegram.Bot;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
    .AddControllers()
    .AddNewtonsoftJson();

// Add AWS Lambda support. When application is run in Lambda Kestrel is swapped out as the web server with Amazon.Lambda.AspNetCoreServer. This
// package will act as the webserver translating request and responses between the Lambda event source and ASP.NET Core.
builder.Services.AddAWSLambdaHosting(LambdaEventSource.RestApi);

builder.Services.Configure<BotConfiguration>(builder.Configuration);
builder.Services.AddTransient<IBotIGService, BotIGService>();

builder.Services
    .AddHttpClient("tgwebhook")
    .AddTypedClient<ITelegramBotClient>((client, sp) =>
    {
        var configuration = sp.GetRequiredService<IOptionsMonitor<BotConfiguration>>();
        return new TelegramBotClient(configuration.CurrentValue.BotToken, client);
    });

 builder.Services
    .AddHttpClient("Instagram API", (sp, client) =>
    {
        var configuration = sp.GetRequiredService<IOptionsMonitor<BotConfiguration>>();
        client.BaseAddress = configuration.CurrentValue.InstagramBaseAPIUri;
    })
    .AddTypedClient<IInstagramClient>((client, sp) =>
    {
        var contractResolver = new DefaultContractResolver
        {
            NamingStrategy = new CamelCaseNamingStrategy()
        };

        var settings = new JsonSerializerSettings
        {
            ContractResolver = contractResolver,
            Formatting = Formatting.None
        };

        var configuration = sp.GetRequiredService<IOptionsMonitor<BotConfiguration>>();
        var botConfig = new BotConfiguration()
        {
            PirateApiBaseUri = configuration.CurrentValue.PirateApiBaseUri,
            AccessToken = configuration.CurrentValue.AccessToken,
            PageID = configuration.CurrentValue.PageID,
            InstagramBaseAPIUri = configuration.CurrentValue.InstagramBaseAPIUri,
            BotToken = configuration.CurrentValue.BotToken,
            BusinessAccount = configuration.CurrentValue.BusinessAccount,
            FaceBookAppId = configuration.CurrentValue.FaceBookAppId,
            FaceBookAppSecret = configuration.CurrentValue.FaceBookAppSecret
        };
        return new InstagramClient(client, settings, botConfig);
    });

var app = builder.Build();

app.UseAuthorization();
app.MapControllers();

app.Run();