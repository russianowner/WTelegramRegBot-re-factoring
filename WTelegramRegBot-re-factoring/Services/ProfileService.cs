using Common;
using Core;
using Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Concurrent;
using WTelegram;

namespace Services;

public class ProfileService : IProfileService
{
    private readonly BotDbContext _db;
    private readonly TelegramClientService _clientService;
    private readonly Logger _logger;
    private static readonly ConcurrentDictionary<Guid, Client> _activeClients = new();

    public ProfileService(BotDbContext db, Logger logger)
    {
        _db = db;
        _logger = logger;
        _clientService = new TelegramClientService();
    }
    public async Task<Profile> CreateProfileAsync(string phone, string apiId, string apiHash, long ownerId)
    {
        var profile = new Profile
        {
            Phone = phone,
            ApiId = apiId,
            ApiHash = apiHash,
            OwnerId = ownerId,
            SessionPath = Path.Combine("sessions", $"{Guid.NewGuid()}.session")
        };
        _db.Profiles.Add(profile);
        await _db.SaveChangesAsync();
        var client = new Client(cfg => cfg switch
        {
            "api_id" => apiId,
            "api_hash" => apiHash,
            "phone_number" => phone,
            "session_pathname" => profile.SessionPath,
            _ => null
        });
        _activeClients[profile.Id] = client;
        var res = await client.Login(phone);
        _logger.Info(res);
        return profile;
    }
    public async Task<(bool done, string next)> ContinueLoginAsync(Guid profileId, string input)
    {
        if (!_activeClients.TryGetValue(profileId, out var client))
            return (false, "😜Нет активной сессии");
        var res = await client.Login(input);
        switch (res)
        {
            case "verification_code": return (false, "🙂Введи код из Telegram");
            case "email": return (false, "🙂Введи почту от аккаунта");
            case "email_verification_code": return (false, "🙂Введи код из почты");
            case "password": return (false, "🙂Введи пароль 2FA");

            default:
                if (client.User != null)
                {
                    return (true, "👤Профиль успешно добавлен");
                }
                return (false, res);
        }
    }
    public async Task<List<Profile>> GetAllProfilesForDebugAsync()
    {
        return await _db.Profiles.AsNoTracking().ToListAsync();
    }
}
