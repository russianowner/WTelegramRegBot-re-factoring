namespace Common;

public class Config
{
    public string BotToken => Environment.GetEnvironmentVariable("Токен") ?? "Токен";
    public string DbConn => "Data Source=bot.db";
}
