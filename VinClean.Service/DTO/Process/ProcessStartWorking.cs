using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VinClean.Service.DTO.Process
{
    public class ProcessStartWorking
    {
        public int ProcessId { get; set; }
        public TimeSpan StartWorking { get; set; }
    }
}
