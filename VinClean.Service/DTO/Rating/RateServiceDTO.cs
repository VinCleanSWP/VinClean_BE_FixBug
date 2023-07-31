using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VinClean.Service.DTO.Account;
using VinClean.Service.DTO.CustomerResponse;
using VinClean.Service.DTO.Service;

namespace VinClean.Service.DTO.Rating
{
    public class RateServiceDTO
    {
        public int RateId { get; set; }

        public byte Rate { get; set; }

        public string Comment { get; set; }

        public int ServiceId { get; set; }

        public int CustomerId { get; set; }

        public int accountId { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime ModifiedDate { get; set; }

        public int ModifiedBy { get; set; }

        public virtual ServiceDTO Service { get; set; }

        public virtual CustomerDTO Customer { get; set; }
    }
}
