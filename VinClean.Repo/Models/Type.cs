using System;
using System.Collections.Generic;

namespace VinClean.Repo.Models;

public partial class Type
{
    public int TypeId { get; set; }

    public string? Type1 { get; set; }

    public bool? Avaiable { get; set; }

    public string? Img { get; set; }

    public virtual ICollection<Service> Services { get; set; } = new List<Service>();
}
