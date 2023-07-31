using System;
using System.Collections.Generic;

namespace VinClean.Repo.Models;

public partial class Slot
{
    public int SlotId { get; set; }

    public string? SlotName { get; set; }

    public byte? DayOfweek { get; set; }

    public TimeSpan? StartTime { get; set; }

    public TimeSpan? EndTime { get; set; }
}
