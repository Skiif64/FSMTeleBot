using FSMTeleBot.Internal.Dispatcher;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace FSMTeleBot.Services;
internal class InternalUpdateHandler //TODO: Add interface, move to internal folder
{
    private readonly IBotDispatcher _dispatcher;

    public InternalUpdateHandler(IBotDispatcher dispatcher)
    {
        _dispatcher = dispatcher;
    }

    public async Task HandleUpdateAsync(Update update, CancellationToken cancellationToken = default)
    {
        var action = update.Type switch
        {
            UpdateType.CallbackQuery 
            => _dispatcher.SendAsync(update.CallbackQuery!, cancellationToken),
            UpdateType.ChannelPost 
            => _dispatcher.SendAsync(update.ChannelPost!, cancellationToken),
            UpdateType.ChatJoinRequest
            => _dispatcher.SendAsync(update.ChatJoinRequest!, cancellationToken),
            UpdateType.ChatMember 
            => _dispatcher.SendAsync(update.ChatMember!, cancellationToken),
            UpdateType.ChosenInlineResult 
            => _dispatcher.SendAsync(update.ChosenInlineResult!, cancellationToken),
            UpdateType.EditedChannelPost 
            => _dispatcher.SendAsync(update.EditedChannelPost!, cancellationToken),
            UpdateType.EditedMessage 
            => _dispatcher.SendAsync(update.EditedMessage!, cancellationToken),
            UpdateType.InlineQuery 
            => _dispatcher.SendAsync(update.InlineQuery!, cancellationToken),
            UpdateType.Message 
            => _dispatcher.SendAsync(update.Message!, cancellationToken),
            UpdateType.MyChatMember 
            => _dispatcher.SendAsync(update.MyChatMember!, cancellationToken),
            UpdateType.Poll 
            => _dispatcher.SendAsync(update.Poll!, cancellationToken),
            UpdateType.PollAnswer 
            => _dispatcher.SendAsync(update.PollAnswer!, cancellationToken),
            UpdateType.PreCheckoutQuery 
            => _dispatcher.SendAsync(update.PreCheckoutQuery!, cancellationToken),
            UpdateType.ShippingQuery 
            => _dispatcher.SendAsync(update.ShippingQuery!, cancellationToken),
            _ => throw new InvalidOperationException("Unknown update type")
        };
        await action;
    }
}
