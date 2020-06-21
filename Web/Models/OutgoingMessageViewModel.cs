using Domain.Entities;

namespace Web.Models
{
    public class OutgoingMessageViewModel
    {
        //Исходящее сообщение
        public OutgoingMessage Message { get; set; }

        //Адресат сообщения
        public User UserTo { get; set; }
    }
}