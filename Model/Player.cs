using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1.Model
{
    internal class Player
    {
        public int ID { get; set; }
        public string FullName { get; set; }
        public string Country { get; set; }
        public string Contact { get; set; }
        public string Email { get; set; }
        public int? Age { get; set; }
    }
}
