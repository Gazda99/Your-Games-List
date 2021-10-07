namespace YGL.API.Settings {
public class JwtSettings {
    public string Secret { get; set; }
    public long TokenLifeTime { get; set; }
    public long RefreshTokenLifeTime { get; set; }

    public string SecurityAlgorithm { get; set; }
}
}