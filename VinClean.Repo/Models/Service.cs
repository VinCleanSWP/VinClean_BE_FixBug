using System;
using System.Collections.Generic;

namespace VinClean.Repo.Models;

public partial class Service
{
    public int ServiceId { get; set; }

    public string? Name { get; set; }

    public decimal? Cost { get; set; }

    public int? MinimalSlot { get; set; }

    public string? Description { get; set; }

    public int? TypeId { get; set; }

    public string? Status { get; set; }

    public bool? Avaiable { get; set; }

    public bool? IsDeleted { get; set; }

    public DateTime? CreatedDate { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public int? ModifiedBy { get; set; }

    public virtual Account? CreatedByNavigation { get; set; }

    public virtual Account? ModifiedByNavigation { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<Rating> Ratings { get; set; } = new List<Rating>();

    public virtual Type? Type { get; set; }
}
