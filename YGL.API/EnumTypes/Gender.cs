using System.Collections.Generic;

namespace YGL.API.EnumTypes {
public static class Gender {
    public const string NotSpecified = "Not Specified";
    public const string Male = "Male";
    public const string Female = "Female";
    public const string Other = "Other";

    public const Genders Default = Genders.NotSpecified;

    private static readonly Dictionary<byte, string> GenderDict = new Dictionary<byte, string>() {
        { (byte)Genders.NotSpecified, NotSpecified },
        { (byte)Genders.Male, Male },
        { (byte)Genders.Female, Female },
        { (byte)Genders.Other, Other },
    };

    public static bool DoesExists(byte gender) {
        return GenderDict.ContainsKey(gender);
    }
}

public enum Genders {
    NotSpecified = 0,
    Male = 1,
    Female = 2,
    Other = 3
}


}