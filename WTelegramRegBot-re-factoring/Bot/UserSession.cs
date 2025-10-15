using Services;
using Telegram.Bot;

namespace Bot;
public class UserSession
{
    public UserMode Mode { get; set; } = UserMode.None;
    public Dictionary<string, string> Temp { get; } = new();

    public void Reset()
    {
        Mode = UserMode.None;
        Temp.Clear();
    }
    public async Task ProcessInput(
        ITelegramBotClient bot, long chatId, string text,
        ProfileService ps, CancellationToken token)
    {
        switch (Mode)
        {
            case UserMode.AddingPhone:
                Temp["phone"] = text;
                Mode = UserMode.AddingApiId;
                await bot.SendMessage(chatId, "😌Введи API ID:", cancellationToken: token);
                break;
            case UserMode.AddingApiId:
                Temp["api_id"] = text;
                Mode = UserMode.AddingApiHash;
                await bot.SendMessage(chatId, "😌Введи API Hash:", cancellationToken: token);
                break;
            case UserMode.AddingApiHash:
                Temp["api_hash"] = text;
                var profile = await ps.CreateProfileAsync(Temp["phone"], Temp["api_id"], Temp["api_hash"], chatId);
                Temp["profileId"] = profile.Id.ToString();
                Mode = UserMode.AddingCode;
                await bot.SendMessage(chatId, "🔑 Код из телеги:", cancellationToken: token);
                break;
            case UserMode.AddingCode:
            case UserMode.AddingEmail:
            case UserMode.AddingEmailCode:
            case UserMode.AddingPassword:
                var result = await ps.ContinueLoginAsync(Guid.Parse(Temp["profileId"]), text);
                await bot.SendMessage(chatId, result.next, cancellationToken: token);
                if (result.done)
                    Reset();
                else if (result.next.Contains("email", StringComparison.OrdinalIgnoreCase))
                    Mode = UserMode.AddingEmail;
                else if (result.next.Contains("код", StringComparison.OrdinalIgnoreCase))
                    Mode = UserMode.AddingEmailCode;
                else if (result.next.Contains("пароль", StringComparison.OrdinalIgnoreCase))
                    Mode = UserMode.AddingPassword;
                break;
        }
    }
}
