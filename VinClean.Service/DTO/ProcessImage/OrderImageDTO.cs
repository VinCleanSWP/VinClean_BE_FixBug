using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VinClean.Service.DTO.ProcessImage
{
    public class OrderImageDTO
    {
        public int Id { get; set; }

        public int OrderId { get; set; }

        public string Type { get; set; }

        public string Name { get; set; }

        public string Image { get; set; }

    }
}
