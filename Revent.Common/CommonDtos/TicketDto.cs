using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revent.Common.CommonDtos
{
    public class TicketDto
    {
        [Required]
        public int EventId { get; set; }
        [Required]
        public int TicketTypeId { get; set; }
        [Required]
        public int Price { get; set; }


    }
}
