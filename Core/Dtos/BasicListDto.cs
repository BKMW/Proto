using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Dtos
{
    public class BasicListDto
    {
        public string page { get; set; }
        public string size { get; set; }
        public string startDate { get; set; }
        public string lastDate { get; set; }
    }
}
