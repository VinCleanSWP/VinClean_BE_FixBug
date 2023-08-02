using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VinClean.Service.DTO.Building
{
    public class BuildingDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int? TypeId { get; set; }
        public int? Floor { get; set; }
        public int? Room { get; set; }
    }
}
