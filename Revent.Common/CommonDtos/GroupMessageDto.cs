using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revent.Common.CommonDtos
{
    public class GroupMessageDto
    {
        public string? Id { get; set; }
        public int GroupChatId { get; set; }
        public int ParticipantId { get; set; }
        public string Message { get; set; }
    }
}
