using System;
using System.Collections.Generic;

namespace VinClean.Repo.Models;

public partial class Employee
{
    public int EmployeeId { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Phone { get; set; }

    public string? Status { get; set; }

    public int? AccountId { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public virtual Account? Account { get; set; }

    public virtual ICollection<Location> Locations { get; set; } = new List<Location>();

    public virtual ICollection<OrderRequest> OrderRequestNewEmployees { get; set; } = new List<OrderRequest>();

    public virtual ICollection<OrderRequest> OrderRequestOldEmployees { get; set; } = new List<OrderRequest>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
