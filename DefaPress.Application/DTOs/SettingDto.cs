using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DefaPress.Application.DTOs
{
    // DTOs/Setting, Newsletter, Contact, Audit
    public class SettingDto
    {
        public int Id { get; set; }
        public string Category { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
    }
    public class SettingCreateDto
    {
        public string Category { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
    }

    public class SettingUpdateDto
    {
        public string Value { get; set; }
    }
}
