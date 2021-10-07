using System.Collections.Generic;

namespace YGL.API.EnumTypes {
public static class Role {
    
    public const string User = "User";
    public const string Mod = "Mod";
    public const string Admin = "Admin";

    public const Roles Default = Roles.User;


    private static readonly Dictionary<int, string> RoleDict = new Dictionary<int, string>() {
        { (int)Roles.User, User },
        { (int)Roles.Mod, Mod },
        { (int)Roles.Admin, Admin },
    };

    public static int? GetRoleKeyOrNull(string providedValue) {
        foreach ((int key, string value) in RoleDict) {
            if (value == providedValue) return key;
        }

        return null;
    }

    public static int GetRoleKeyOrDefault(string providedValue) {
        foreach ((int key, string value) in RoleDict) {
            if (value == providedValue) return key;
        }

        return (int)Roles.User;
    }

    public static string GetRoleValueOrNull(int key) {
        return RoleDict.ContainsKey(key) ? RoleDict[key] : null;
    }

    public static string GetRoleValueOrDefault(int key) {
        return RoleDict.ContainsKey(key) ? RoleDict[key] : User;
    }

    public static Roles GetDefault() {
        return Roles.User;
    }
}

public enum Roles {
    User = 0,
    Mod = 5,
    Admin = 9,
}
}