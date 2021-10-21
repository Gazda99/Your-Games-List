using System;
using System.Collections.Generic;
using System.Linq;

namespace YGL.API.SafeObjects {
public class SafeGroup {
    public long Id { get; set; }
    public long CreatorId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Slug { get; set; }
    public short UsersAmount { get; set; }
    public DateTime CreatedAt { get; set; }

    public IEnumerable<long> UsersInGroup { get; set; }


    public SafeGroup(YGL.Model.Group group) {
        this.Id = group.Id;
        this.CreatorId = group.CreatorId;
        this.Name = group.Name;
        this.Description = group.Description;
        this.Slug = group.Slug;
        this.UsersAmount = group.UsersAmount;
        this.CreatedAt = group.CreatedAt;

        this.UsersInGroup = group.GroupHasUsers.Select(ghu => ghu.UserId);
    }
}
}