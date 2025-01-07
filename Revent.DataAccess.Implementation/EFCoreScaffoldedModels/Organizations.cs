using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Revent.EFCore.DataModel.Models;

[Table("organizations")]
[Index("UserId", Name = "organizations_user_id_key", IsUnique = true)]
public partial class Organizations
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("user_id")]
    public int UserId { get; set; }

    [Column("contact_number", TypeName = "character varying")]
    public string? ContactNumber { get; set; }

    [Column("organization_type")]
    public int OrganizationType { get; set; }

    [Column("address")]
    [StringLength(255)]
    public string? Address { get; set; }

    [Column("is_deleted")]
    public bool IsDeleted { get; set; }

    [ForeignKey("ContactNumber")]
    [InverseProperty("OrganizationsContactNumberNavigation")]
    public virtual Users? ContactNumberNavigation { get; set; }

    [ForeignKey("OrganizationType")]
    [InverseProperty("Organizations")]
    public virtual Lookups OrganizationTypeNavigation { get; set; } = null!;

    [ForeignKey("UserId")]
    [InverseProperty("OrganizationsUser")]
    public virtual Users User { get; set; } = null!;
}
