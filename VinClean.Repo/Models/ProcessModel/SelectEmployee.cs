using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VinClean.Repo.Models.ProcessModel
{
    public class SelectEmployee
    {
        public DateTime Date { get; set; }

        public TimeSpan StarTime { get; set; }

        public TimeSpan EndTime { get; set; }
    }
}
