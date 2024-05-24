using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace talabat.core.Entites
{
    public class Department : BaseEntity
    {
        public string Name { get; set; }
        public DateOnly DateOfCreation { get; set; }

    }
}
