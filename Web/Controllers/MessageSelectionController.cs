using BusinessLogic;
using Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using Web.Models;

namespace Web.Controllers
{
    public class MessageSelectionController : Controller
    {
        private const string prefix = "Вы: ";
        private DataManager dataManager;
        public MessageSelectionController(DataManager dataManager)
        {
            this.dataManager = dataManager;
        }

        private int GetCurrentUserId()
        {
            return (int)Membership.GetUser().ProviderUserKey;
        }

        public ActionResult Index()
        {
            List<MessagePreviewViewModel> models = new List<MessagePreviewViewModel>();

            List<IncomingMessage> incomingMessages = dataManager.Messages.GetIncomingMessagesByUserId(GetCurrentUserId()).ToList();
            incomingMessages.Reverse();
            List<OutgoingMessage> outgoingMessages = dataManager.Messages.GetOutgoingMessagesByUserId(GetCurrentUserId()).ToList();
            outgoingMessages.Reverse();

            List<IncomingMessage> uniqueIncomingMessages = new List<IncomingMessage>();
            foreach (var item in incomingMessages)
            {
                if (uniqueIncomingMessages.Where(i => i.UserFromId == item.UserFromId).ToList().Count == 0)
                {
                    uniqueIncomingMessages.Add(item);
                }
            }
            List<OutgoingMessage> uniqueOutgoingMessages = new List<OutgoingMessage>();
            foreach (var item in outgoingMessages)
            {
                if (uniqueOutgoingMessages.Where(i => i.UserToId == item.UserToId).ToList().Count == 0)
                {
                    uniqueOutgoingMessages.Add(item);
                }
            }
            if (uniqueIncomingMessages.Count > uniqueOutgoingMessages.Count)
            {
                foreach (var item in uniqueIncomingMessages)
                {
                    var tempList = uniqueOutgoingMessages.Where(i => i.UserToId == item.UserFromId).ToList();
                    if (tempList.Count == 0)
                    {
                        models.Add(
                           new MessagePreviewViewModel
                           {
                               Text = item.Text,
                               UserFrom = dataManager.Users.GetUserById(item.UserFromId),
                               CreatedDate = item.CreatedDate
                           });
                    }
                    else
                    {
                        var item2 = tempList[0];
                        if (item.CreatedDate > item2.CreatedDate)
                        {
                            models.Add(
                               new MessagePreviewViewModel
                               {
                                   Text = item.Text,
                                   UserFrom = dataManager.Users.GetUserById(item.UserFromId),
                                   CreatedDate = item.CreatedDate
                               });
                        }
                        else
                        {
                            models.Add(
                               new MessagePreviewViewModel
                               {
                                   Text = prefix + item2.Text,
                                   UserFrom =  dataManager.Users.GetUserById(item2.UserToId),
                                   CreatedDate = item2.CreatedDate
                               });
                        }
                    }
                }
            }
            else
            {
                foreach (var item in uniqueOutgoingMessages)
                {
                    var tempList = uniqueIncomingMessages.Where(i => i.UserFromId == item.UserToId).ToList();
                    if (tempList.Count == 0)
                    {
                        models.Add(
                           new MessagePreviewViewModel
                           {
                               Text = prefix + item.Text,
                               UserFrom = dataManager.Users.GetUserById(item.UserToId),
                               CreatedDate = item.CreatedDate
                           });
                    }
                    else
                    {
                        var item2 = tempList[0];
                        if (item.CreatedDate > item2.CreatedDate)
                        {
                            models.Add(
                               new MessagePreviewViewModel
                               {
                                   Text = prefix + item.Text,
                                   UserFrom = dataManager.Users.GetUserById(item.UserToId),
                                   CreatedDate = item.CreatedDate
                               });
                        }
                        else
                        {
                            models.Add(
                               new MessagePreviewViewModel
                               {
                                   Text = item2.Text,
                                   UserFrom = dataManager.Users.GetUserById(item2.UserFromId),
                                   CreatedDate = item2.CreatedDate
                               });
                        }
                    }
                }
            }
            return View(models);
        }
    }
}