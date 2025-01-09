using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Revent.EFCore.DataModel.Models;

[Table("tickets")]
public partial class Tickets
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("ticket_type")]
    public int TicketType { get; set; }

    [Column("event_id")]
    public int EventId { get; set; }

    [Column("price")]
    public int Price { get; set; }

    [Column("is_deleted")]
    public bool IsDeleted { get; set; }

    [ForeignKey("EventId")]
    [InverseProperty("Tickets")]
    public virtual Events Event { get; set; } = null!;

    [ForeignKey("TicketType")]
    [InverseProperty("Tickets")]
    public virtual Lookups TicketTypeNavigation { get; set; } = null!;
}
