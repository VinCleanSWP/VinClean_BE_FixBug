using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VinClean.Repo.Models;

namespace VinClean.Service.DTO.WorkingSlot
{
    public class LocationDTO
    {
        public int OrderId { get; set; }
        public int EmployeeId { get; set; }
        public double Latitude { get; set; }

        public double Longtitude { get; set; }
    }
}
