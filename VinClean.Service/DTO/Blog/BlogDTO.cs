using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VinClean.Service.DTO.Blog
{
    public class BlogDTO
    {
        public int BlogId { get; set; }
        public string Img { get; set; }

        public string Title { get; set; }

        public string Sumarry { get; set; }

        public string Content { get; set; }

        public int CategoryId { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime CreatedDate { get; set; }

        public int CreatedBy { get; set; }

        public DateTime ModifiedDate { get; set; }

        public int ModifiedBy { get; set; }
       
    }
}
