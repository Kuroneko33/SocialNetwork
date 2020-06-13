using BusinessLogic.Interfaces;
using Domain;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Implementations
{
    public class EFMessagesRepository : IMessagesRepository
    {
        private EFDbContext context;
        public EFMessagesRepository(EFDbContext context)
        {
            this.context = context;
        }

        #region Входящие сообщения
        public IEnumerable<IncomingMessage> GetIncomingMessages()
        {
            return context.IncomingMessages;
        }

        public IEnumerable<IncomingMessage> GetIncomingMessagesByUserId(int userId)
        {
            return context.IncomingMessages.Where(x => x.UserId == userId);
        }

        public void SaveIncomingMessage(IncomingMessage message)
        {
            if (message.Id == 0)
                context.IncomingMessages.Add(message);
            else
                context.Entry(message).State = EntityState.Modified;
            context.SaveChanges();
        }

        public void DeleteIncomingMessage(IncomingMessage message)
        {
            context.IncomingMessages.Remove(message);
            context.SaveChanges();
        }
        #endregion

        #region Исходящие сообщения
        public IEnumerable<OutgoingMessage> GetOutgoingMessages()
        {
            return context.OutgoingMessages;
        }

        public IEnumerable<OutgoingMessage> GetOutgoingMessagesByUserId(int userId)
        {
            return context.OutgoingMessages.Where(x => x.UserId == userId);
        }
        
        public void SaveOutgoingMessage(OutgoingMessage message)
        {
            if (message.Id == 0)
                context.OutgoingMessages.Add(message);
            else
                context.Entry(message).State = EntityState.Modified;
            context.SaveChanges(); 
        }

        public void DeleteOutgoingMessage(OutgoingMessage message)
        {
            context.OutgoingMessages.Remove(message);
            context.SaveChanges();
        }
        #endregion
    }
}
