namespace YGL.API.Contracts.V1.Requests.Identity {
public class UpdateUserReq {
    //  public string Username { get; set; }
    public byte Gender { get; set; }
    public short BirthYear { get; set; }
    public short Country { get; set; }
    public string About { get; set; }

    public string Slug { get; set; }
}
}