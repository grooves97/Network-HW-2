using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Models
{
    public class City : Entity
    {
        public string Name { get; set; }
        public ICollection<Street> Streets { get; set; }
    }
}
