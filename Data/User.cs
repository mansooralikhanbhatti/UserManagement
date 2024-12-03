using System;
using System.Collections.Generic;

namespace UserManagement.Data;

public partial class User
{
    public int UserId { get; set; }

    public string? FirstName { get; set; }

    public string? MiddleName { get; set; }

    public string? LastName { get; set; }

    public string? Address { get; set; }

    public string? PhoneNumber { get; set; }

    public string? EmailId { get; set; }

    public string? City { get; set; }

    public string? State { get; set; }

    public string? PinCode { get; set; }

    public string? Username { get; set; }

    public string? Password { get; set; }

    public int? StatusId { get; set; }

    public DateTime? CreatedDate { get; set; }

    public virtual UserStatus? Status { get; set; }
}
