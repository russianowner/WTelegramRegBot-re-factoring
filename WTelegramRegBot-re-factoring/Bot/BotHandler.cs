using Common;
using Services;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Bot;

public class BotHandler
{
    private readonly ITelegramBotClient _bot;
    private readonly ProfileService _profiles;
    private readonly Logger _logger;
    private readonly Dictionary<long, UserSession> _sessions = new();
    public BotHandler(ITelegramBotClient bot, ProfileService profiles, Logger logger)
    {
        _bot = bot;
        _profiles = profiles;
        _logger = logger;
    }
    public async Task HandleUpdateAsync(ITelegramBotClient bot, Update update, CancellationToken token)
    {
        try
        {
            if (update.Type == UpdateType.Message && update.Message?.Text is { } text)
            {
                var chatId = update.Message.Chat.Id;
                if (!_sessions.ContainsKey(chatId))
                    _sessions[chatId] = new UserSession();
                var s = _sessions[chatId];
                if (text == "/start")
                {
                    s.Reset();
                    await ShowMainMenu(chatId, token);
                    return;
                }
                await s.ProcessInput(_bot, chatId, text, _profiles, token);
            }
            else if (update.Type == UpdateType.CallbackQuery)
            {
                await HandleCallback(update.CallbackQuery!, token);
            }
        }
        catch (Exception ex)
        {
            _logger.Error($"{ex}", ex);
        }
    }
    public Task HandleErrorAsync(ITelegramBotClient bot, Exception ex, CancellationToken token)
    {
        _logger.Error($"{ex}", ex);
        return Task.CompletedTask;
    }
    private async Task ShowMainMenu(long chatId, CancellationToken token)
    {
        var menu = new InlineKeyboardMarkup(new[]
        {
            new [] { InlineKeyboardButton.WithCallbackData("➕ Добавить профиль", "add_profile") },
            new [] { InlineKeyboardButton.WithCallbackData("📋 Список профилей", "list_profiles") },
        });
        await _bot.SendMessage(chatId, "😇 Главное меню:", replyMarkup: menu, cancellationToken: token);
    }
    private async Task HandleCallback(CallbackQuery cq, CancellationToken token)
    {
        var chatId = cq.From.Id;
        var data = cq.Data ?? "";
        if (!_sessions.ContainsKey(chatId))
            _sessions[chatId] = new UserSession();
        var s = _sessions[chatId];
        await _bot.AnswerCallbackQuery(cq.Id, cancellationToken: token);
        switch (data)
        {
            case "add_profile":
                s.Mode = UserMode.AddingPhone;
                await _bot.SendMessage(chatId, "📱 Введи номер телефона:", cancellationToken: token);
                break;
            case "list_profiles":
                var profiles = await _profiles.GetAllProfilesForDebugAsync();
                if (!profiles.Any())
                    await _bot.SendMessage(chatId, "😕 Нет профилей.", cancellationToken: token);
                else
                {
                    var lines = profiles.Select(p => $"{p.Id} | {p.Phone}");
                    await _bot.SendMessage(chatId, string.Join("\n", lines), cancellationToken: token);
                }
                break;
        }
    }
}
