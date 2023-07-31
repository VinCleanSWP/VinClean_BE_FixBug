using System;
using System.Collections.Generic;

namespace VinClean.Repo.Models;

public partial class WorkingSlot
{
    public int? SlotId { get; set; }

    public int? EmployeeId { get; set; }

    public virtual Employee? Employee { get; set; }

    public virtual Slot? Slot { get; set; }
}
