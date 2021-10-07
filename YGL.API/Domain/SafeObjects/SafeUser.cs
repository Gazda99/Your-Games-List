using System;
using System.Collections.Generic;
using YGL.Model;

namespace YGL.API.Domain.SafeObjects {
public class SafeUser {
    public long Id { get; set; }
    public string Username { get; set; }
    public byte Gender { get; set; }
    public short BirthYear { get; set; }
    public short Country { get; set; }
    public DateTime CreatedAt { get; set; }
    public string About { get; set; }
    public short Rank { get; set; }
    public int Experience { get; set; }

    public IEnumerable<ListOfGame> ListOfGames { get; set; }
    public IEnumerable<Group> Groups { get; set; }


    public SafeUser(YGL.Model.User user) {
        this.Id = user.Id;
        this.Username = user.Username;
        this.Gender = user.Gender;
        this.BirthYear = user.BirthYear;
        this.Country = user.Country;
        this.CreatedAt = user.CreatedAt;
        this.About = user.About;
        this.Rank = user.Rank;
        this.Experience = user.Experience;
        this.ListOfGames = user.ListOfGames;
        this.Groups = user.Groups;
    }
}
}