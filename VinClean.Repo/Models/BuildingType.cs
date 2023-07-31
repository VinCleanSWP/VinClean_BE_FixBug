using System;
using System.Collections.Generic;

namespace VinClean.Repo.Models;

public partial class BuildingType
{
    public int Id { get; set; }

    public string? Type { get; set; }

    public virtual ICollection<Building> Buildings { get; set; } = new List<Building>();
}
