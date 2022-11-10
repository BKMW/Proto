using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

namespace Application.Tools
{
    public class TextOperations
    {

        #region MaskWallet
        public static string MaskWallet(string pan)
        {
            if (string.IsNullOrEmpty(pan)|| pan.Length != 19)
            {
                return "*******************";
            }
           
              return pan.Substring(0, 6) + "**********" + pan.Substring(16);
         
        }

        #endregion

        #region RandomString
        public static string RandomString(int length)
        {
            var random = new Random();
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(x => x[random.Next(x.Length)]).ToArray()) + Guid.NewGuid();
        }

        #endregion

        #region GenerateRefreshToken
        public static string GenerateRefreshToken()//x=32
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();

                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
        }
        #endregion

        #region UnixTimeStampToDateTime
        public static DateTime UnixTimeStampToDateTime(long unixTimeStamp)
        {
            var dateTimeVal = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTimeVal = dateTimeVal.AddSeconds(unixTimeStamp).ToUniversalTime();

            return dateTimeVal;
        }
        #endregion
    }
}
