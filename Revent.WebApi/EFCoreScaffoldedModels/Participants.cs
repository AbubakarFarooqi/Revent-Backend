using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Revent.EFCore.DataModel.Models;

[Table("participants")]
public partial class Participants
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("group_chat_id")]
    public int GroupChatId { get; set; }

    [Column("user_id")]
    public int UserId { get; set; }

    [Column("joined_at", TypeName = "timestamp without time zone")]
    public DateTime? JoinedAt { get; set; }

    [ForeignKey("GroupChatId")]
    [InverseProperty("Participants")]
    public virtual GroupChats GroupChat { get; set; } = null!;

    [InverseProperty("Participant")]
    public virtual ICollection<GroupMessages> GroupMessages { get; set; } = new List<GroupMessages>();

    [ForeignKey("UserId")]
    [InverseProperty("Participants")]
    public virtual Users User { get; set; } = null!;
}
