using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using talabat.core.Entites;

namespace talabat.core.Specifications.Employee_specs
{
    public class EmployeeWithDepartmentSpecification :baseSpecification<Employee>
    {
        public EmployeeWithDepartmentSpecification()
        {
            Iincludes.Add(E => E.Department);
        }

        public EmployeeWithDepartmentSpecification(int id) :base(Employee=>Employee.id==id)
        {
            Iincludes.Add(E => E.Department);
        }
    }
}
