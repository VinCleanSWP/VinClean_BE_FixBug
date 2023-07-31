using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VinClean.Service.DTO.Rating
{
    public class GetDetailDTO
    {
        public int OrderId { get; set; }

        public int ServiceId { get; set; }

        public virtual CheckOrderDTO Order { get; set; }

        public virtual RateServiceDTO Service { get; set; }
    }
}
