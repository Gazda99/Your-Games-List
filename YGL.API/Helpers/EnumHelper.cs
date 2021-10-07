using System;

namespace YGL.API.Helpers {
public static class EnumHelper {
    public static int ToInt<TEnum>(this TEnum value) where TEnum : Enum {
        return (int)(object)value;
    }
}
}