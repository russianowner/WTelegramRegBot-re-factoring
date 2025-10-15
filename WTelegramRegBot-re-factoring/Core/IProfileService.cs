namespace Core;

public interface IProfileService
{
    Task<Profile> CreateProfileAsync(string phone, string apiId, string apiHash, long ownerId);
    Task<(bool done, string next)> ContinueLoginAsync(Guid profileId, string input);
    Task<List<Profile>> GetAllProfilesForDebugAsync();
}
