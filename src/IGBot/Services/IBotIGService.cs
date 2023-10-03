using Telegram.Bot.Types;

namespace IGBot.Services;

public interface IBotIGService
{
    Task HandleUpdate(Update update, CancellationToken cancellationToken = default);
}
