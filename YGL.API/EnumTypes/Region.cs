using System.Collections.Generic;

namespace YGL.API.EnumTypes {
public class Region {
    public const string NotSpecified = "NotSpecified";
    public const string Europe = "Europe";
    public const string NorthAmerica = "North America";
    public const string Australia = "Australia";
    public const string NewZealand = "New Zealand";
    public const string Japan = "Japan";
    public const string China = "China";
    public const string Asia = "Asia";
    public const string WorldWide = "WorldWide";
    public const string Korea = "Korea";
    public const string Brazil = "Brazil";

    public const Regions Default = Regions.NotSpecified;

    private static readonly Dictionary<short, string> RegionDict = new Dictionary<short, string>() {
        { (short)Regions.NotSpecified, NotSpecified },
        { (short)Regions.Europe, Europe },
        { (short)Regions.NorthAmerica, NorthAmerica },
        { (short)Regions.Australia, Australia },
        { (short)Regions.NewZealand, NewZealand },
        { (short)Regions.Japan, Japan },
        { (short)Regions.China, China },
        { (short)Regions.Asia, Asia },
        { (short)Regions.WorldWide, WorldWide },
        { (short)Regions.Korea, Korea },
        { (short)Regions.Brazil, Brazil },
    };

    public static bool DoesExists(short region) {
        return RegionDict.ContainsKey(region);
    }
}

public enum Regions {
    NotSpecified = 0,
    Europe = 1,
    NorthAmerica = 2,
    Australia = 3,
    NewZealand = 4,
    Japan = 5,
    China = 6,
    Asia = 7,
    WorldWide = 8,
    Korea = 9,
    Brazil = 10,
}
}