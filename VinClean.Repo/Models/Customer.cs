using System;
using System.Collections.Generic;

namespace VinClean.Repo.Models;

public partial class Customer
{
    public int CustomerId { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Phone { get; set; }

    public string? Address { get; set; }

    public decimal? TotalMoney { get; set; }

    public int? TotalPoint { get; set; }

    public string? Status { get; set; }

    public int? AccountId { get; set; }

    public DateTime? Dob { get; set; }

    public virtual Account? Account { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<Rating> Ratings { get; set; } = new List<Rating>();
}
