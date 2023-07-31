using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VinClean.Repo.Models.ProcessModel
{
    public class SelectEmployeeForProcess
    {
        public virtual Employee? Employee { get; set; }

        public virtual Process? Process { get; set; }

        public virtual WorkingBy? WorkingBy { get; set; }

    }
}
