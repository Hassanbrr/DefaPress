using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DefaPress.Domain
{
    namespace YourProject.Domain.Entities
    {
        public class Tag
        {
            public int TagId { get; set; }
            public string Name { get; set; }
            public ICollection<Article> Articles { get; set; }
        }
    }

}
