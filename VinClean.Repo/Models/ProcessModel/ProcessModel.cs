using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VinClean.Repo.Models.ProcessModel
{
    public class ProcessModel
    {
        public int ProcessId { get; set; }
        public Customer Customer { get; set; }
        public ICollection<ProcessDetail> ProcessDetails { get; set; }
        public ICollection<ProcessSlot> ProcessSlots { get; set; }
        public ICollection<WorkingBy> WorkingBies { get; set; }
        public string Status { get; set; }
        public string Note { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }

    }
}
