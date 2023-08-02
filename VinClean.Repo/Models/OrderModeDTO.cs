using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VinClean.Repo.Models
{
    public class OrderModeDTO
    {
        public int OrderId { get; set; }
        public int? CustomerId { get; set; }
        public int? AccountId { get; set; }
        public int? BuildingId { get; set; }
        public int? RatingId { get; set; }
        public string? Name { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string? Email { get; set; }
        public DateTime Date { get; set; }
        public DateTime? Dob { get; set; }
        public string? Status { get; set; }
        public string? Note { get; set; }
        public bool? IsDeleted { get; set; }
        public TimeSpan? StartWorking { get; set; }
        public TimeSpan? EndWorking { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? CancelDate { get; set; }
        public int? CancelBy { get; set; }
        public string? CancelByName { get; set; }
        public string? CancelByRole { get; set; }
        public int? ServiceId { get; set; }
        public string? ServiceName { get; set; }
        public decimal? CostPerSlot { get; set; }
        public int? MinimalSlot { get; set; }
        public int? TypeId { get; set; }
        public string? TypeName { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
        public decimal? Price { get; set; }
        public decimal? SubPrice { get; set; }
        public int? PointUsed { get; set; }
        public string? AccountImage { get; set; }
        public string? EmployeeImage { get; set; }
        public int? EmployeeId { get; set; }
        public int? EmployeeAccountId { get; set; }
        public string? EmployeeName { get; set; }
        public string? EmployeePhone { get; set; }
        public string? EmployeeEmail { get; set; }
        public double? Latitude { get; set; }
        public double? Longtitude { get; set; }
        public string? ReasonCancel { get; set; }
    }
}
