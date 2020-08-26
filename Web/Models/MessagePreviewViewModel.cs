using Domain.Entities;
using System;

namespace Web.Models
{
    public class MessagePreviewViewModel
    {
        public string Text { get; set; }
        public User UserFrom { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}