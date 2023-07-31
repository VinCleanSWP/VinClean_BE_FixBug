using System;
using System.Collections.Generic;

namespace VinClean.Repo.Models;

public partial class Category
{
    public int CategoryId { get; set; }

    public string? Category1 { get; set; }

    public virtual ICollection<Blog> Blogs { get; set; } = new List<Blog>();
}
