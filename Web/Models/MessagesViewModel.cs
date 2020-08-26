using Domain.Entities;
using System;

namespace Web.Models
{
    public class MessagesViewModel : IComparable
    {
        public int Id { get; set; }
        public int UserFromId { get; set; }
        public int UserToId { get; set; }
        public string Text { get; set; }
        public DateTime CreatedDate { get; set; }
        public User UserFrom { get; set; }

        public int CompareTo(object obj)
        {
            MessagesViewModel message = obj as MessagesViewModel;
            if (message != null)
                return this.CreatedDate.CompareTo(message.CreatedDate);
            else
                throw new Exception("Невозможно сравнить два объекта");
        }
    }
}