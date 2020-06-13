using System;

namespace Domain.Entities
{
    public class IncomingMessage
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int UserFromId { get; set; }
        public string Text { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
