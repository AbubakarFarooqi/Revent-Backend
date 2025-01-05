using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Revent.EFCore.DataModel.Models;

[Index("NormalizedName", Name = "RoleNameIndex", IsUnique = true)]
public partial class AspNetRoles
{
    [Key]
    public string Id { get; set; } = null!;

    [StringLength(256)]
    public string? Name { get; set; }

    [StringLength(256)]
    public string? NormalizedName { get; set; }

    public string? ConcurrencyStamp { get; set; }

    [InverseProperty("Role")]
    public virtual ICollection<AspNetRoleClaims> AspNetRoleClaims { get; set; } = new List<AspNetRoleClaims>();

    [ForeignKey("RoleId")]
    [InverseProperty("Role")]
    public virtual ICollection<AspNetUsers> User { get; set; } = new List<AspNetUsers>();
}
