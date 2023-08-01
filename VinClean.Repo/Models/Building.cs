using System;
using System.Collections.Generic;

namespace VinClean.Repo.Models;

public partial class Building
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public int? TypeId { get; set; }

    public int? Floor { get; set; }

    public int? Room { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual BuildingType? Type { get; set; }
}
