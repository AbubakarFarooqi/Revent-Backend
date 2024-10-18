using Revent.Common.CustomValidationAttributes;
using Revent.EFCore.DataModel.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revent.Common.CommonDtos
{
    public class EventCreateDto
    {
        [Required(ErrorMessage = "Name field is required")]
        [StringLength(100, ErrorMessage = "Name can't be longer than 100 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Latitude field is required")]
        public decimal Latitude { get; set; }

        [Required(ErrorMessage = "Longitude field is required")]
        public decimal Longitude { get; set; }

        [Required(ErrorMessage = "Start date is required")]
        [DataType(DataType.Date, ErrorMessage = "Invalid date format")]
        public DateOnly StartDate { get; set; }

        [Required(ErrorMessage = "End date is required")]
        [DataType(DataType.Date, ErrorMessage = "Invalid date format")]
        [DateGreaterThan("StartDate", ErrorMessage = "End date must be greater than start date")]
        public DateOnly EndDate { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Size limit must be a positive number")]
        public int SizeLimit { get; set; }

        [Range(0.0, double.MaxValue, ErrorMessage = "Ticket price must be a non-negative value")]
        [DataType(DataType.Currency)]
        public decimal TicketPrice { get; set; }

        [Required(ErrorMessage = "OtganizerId is required")]
        public int OrganizerId { get; set; }
        
        [Required(ErrorMessage = "EventTypeId is required")]
        public int EventTypeId { get; set; }
        public Users? Organizer { get; set; }
        public Lookups? EventType { get; set; }
    }
}
