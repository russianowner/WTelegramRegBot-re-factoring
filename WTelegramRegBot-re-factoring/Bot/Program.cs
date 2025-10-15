using Bot;
using Data;
using Services;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Common;

var config = new Config();
var logger = new Logger();
var db = new BotDbContext(config.DbConn);
var profileService = new ProfileService(db, logger);
var bot = new TelegramBotClient(config.BotToken);
var handler = new BotHandler(bot, profileService, logger);
using var cts = new CancellationTokenSource();
bot.StartReceiving(
    handler.HandleUpdateAsync,
    handler.HandleErrorAsync,
    new ReceiverOptions(),
    cts.Token);
Console.WriteLine("Бот запущен");
Console.ReadLine();
cts.Cancel();
