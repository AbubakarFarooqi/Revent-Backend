using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Revent.EFCore.DataModel.Models;

[Table("group_chats")]
public partial class GroupChats
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("created_at", TypeName = "timestamp without time zone")]
    public DateTime? CreatedAt { get; set; }

    [Column("updated_at", TypeName = "timestamp without time zone")]
    public DateTime? UpdatedAt { get; set; }

    [Column("name")]
    [StringLength(255)]
    public string? Name { get; set; }

    [InverseProperty("GroupChat")]
    public virtual ICollection<Events> Events { get; set; } = new List<Events>();

    [InverseProperty("GroupChat")]
    public virtual ICollection<GroupMessages> GroupMessages { get; set; } = new List<GroupMessages>();

    [InverseProperty("GroupChat")]
    public virtual ICollection<Participants> Participants { get; set; } = new List<Participants>();
}
