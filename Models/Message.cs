using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRTut.Models
{
    public class Message
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Text { get; set; }

        public string SenderId { get; set; }

        public string RecId { get; set; }
    }
}
