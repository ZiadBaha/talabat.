using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace talabat.core.Entites
{
    public class Employee : BaseEntity
    {
        public string Name { get; set; }
        public Decimal Slary { get; set; }
        public int? age { get; set; }
        public int Departmentid { get; set; }
        public Department Department { get; set; } // Navigational Property [ one ]
    }
}
