using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VinClean.Service.DTO.Rating
{
    public class AverageRatingDTO
    {
        public int TypeId { get; set; }

        public string Type1 { get; set; }
        public int RateId { get; set; }
        public double Rate { get; set; }
    }
}
