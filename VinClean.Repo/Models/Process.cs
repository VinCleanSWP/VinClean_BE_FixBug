using System;
using System.Collections.Generic;

namespace VinClean.Repo.Models;

public partial class Process
{
    public int ProcessId { get; set; }

    public int? CustomerId { get; set; }

    public string? Status { get; set; }

    public string? Note { get; set; }

    public bool? IsDeleted { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public int? ModifiedBy { get; set; }

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

    public virtual Customer? Customer { get; set; }

    public virtual Account? ModifiedByNavigation { get; set; }

    public virtual ICollection<ProcessImage> ProcessImages { get; set; } = new List<ProcessImage>();
}
