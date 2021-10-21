using System;
using System.Collections.Generic;
using System.Linq;

namespace YGL.API.SafeObjects {
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

    public bool ItemStatus { get; set; }

    public IEnumerable<long> ListOfGames { get; set; }
    public IEnumerable<long> Groups { get; set; }
    public IEnumerable<long> Friends { get; set; }


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
        this.ItemStatus = (bool)user.ItemStatus;

        this.ListOfGames = user.ListOfGames.Select(l => l.Id);
        this.Groups = user.Groups.Select(l => l.Id);
        
        this.Friends = user.FriendFriendOnes
            .Concat(user.FriendFriendTwos)
            .Select(friend => friend.FriendOneId != this.Id ? friend.FriendOneId : friend.FriendTwoId);
    }
}
}