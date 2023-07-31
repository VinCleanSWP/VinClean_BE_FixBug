using System;
using System.Collections.Generic;

namespace VinClean.Repo.Models;

public partial class Location
{
    public int Id { get; set; }

    public int? OrderId { get; set; }

    public int? EmployeeId { get; set; }

    public double? Latitude { get; set; }

    public double? Longtitude { get; set; }

    public virtual Employee? Employee { get; set; }

    public virtual Order? Order { get; set; }
}
