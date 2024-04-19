using System.Security.Cryptography;

namespace OurBeautyReferralNetwork.Utilities
{
    public class JWTUtilities
    {
        public static string GenerateRandomKey(int keyLength)
        {
            var rng = new RNGCryptoServiceProvider();
            var keyBytes = new byte[keyLength / 8];
            rng.GetBytes(keyBytes);
            return Convert.ToBase64String(keyBytes);
        }
    }
}
