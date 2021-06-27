using System;
using System.Security.Cryptography;

namespace Core.Tools
{
    public class RSACrypt
    {
        #region Decrypt
        public static string Decrypt(string text)
        {
            try
            {
                const int PROVIDER_RSA_FULL = 1;
                const string CONTAINER_NAME = "Tracker";

                CspParameters cspParams;
                cspParams = new CspParameters(PROVIDER_RSA_FULL);
                cspParams.KeyContainerName = CONTAINER_NAME;
                RSACryptoServiceProvider rsa1 = new RSACryptoServiceProvider(cspParams);
                rsa1.FromXmlString("<RSAKeyValue><Modulus>2rRVVVFJRbH/wAPDtnwZwu+nxU+AZ6uXxh/sW+AMCBogg7vndZsnRiHoLttYYPqOyOhfgaBOQogrIfrKL4lipK4m52SBzw/FfcM9DsKs/rYR83tBLiIAfgdnVjF27tZID+HJMFTiI30mALjr7+tfp+2lIACXA1RIKTk7S9pDmX8=</Modulus><Exponent>AQAB</Exponent><P>92jJJyzFBSx6gL4Y1YpALmc5CNjoE/wETjqb3ci2v0+3rZWvJKmKy1ZEdlXpyuvXVksJ6cMdUpNAkMknUk9pTQ==</P><Q>4kxkABZOXyDLryYGCGY0b8N0FIdu5BTCFDYEdcatxl/f7ZGDS1NgHJpUWxkVXFfHy2Y/GuDOIbpcwlsO739H+w==</Q><DP>5bNFvrdUHF+VRN45VFjNCcgQLeSkY5mBrdfASoNFGA29LM5iE5nNIMfxPCS7sQiRnq6Af6YFHVtVgJchiMvtqQ==</DP><DQ>j+ng1qVY5epnXlWiFIla45C7K6sNfIMvAcdwgq39KWEjeWPGyYqWXtpOtzh2eylf6Bx4GVHKBW0NPJTIJMsfLQ==</DQ><InverseQ>8uu0dfPVDqB2qFM1Vdi8hl+2uZtN7gjT2co1cEWy29HVYBZD0k9KKCf2PbkeuSfpgFpE70wW5Hrp8V7l/SwSOw==</InverseQ><D>MM/c18zroJ2Iqi9s5/asvUBF3pjO3NSEbFjFpP/NT6WdKimvECWPz2xT6NlV0Vc6tQaAAmtn7Bt+HPhfVdrA4/ysYVe3/6TWkPjW+bvAhMWu/ZqISx11/jPYSGD9g3ZXgUiqcQM8UbOjlswoq4fpheEXTB0xdVutDLpO3qgHN6k=</D></RSAKeyValue>");

                //    string data2Decrypt = "kZBXGnNgSUj5nbJF9Q44Ik73aGbYj88Lxj2lKeG2+Y9IqCLl5wPVpDnF40y4lN/Ef/aZJGC/phnYEZLMwY6wPukpxNGVczVQEaZuLbkesEeTbZ+lGTiHE2J7rKYPwElvmLv2SHVdWDMa/NAiCOIX/gsnB8ofHZFiIUdOqCkuqYo=";

                byte[] encyrptedBytes = Convert.FromBase64String(text);

                byte[] plain = rsa1.Decrypt(encyrptedBytes, false);
                string decryptedString = System.Text.Encoding.UTF8.GetString(plain);
                return decryptedString;
            }
            catch
            {
                return "";

            }

        }
        #endregion

        #region Encrypt
        public static string Encrypt(string text)
        {
            const int PROVIDER_RSA_FULL = 1;
            const string CONTAINER_NAME = "Tracker";

            CspParameters cspParams;
            cspParams = new CspParameters(PROVIDER_RSA_FULL);
            cspParams.KeyContainerName = CONTAINER_NAME;
            RSACryptoServiceProvider rsa1 = new RSACryptoServiceProvider(cspParams);
            rsa1.FromXmlString("<RSAKeyValue><Modulus>2rRVVVFJRbH/wAPDtnwZwu+nxU+AZ6uXxh/sW+AMCBogg7vndZsnRiHoLttYYPqOyOhfgaBOQogrIfrKL4lipK4m52SBzw/FfcM9DsKs/rYR83tBLiIAfgdnVjF27tZID+HJMFTiI30mALjr7+tfp+2lIACXA1RIKTk7S9pDmX8=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>");

            System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
            byte[] textBytes = encoding.GetBytes(text);
            byte[] encryptedOutput = rsa1.Encrypt(textBytes, false);
            string outputB64 = Convert.ToBase64String(encryptedOutput);
            return outputB64;
        }

        #endregion

        #region DecryptQrCode
        public string DecryptQrCode(string text)
        {
            const int PROVIDER_RSA_FULL = 1;
            const string CONTAINER_NAME = "Tracker";

            CspParameters cspParams;
            cspParams = new CspParameters(PROVIDER_RSA_FULL);
            cspParams.KeyContainerName = CONTAINER_NAME;
            RSACryptoServiceProvider rsa1 = new RSACryptoServiceProvider(cspParams);
            rsa1.FromXmlString("<RSAKeyValue><Modulus>VyCgyOTW8mXf3ebTunY0r7fBg3O7KMjWqkqmav8PQP88YmUwNLsJ72L/RPjlykvcozwqqbvXdRC88UMVPSkRQw==</Modulus><Exponent>AQAB</Exponent><P>qATisHlaELs6QZNn75HE7WjB3gzzMDFiVBA0ld41KKs=</P><Q>hMAk7HYgrLvYl+Bn8dGRa/hl4J5UESkqlIBKwkM6ack=</Q><DP>CBn0ND4lsZjVfkP1Rv/oVuouLt7A+xnme9csMe288nE=</DP><DQ>Zabcn1U1YA/XozTrY3ieapcjLAURSrLDMEOs+2SPbkE=</DQ><InverseQ>VWI54/c9ThBlSIEfEB2AO1m1lahhdb2IF8IuJmiq51c=</InverseQ><D>GBAW1+T7yG6CaZK6nyDOZEln1Jo98oGlZ9q+I4bV+6kvPtL9PX8In6HjD+U4JgdgfPuO5/PjEOef3CmzLNkKQQ==</D></RSAKeyValue>");

            //    string data2Decrypt = "kZBXGnNgSUj5nbJF9Q44Ik73aGbYj88Lxj2lKeG2+Y9IqCLl5wPVpDnF40y4lN/Ef/aZJGC/phnYEZLMwY6wPukpxNGVczVQEaZuLbkesEeTbZ+lGTiHE2J7rKYPwElvmLv2SHVdWDMa/NAiCOIX/gsnB8ofHZFiIUdOqCkuqYo=";

            byte[] encyrptedBytes = Convert.FromBase64String(text);

            byte[] plain = rsa1.Decrypt(encyrptedBytes, false);
            string decryptedString = System.Text.Encoding.UTF8.GetString(plain);
            return decryptedString;
        }
        #endregion

        #region EncryptQRCode
        public string EncryptQRCode(string text)
        {
            const int PROVIDER_RSA_FULL = 1;
            const string CONTAINER_NAME = "Tracker";

            CspParameters cspParams;
            cspParams = new CspParameters(PROVIDER_RSA_FULL);
            cspParams.KeyContainerName = CONTAINER_NAME;
            RSACryptoServiceProvider rsa1 = new RSACryptoServiceProvider(cspParams);
            rsa1.FromXmlString("<RSAKeyValue><Modulus>VyCgyOTW8mXf3ebTunY0r7fBg3O7KMjWqkqmav8PQP88YmUwNLsJ72L/RPjlykvcozwqqbvXdRC88UMVPSkRQw==</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>");

            System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
            byte[] textBytes = encoding.GetBytes(text);
            byte[] encryptedOutput = rsa1.Encrypt(textBytes, false);
            string outputB64 = Convert.ToBase64String(encryptedOutput);
            return outputB64;
        }
        #endregion
    }
}
