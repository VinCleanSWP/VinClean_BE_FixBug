using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VinClean.Repo.Models
{
    public class RatingModelDTO
    {
        public int RateId { get; set; }
        public int OrderId { get; set; }
        public int TypeId { get; set; }
        public int ServiceId { get; set; }
        public int CustomerId { get; set; }
        public string ServiceName { get; set; }
        public string ServiceType { get; set; }
        public string Note { get; set; }
        public string Img { get; set; }
        public string CustomerFirstName { get; set; }
        public string CustomerLastName { get; set; }
        public byte? Rate { get; set; }
        public string Comment { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? ModifiedBy { get; set; }
    }
}
