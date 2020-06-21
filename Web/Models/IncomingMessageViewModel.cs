using Domain.Entities;

namespace Web.Models
{
    public class IncomingMessageViewModel
    {
        //Входящее сообщение
        public IncomingMessage Message { get; set; }

        //Автор сообщения
        public User UserFrom { get; set; }
    }
}