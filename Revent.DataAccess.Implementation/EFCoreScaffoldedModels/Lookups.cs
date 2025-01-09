using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Revent.EFCore.DataModel.Models;

[Table("lookups")]
public partial class Lookups
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("value")]
    [StringLength(255)]
    public string Value { get; set; } = null!;

    [Column("data")]
    [StringLength(255)]
    public string Data { get; set; } = null!;

    [InverseProperty("EventTypeNavigation")]
    public virtual ICollection<Events> Events { get; set; } = new List<Events>();

    [InverseProperty("OrganizationTypeNavigation")]
    public virtual ICollection<Organizations> Organizations { get; set; } = new List<Organizations>();

    [InverseProperty("TicketTypeNavigation")]
    public virtual ICollection<Tickets> Tickets { get; set; } = new List<Tickets>();
}
