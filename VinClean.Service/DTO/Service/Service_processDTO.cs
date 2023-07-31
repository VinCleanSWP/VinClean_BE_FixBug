using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VinClean.Repo.Models;
using VinClean.Service.DTO.Type;

namespace VinClean.Service.DTO.Service
{
    public class Service_processDTO
    {
        public int ServiceId { get; set; }

        public string? Name { get; set; }

        public decimal? CostPerSlot { get; set; }

        public byte? MinimalSlot { get; set; }

        public string? Description { get; set; }

        public int? TypeId { get; set; }

        public virtual TypeDTO Type { get; set; }


    }

}
