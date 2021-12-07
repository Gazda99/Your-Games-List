using System.Security.Cryptography;

namespace YGL.API.Helpers; 

public static class RngHelper {
    private static readonly RandomNumberGenerator Rng = RandomNumberGenerator.Create();

    public static byte[] GenerateRandomByteArray(int size) {
        var randomByteArray = new byte[size];
        Rng.GetBytes(randomByteArray);
        return randomByteArray;
    }
}