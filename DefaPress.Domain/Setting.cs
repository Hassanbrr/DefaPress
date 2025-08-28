using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DefaPress.Domain
{
    public class Setting
    {
        public int Id { get; set; }
        public string Category { get; set; } // General, SEO, Mail
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
