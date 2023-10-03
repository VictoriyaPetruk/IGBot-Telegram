using IGBot.Clients;
using IGBot.Models;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using IGBot.Configuration;
using System.Text.RegularExpressions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.ReplyMarkups;
using Newtonsoft.Json;
using System;

namespace IGBot.Services;

internal class BotIGService : IBotIGService
{
    private readonly ITelegramBotClient _botClient;
    private readonly IInstagramClient _instagramClient;
    private readonly ILogger<BotIGService> _logger;

    public BotIGService(ITelegramBotClient botClient, IInstagramClient instagramClient,
        ILogger<BotIGService> logger)
    {
        _botClient = botClient;
        _instagramClient = instagramClient;
        _logger = logger;
    }

    public async Task HandleUpdate(Update update, CancellationToken cancellationToken = default)
    {
        var message = update?.Message?.Text;
        if (string.IsNullOrEmpty(message))
        {
            return;
        }
        var chatId = update.Message.Chat.Id;

        try
        {
            if (message.Equals("/start") == true)
            {
                //await InitializeMenuCommands();

                await _botClient.SendTextMessageAsync(update.Message.Chat.Id,
                    Constants.WelcomeMessage,
                    cancellationToken: cancellationToken);
                return;
            }

            if (update.Message.ReplyToMessage != null) //check name?????
            {
                if (update.Message.ReplyToMessage.Text == Constants.GetPostsReply)
                {
                    var response = await _instagramClient.GetPosts(message, cancellationToken);
                    var business = response.business_discovery;
                    //var result = Template.GetInfoTemplate(business.username,
                    //   business.website, business.name, business.profile_picture_url, business.biography, business.follows_count,
                    //   business.followers_count, business.media_count);
                    var result1 = Template.GetPostTemplate(business.media.data[0], 1);
                    
                    await _botClient.SendTextMessageAsync(update.Message.Chat.Id,
                        result1, ParseMode.Html,
                        replyToMessageId: update.Message.MessageId,
                        cancellationToken: cancellationToken);
                }
                else
                {
                    await _botClient.SendTextMessageAsync(update.Message.Chat.Id,
                     "Can`t recognize your reply:). Write the exact command.",
                     allowSendingWithoutReply: false,
                     replyToMessageId: update.Message.MessageId,
                     cancellationToken: cancellationToken);
                }
                
            }

            if (message.Equals("/getposts") == true)
            {
                await _botClient.SendTextMessageAsync(update.Message.Chat.Id,
                    Constants.GetPostsReply,
                    replyToMessageId: update.Message.MessageId,
                    cancellationToken: cancellationToken,
                    replyMarkup: (new ForceReplyMarkup { Selective = true })
                    );

            }

            if (message.Equals("/getinfo") == true)
            {
                await _botClient.SendTextMessageAsync(update.Message.Chat.Id,
                   "Please send the name of account starts with @",
                   replyToMessageId: update.Message.MessageId,
                   cancellationToken: cancellationToken);
            }

            if (message.Equals("/language") == true)
            {
                await _botClient.SendTextMessageAsync(update.Message.Chat.Id,
                   "Developing is hard, we are working on this:)",
                   replyToMessageId: update.Message.MessageId,
                   cancellationToken: cancellationToken
                   );
            }
            if (message.Equals("/saved") == true)
            { 
                await _botClient.SendTextMessageAsync(update.Message.Chat.Id,
                   "Developing is hard, we are working on this:)",
                   replyToMessageId: update.Message.MessageId,
                   cancellationToken: cancellationToken
                   );
            }
            try
            {
                Regex regex = new Regex("^@");
                var matches = regex.Matches(message);
                if (matches.Count > 0)
                {
                    var response = await _instagramClient.GetInfo(message.TrimStart('@'), cancellationToken);

                    Business_Discovery business = response.business_discovery;
                    var result = Template.GetInfoTemplate(business.username,
                        business.website, business.name, business.profile_picture_url, business.biography, business.follows_count,
                        business.followers_count, business.media_count);

                    if (string.IsNullOrEmpty(result))
                    {
                        throw new Exception(Constants.ErrorEmptyContent);
                    }
                    await _botClient.SendTextMessageAsync(update.Message.Chat.Id,
                        result, ParseMode.Html,
                        replyToMessageId: update.Message.MessageId,
                        cancellationToken: cancellationToken);
                }
                //else
                //{
                //    await _botClient.SendTextMessageAsync(update.Message.Chat.Id,
                //        "Please send the name of account starts with @",
                //        replyToMessageId: update.Message.MessageId,
                //        cancellationToken: cancellationToken);
                //}
            }
            catch (Exception)
            {
                await _botClient.SendTextMessageAsync(update.Message.Chat.Id,
                    Constants.ErrorUsingBot,
                    replyToMessageId: update.Message.MessageId,
                    cancellationToken: cancellationToken);

                throw;
            }
        }
        catch (Exception exc)
        {
            _logger.LogError(exc, "Unknown error", new
            {
                Message = message
            });
        }
    }

    public async Task InitializeMenuCommands()
    {
        List<BotCommand> commandsList = new List<BotCommand>();
        foreach (var command in Commands.commands)
        {
            BotCommand botCommand = new BotCommand();
            botCommand.Command = command.Key;
            botCommand.Description = command.Value;
            commandsList.Add(botCommand);
        }

        await _botClient.SetMyCommandsAsync(commandsList);
    }



    //var media = new List<IAlbumInputMedia>()
    //{
    //   new InputMediaPhoto(business.media.data[0].media_url),
    //   new InputMediaPhoto(business.media.data[1].media_url),
    //   new InputMediaPhoto(business.media.data[2].media_url),
    //   new InputMediaPhoto(business.media.data[3].media_url)
    //};
    //await _botClient.SendMediaGroupAsync(update.Message.Chat.Id,
    //    media,
    //    replyToMessageId: update.Message.MessageId,
    //    cancellationToken: cancellationToken);
    //var result2 = Template.GetPostTemplate(business.media.data[1]);
    //var r = JsonConvert.SerializeObject(business);

    //InlineKeyboardButton urlButton = new InlineKeyboardButton("1 post");
    //InlineKeyboardButton urlButton2 = new InlineKeyboardButton("2 post");
    //InlineKeyboardButton urlButton3 = new InlineKeyboardButton("3 post");

    //urlButton.Url = "https://www.google.com/";

    //urlButton2.Url = "https://www.bing.com/";

    //urlButton3.Url = "https://www.duckduckgo.com/";

    // Rows, every row is InlineKeyboardButton[], You can put multiple buttons!
    //InlineKeyboardButton[] row1 = new InlineKeyboardButton[] { urlButton };
    //InlineKeyboardButton[] row2 = new InlineKeyboardButton[] { urlButton2, urlButton3 };

    //var inlineKeyboard = new InlineKeyboardMarkup(new[]
    //{
    //    InlineKeyboardButton.WithUrl("Image 1", business.media.data[0].media_url),
    //    InlineKeyboardButton.WithUrl("Image 2", business.media.data[1].media_url)
    //});

    //// Buttons by rows
    //InlineKeyboardButton[][] buttons = new InlineKeyboardButton[][] { row1, row2 };
    //InlineKeyboardMarkup keyboard = new InlineKeyboardMarkup(new InlineKeyboardButton[] { urlButton, urlButton2 });
    //await _botClient.SendTextMessageAsync(update.Message.Chat.Id, business.media.data[0].caption, 
    //    ParseMode.Markdown, cancellationToken: cancellationToken, replyMarkup: inlineKeyboard);




    //var pinnedmsgtxt = update.Message.PinnedMessage;
    //if (pinnedmsgtxt != null)
    //{
    //    await _botClient.SendTextMessageAsync(update.Message.Chat.Id,
    //    pinnedmsgtxt.Text,
    //    replyToMessageId: update.Message.MessageId,
    //    cancellationToken: cancellationToken,
    //    replyMarkup: (new ForceReplyMarkup { Selective = true })
    //    );
    //}

}
