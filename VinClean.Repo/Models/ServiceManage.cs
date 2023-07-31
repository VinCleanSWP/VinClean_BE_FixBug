using System;
using System.Collections.Generic;

namespace VinClean.Repo.Models;

public partial class ServiceManage
{
    public int? EmployeeId { get; set; }

    public int? ServiceId { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public virtual Employee? Employee { get; set; }

    public virtual Service? Service { get; set; }
}
