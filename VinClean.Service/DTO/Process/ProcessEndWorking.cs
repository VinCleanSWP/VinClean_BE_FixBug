using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VinClean.Service.DTO.Process
{
    public class ProcessEndWorking
    {
        public int ProcessId { get; set; }
        public TimeSpan EndWorking { get; set; }
    }
}
