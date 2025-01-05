using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Revent.EFCore.DataModel.Models;

[Table("group_messages")]
public partial class GroupMessages
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("group_chat_id")]
    public int GroupChatId { get; set; }

    [Column("participant_id")]
    public int ParticipantId { get; set; }

    [Column("message")]
    public string Message { get; set; } = null!;

    [Column("sent_at", TypeName = "timestamp without time zone")]
    public DateTime? SentAt { get; set; }

    [ForeignKey("GroupChatId")]
    [InverseProperty("GroupMessages")]
    public virtual GroupChats GroupChat { get; set; } = null!;

    [ForeignKey("ParticipantId")]
    [InverseProperty("GroupMessages")]
    public virtual Participants Participant { get; set; } = null!;
}
