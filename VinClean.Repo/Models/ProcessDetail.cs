using System;
using System.Collections.Generic;

namespace VinClean.Repo.Models;

public partial class ProcessDetail
{
    public int? ProcessId { get; set; }

    public int? ServiceId { get; set; }

    public virtual Process? Process { get; set; }

    public virtual Service? Service { get; set; }
}
