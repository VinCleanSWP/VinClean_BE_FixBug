using System;
using System.Collections.Generic;

namespace VinClean.Repo.Models;

public partial class Blog
{
    public int BlogId { get; set; }

    public string? Title { get; set; }

    public string? Sumarry { get; set; }

    public string? Content { get; set; }

    public int? CategoryId { get; set; }

    public bool? IsDeleted { get; set; }

    public DateTime? CreatedDate { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public int? ModifiedBy { get; set; }

    public string? Img { get; set; }

    public virtual Category? Category { get; set; }

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual Account? CreatedByNavigation { get; set; }

    public virtual Account? ModifiedByNavigation { get; set; }
}
