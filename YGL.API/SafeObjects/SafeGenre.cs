namespace YGL.API.SafeObjects {
public class SafeGenre {
    public int Id { get; set; }

    public string Name { get; set; }

    public SafeGenre(YGL.Model.Genre genre) {
        this.Id = genre.Id;
        this.Name = genre.Name;
    }
}
}