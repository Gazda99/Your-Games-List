using System;

namespace YGL.API.SafeObjects; 

public class SafeFriend {
    public long Id { get; set; }
    public long FriendOneId { get; set; }
    public long FriendTwoId { get; set; }
    public DateTime Since { get; set; }

    public SafeFriend(YGL.Model.Friend friend) {
        this.Id = friend.Id;
        this.FriendOneId = friend.FriendOneId;
        this.FriendTwoId = friend.FriendTwoId;
        this.Since = friend.Since;
    }
}