using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    class OutgoingMessage
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int UserToId { get; set; }
        [Required(ErrorMessage ="*")]
        public string Text { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
