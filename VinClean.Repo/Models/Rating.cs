using System;
using System.Collections.Generic;

namespace VinClean.Repo.Models;

public partial class Rating
{
    public int RateId { get; set; }

    public byte? Rate { get; set; }

    public string? Comment { get; set; }

    public int? ServiceId { get; set; }

    public int? CustomerId { get; set; }

    public bool? IsDeleted { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public int? ModifiedBy { get; set; }

    public virtual Customer? Customer { get; set; }

    public virtual Account? ModifiedByNavigation { get; set; }

    public virtual Service? Service { get; set; }
}
