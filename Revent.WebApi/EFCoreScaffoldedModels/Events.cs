using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Revent.EFCore.DataModel.Models;

[Table("events")]
public partial class Events
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("name")]
    [StringLength(255)]
    public string Name { get; set; } = null!;

    [Column("latitude")]
    [Precision(9, 6)]
    public decimal Latitude { get; set; }

    [Column("longitude")]
    [Precision(9, 6)]
    public decimal Longitude { get; set; }

    [Column("start_date")]
    public DateOnly StartDate { get; set; }

    [Column("end_date")]
    public DateOnly EndDate { get; set; }

    [Column("organizer_id")]
    public int OrganizerId { get; set; }

    [Column("size_limit")]
    public int? SizeLimit { get; set; }

    [Column("ticket_price")]
    [Precision(10, 2)]
    public decimal? TicketPrice { get; set; }

    [Column("created_at", TypeName = "timestamp without time zone")]
    public DateTime? CreatedAt { get; set; }

    [Column("updated_at", TypeName = "timestamp without time zone")]
    public DateTime? UpdatedAt { get; set; }

    [Column("event_type")]
    public int EventType { get; set; }

    [Column("group_chat_id")]
    public int? GroupChatId { get; set; }

    [Column("is_deleted")]
    public bool? IsDeleted { get; set; }

    [ForeignKey("EventType")]
    [InverseProperty("Events")]
    public virtual Lookups EventTypeNavigation { get; set; } = null!;

    [ForeignKey("GroupChatId")]
    [InverseProperty("Events")]
    public virtual GroupChats? GroupChat { get; set; }

    [ForeignKey("OrganizerId")]
    [InverseProperty("Events")]
    public virtual Users Organizer { get; set; } = null!;
}
