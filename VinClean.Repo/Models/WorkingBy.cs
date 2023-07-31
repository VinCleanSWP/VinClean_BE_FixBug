using System;
using System.Collections.Generic;

namespace VinClean.Repo.Models;

public partial class WorkingBy
{
    public int? ProcessId { get; set; }

    public int? EmployeeId { get; set; }

    public double? Latitude { get; set; }

    public double? Longtitude { get; set; }

    public virtual Employee? Employee { get; set; }

    public virtual Process? Process { get; set; }
}
