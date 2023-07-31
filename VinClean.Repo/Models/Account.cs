using System;
using System.Collections.Generic;

namespace VinClean.Repo.Models;

public partial class Account
{
    public int AccountId { get; set; }

    public string? Name { get; set; }

    public string? Password { get; set; }

    public string? Email { get; set; }

    public int? RoleId { get; set; }

    public string? Status { get; set; }

    public bool? IsDeleted { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? Dob { get; set; }

    public string? Gender { get; set; }

    public string? Img { get; set; }

    public string? VerificationToken { get; set; }

    public DateTime? VerifiedAt { get; set; }

    public string? PasswordResetToken { get; set; }

    public DateTime? ResetTokenExpires { get; set; }

    public virtual ICollection<Blog> BlogCreatedByNavigations { get; set; } = new List<Blog>();

    public virtual ICollection<Blog> BlogModifiedByNavigations { get; set; } = new List<Blog>();

    public virtual ICollection<Comment> CommentCreatedByNavigations { get; set; } = new List<Comment>();

    public virtual ICollection<Comment> CommentModifiedByNavigations { get; set; } = new List<Comment>();

    public virtual ICollection<Customer> Customers { get; set; } = new List<Customer>();

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();

    public virtual ICollection<OrderRequest> OrderRequests { get; set; } = new List<OrderRequest>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<Rating> Ratings { get; set; } = new List<Rating>();

    public virtual Role? Role { get; set; }

    public virtual ICollection<Service> ServiceCreatedByNavigations { get; set; } = new List<Service>();

    public virtual ICollection<Service> ServiceModifiedByNavigations { get; set; } = new List<Service>();
}
