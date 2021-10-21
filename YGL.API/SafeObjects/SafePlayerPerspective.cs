namespace YGL.API.SafeObjects {
public class SafePlayerPerspective {
    public int Id { get; set; }

    public string Name { get; set; }

    public SafePlayerPerspective(YGL.Model.PlayerPerspective playerPerspective) {
        this.Id = playerPerspective.Id;
        this.Name = playerPerspective.Name;
    }
}
}