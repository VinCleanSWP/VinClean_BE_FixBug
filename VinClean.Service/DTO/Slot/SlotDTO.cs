using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VinClean.Service.DTO.Slot
{
    public class SlotDTO
    {
        public int SlotId { get; set; }

        public string? SlotName { get; set; }

        public byte? DayOfweek { get; set; }

        public TimeSpan? StartTime { get; set; }

        public TimeSpan? EndTime { get; set; }
    }
}
