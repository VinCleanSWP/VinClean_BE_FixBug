using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VinClean.Repo.Models.ProcessModel
{
    public class SelectOrder
    {
        public String StartMonth { get; set; }
        public String EndMonth { get; set; }

        public int EmployeeId { get; set; }
    }
}
