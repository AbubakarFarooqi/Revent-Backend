using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Revent.EFCore.DataModel.Models;

[Table("users")]
public partial class Users
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("firstname")]
    [StringLength(100)]
    public string Firstname { get; set; } = null!;

    [Column("lastname")]
    [StringLength(100)]
    public string Lastname { get; set; } = null!;

    [Column("profileimage")]
    [StringLength(500)]
    public string? Profileimage { get; set; }

    [Column("email")]
    [StringLength(256)]
    public string Email { get; set; } = null!;

    [Column("aspnetuserid")]
    public string Aspnetuserid { get; set; } = null!;

    [Column("createdat", TypeName = "timestamp without time zone")]
    public DateTime Createdat { get; set; }

    [Column("updatedat", TypeName = "timestamp without time zone")]
    public DateTime Updatedat { get; set; }

    [Column("two_factor_secret", TypeName = "character varying")]
    public string? TwoFactorSecret { get; set; }

    [ForeignKey("Aspnetuserid")]
    [InverseProperty("Users")]
    public virtual AspNetUsers Aspnetuser { get; set; } = null!;

    [InverseProperty("Organizer")]
    public virtual ICollection<Events> Events { get; set; } = new List<Events>();

    [InverseProperty("User")]
    public virtual ICollection<Participants> Participants { get; set; } = new List<Participants>();
}
