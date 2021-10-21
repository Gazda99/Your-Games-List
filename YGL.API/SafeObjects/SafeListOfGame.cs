namespace YGL.API.SafeObjects {
public class SafeListOfGame {
    public long Id { get; set; }
    public long OwnerId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public bool IsDefault { get; set; }
    public bool IsPublic { get; set; }
    

    public SafeListOfGame(YGL.Model.ListOfGame listOfGame) {
        this.Id = listOfGame.Id;
        this.OwnerId = listOfGame.OwnerId;
        this.Name = listOfGame.Name;
        this.Description = listOfGame.Description;
        this.IsDefault = listOfGame.IsDefault;
        this.IsPublic = listOfGame.IsPublic;
    }
}
}