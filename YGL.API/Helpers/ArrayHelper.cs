namespace YGL.API.Helpers {
public static class ArrayHelper {
    public static bool CompareTwoByteArrays(byte[] array1, byte[] array2) {
        if (array1.Length != array2.Length)
            return false;

        for (int i = 0; i < array1.Length; i++) {
            if (array1[i] != array2[i])
                return false;
        }

        return true;
    }
}
}