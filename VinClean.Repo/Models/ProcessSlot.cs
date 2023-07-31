using System;
using System.Collections.Generic;

namespace VinClean.Repo.Models;

public partial class ProcessSlot
{
    public int? ProcessId { get; set; }

    public int? SlotId { get; set; }

    public int? OldEmployeeId { get; set; }

    public int? NewEmployeeId { get; set; }

    public string? Note { get; set; }

    public string? Satus { get; set; }

    public DateTime? CreateAt { get; set; }

    public int? CreateBy { get; set; }

    public virtual Account? CreateByNavigation { get; set; }

    public virtual Employee? NewEmployee { get; set; }

    public virtual Employee? OldEmployee { get; set; }

    public virtual Process? Process { get; set; }

    public virtual Slot? Slot { get; set; }
}
