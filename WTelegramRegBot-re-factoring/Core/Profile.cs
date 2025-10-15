namespace Core;

public class Profile
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Phone { get; set; } = "";
    public string ApiId { get; set; } = "";
    public string ApiHash { get; set; } = "";
    public string SessionPath { get; set; } = "";
    public long OwnerId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
