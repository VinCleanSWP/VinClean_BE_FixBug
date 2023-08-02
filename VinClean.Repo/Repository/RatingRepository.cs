using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VinClean.Repo.Models;

// lay table

namespace VinClean.Repo.Repository
{
    public interface IRatingRepository
    {
        Task<ICollection<RatingModelDTO>> GetRatinglist();
        Task<ICollection<RatingModelDTO>> GetRatingByService(int id);
        Task<Rating> GetRatingById(int id);
        Task<bool> AddRating(Rating rating);
        Task<bool> UpdateRating(Rating rating);
        Task<bool> DeleteRating(Rating rating);
        Task<bool> CheckServiceRating(int serviceId, int customerId);

    }
    public class RatingRepository : IRatingRepository
    {
        private readonly ServiceAppContext _context;
        public RatingRepository(ServiceAppContext context)
        {
            _context = context;
        }

        async Task<ICollection<RatingModelDTO>> IRatingRepository.GetRatinglist()
        {
            //return await _context.Ratings.ToListAsync();

            var list = from s in _context.Services
                       join t in _context.Types on s.TypeId equals t.TypeId into st
                       from subt in st.DefaultIfEmpty()
                       join r in _context.Ratings on s.ServiceId equals r.ServiceId into sr
                       from subr in sr.DefaultIfEmpty()
                       join c in _context.Customers on subr.CustomerId equals c.CustomerId into cr
                       from subc in cr.DefaultIfEmpty()
                       join o in _context.Orders on subc.CustomerId equals o.CustomerId into oc
                       from subo in oc.DefaultIfEmpty()
                       where subo.CustomerId == subc.CustomerId && s.ServiceId == subr.ServiceId
                       join a in _context.Accounts on subc.AccountId equals a.AccountId into ac
                       from suba in ac.DefaultIfEmpty()
                       select new RatingModelDTO
                       {
                           RateId = subr.RateId,
                           OrderId = subo.OrderId,
                           ServiceName = s.Name,
                           ServiceType = subt.Type1,
                           Note = subo.Note,
                           CustomerFirstName = subc.FirstName,
                           CustomerLastName = subc.LastName,
                           Rate = subr.Rate,
                           Comment = subr.Comment,
                           CreatedDate = subr.CreatedDate,
                           Img = suba.Img
                       };

            return await list.ToListAsync();
        }

        async Task<ICollection<RatingModelDTO>> IRatingRepository.GetRatingByService(int id)
        {
            //return await _context.Ratings.Include(r => r.Service)
            //                     .Where(r => r.Service.TypeId == id)
            //                     .Include(c => c.Customer)
            //                     .ToListAsync();

            var list = from r in _context.Ratings
                        join s in _context.Services on r.ServiceId equals s.ServiceId into rs
                        from subS in rs.DefaultIfEmpty()
                        join t in _context.Types on subS.TypeId equals t.TypeId into st
                        from subT in st.DefaultIfEmpty()
                        join c in _context.Customers on r.CustomerId equals c.CustomerId into cr
                        from subC in cr.DefaultIfEmpty()
                        join a in _context.Accounts on subC.AccountId equals a.AccountId into ac
                        from subA in ac.DefaultIfEmpty()
                        where subS.TypeId == id
                        select new RatingModelDTO
                        {
                            TypeId = subT.TypeId,
                            ServiceId = subS.ServiceId,
                            ServiceName = subS.Name,
                            CustomerId = subC.CustomerId,
                            Img = subA.Img,
                            CustomerFirstName = subC.FirstName,
                            CustomerLastName = subC.LastName,
                            Rate = r.Rate,
                            Comment = r.Comment,
                            CreatedDate = r.CreatedDate,
                            ModifiedDate = r.ModifiedDate,
                            ModifiedBy = r.ModifiedBy
                        };
            return await list.ToListAsync();
        }

        async Task<Rating> IRatingRepository.GetRatingById(int id)
        {
            return await _context.Ratings.FirstOrDefaultAsync(r => r.RateId == id);
        }

        async Task<bool> IRatingRepository.AddRating(Rating rating)
        {
            _context.Entry(rating).State = EntityState.Added;
            return await _context.SaveChangesAsync() > 0 ? true : false;
        }

        async Task<bool> IRatingRepository.UpdateRating(Rating rating)
        {
            _context.Ratings.Update(rating);
            return await _context.SaveChangesAsync() > 0 ? true : false;
        }

        async Task<bool> IRatingRepository.DeleteRating(Rating rating)
        {
            _context.Ratings.Remove(rating);
            return await _context.SaveChangesAsync() > 0 ? true : false;
        }

        async Task<bool> IRatingRepository.CheckServiceRating(int serviceId, int customerId)
        {
            //return await _context.Ratings.Include(s => s.Service).FirstAsync(OrderDetail)
            //    .Include(od => od.OrderDetail).Include("Orders")
            //    .Where(od => od.ServiceId == serviceId).FirstOrDefaultAsync();


            var check = from r in _context.Ratings
                        join s in _context.Services on r.ServiceId equals s.ServiceId
                        join o in _context.Orders on s.ServiceId equals o.ServiceId
                        join c in _context.Customers on o.CustomerId equals c.CustomerId
                        where o.CustomerId == customerId && o.ServiceId == serviceId
                        select o.ServiceId;
            return await check.AnyAsync();
        }
    }
}
