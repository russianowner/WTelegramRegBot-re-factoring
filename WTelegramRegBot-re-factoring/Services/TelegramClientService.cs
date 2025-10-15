#nullable disable
using Core;
using WTelegram;

namespace Services;

public class TelegramClientService
{
    private readonly string _sessionsDir = "sessions";
    public TelegramClientService()
    {
        if (!Directory.Exists(_sessionsDir))
            Directory.CreateDirectory(_sessionsDir);
    }
    public Client CreateClient(Profile profile)
    {
        string Config(string what) => what switch
        {
            "api_id" => profile.ApiId,
            "api_hash" => profile.ApiHash,
            "phone_number" => profile.Phone,
            "session_pathname" => profile.SessionPath,
            _ => null
        };
        return new Client(Config);
    }
}
