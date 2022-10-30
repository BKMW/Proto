using System.Collections.Generic;

namespace Application.Dtos
{
    public class AppSettings
    {
        public string Version { get; set; }
        // Properties for JWT Token Signature
        public int ExpireTime { get; set; }
            
        public string OutDir { get; set; }
        public int Period { get; set; }

      
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
