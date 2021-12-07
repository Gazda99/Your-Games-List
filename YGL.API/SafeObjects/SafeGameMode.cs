namespace YGL.API.SafeObjects; 

public class SafeGameMode {
    public int Id { get; set; }
    public string Name { get; set; }

    public SafeGameMode(YGL.Model.GameMode gameMode) {
        this.Id = gameMode.Id;
        this.Name = gameMode.Name;
    }
}