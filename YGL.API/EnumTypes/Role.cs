using System.Collections.Generic;

namespace YGL.API.EnumTypes {
public static class Role {
    public const string User = "User";
    public const string Mod = "Mod";
    public const string Admin = "Admin";

    public const Roles Default = Roles.User;
    
    private static readonly Dictionary<short, string> RoleDict = new Dictionary<short, string>() {
        { (short)Roles.User, User },
        { (short)Roles.Mod, Mod },
        { (short)Roles.Admin, Admin },
    };

    public static short? GetRoleKeyOrNull(string providedValue) {
        foreach ((short key, string value) in RoleDict) {
            if (value == providedValue) return key;
        }

        return null;
    }

    public static short GetRoleKeyOrDefault(string providedValue) {
        foreach ((short key, string value) in RoleDict) {
            if (value == providedValue) return key;
        }

        return (short)Roles.User;
    }

    public static string GetRoleValueOrNull(short key) {
        return RoleDict.ContainsKey(key) ? RoleDict[key] : null;
    }

    public static string GetRoleValueOrDefault(short key) {
        return RoleDict.ContainsKey(key) ? RoleDict[key] : User;
    }
    
    public static bool DoesExists(short role) {
        return RoleDict.ContainsKey(role);
    }
}

public enum Roles {
    User = 0,
    Mod = 5,
    Admin = 9,
}
}