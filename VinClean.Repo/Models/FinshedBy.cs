using System;
using System.Collections.Generic;

namespace VinClean.Repo.Models;

public partial class FinshedBy
{
    public int? OrderId { get; set; }

    public int? EmployeeId { get; set; }

    public virtual Employee? Employee { get; set; }

    public virtual Order? Order { get; set; }
}
