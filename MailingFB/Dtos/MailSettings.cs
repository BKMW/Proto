using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MailingFB.Dtos
{
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
        public string URL { get; set; }

    }
}
