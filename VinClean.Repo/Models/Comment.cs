using System;
using System.Collections.Generic;

namespace VinClean.Repo.Models;

public partial class Comment
{
    public int CommentId { get; set; }

    public string? Content { get; set; }

    public int? BlogId { get; set; }

    public bool? IsDeleted { get; set; }

    public DateTime? CreatedDate { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public int? ModifiedBy { get; set; }

    public virtual Blog? Blog { get; set; }

    public virtual Account? CreatedByNavigation { get; set; }

    public virtual Account? ModifiedByNavigation { get; set; }
}
