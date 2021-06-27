using System.Collections.Generic;

namespace Core.Dtos
{
    public class AppSettings
    {
        public string Version { get; set; }
        // Properties for JWT Token Signature
        public int ExpireTime { get; set; }
            
        public string OutDir { get; set; }
        public int Period { get; set; }

      
    }

    public class MailSettings
    {
      
        public string Email { get; set; }
        public string DisplayName { get; set; }
        public string PWD { get; set; }
        public int PORT { get; set; }
        public string HOST { get; set; }
        public IList<string> CC { get; set; }
        public IList<string> TO { get; set; }
        //URL Portal
        public string URLL { get; set; }

    }
    public class SwaggerSettings
    {
        public string Version { get; set; }
        // Properties for JWT Token Signature
        public int ExpireTime { get; set; }

        public string OutDir { get; set; }
        public int Period { get; set; }


    }

}
