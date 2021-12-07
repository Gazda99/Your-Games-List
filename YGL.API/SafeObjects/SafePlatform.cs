namespace YGL.API.SafeObjects; 

public class SafePlatform {
    public int Id { get; set; }

    public string Abbr { get; set; }

    public string Name { get; set; }

    public SafePlatform(YGL.Model.Platform platform) {
        this.Id = platform.Id;
        this.Abbr = platform.Abbr;
        this.Name = platform.Name;
    }
}