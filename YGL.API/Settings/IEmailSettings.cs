namespace YGL.API.Settings; 

public interface IEmailSettings {
    public string Name { get; set; }
    public string Address { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string Host { get; set; }
    public int Port { get; set; }
    public long UrlLifeTime { get; set; }
}