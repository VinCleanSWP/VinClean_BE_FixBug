using System;
using System.Collections.Generic;

namespace VinClean.Repo.Models;

public partial class Order
{
    public int OrderId { get; set; }

    public int? CustomerId { get; set; }

    public int? ServiceId { get; set; }

    public int? RatingId { get; set; }

    public string? Status { get; set; }

    public string? Note { get; set; }

    public bool? IsDeleted { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? CancelDate { get; set; }

    public int? CancelBy { get; set; }

    public TimeSpan? StartWorking { get; set; }

    public TimeSpan? EndWorking { get; set; }

    public DateTime? Date { get; set; }

    public TimeSpan? StarTime { get; set; }

    public TimeSpan? EndTime { get; set; }

    public string? Address { get; set; }

    public string? Phone { get; set; }

    public decimal? Price { get; set; }

    public int? PointUsed { get; set; }

    public decimal? SubPrice { get; set; }

    public int? BuildingId { get; set; }

    public int? EmployeeId { get; set; }

    public string? ReasonCancel { get; set; }

    public virtual Building? Building { get; set; }

    public virtual Account? CancelByNavigation { get; set; }

    public virtual Customer? Customer { get; set; }

    public virtual Employee? Employee { get; set; }

    public virtual ICollection<Location> Locations { get; set; } = new List<Location>();

    public virtual ICollection<OrderImage> OrderImages { get; set; } = new List<OrderImage>();

    public virtual ICollection<OrderRequest> OrderRequests { get; set; } = new List<OrderRequest>();

    public virtual Rating? Rating { get; set; }

    public virtual Service? Service { get; set; }
}
