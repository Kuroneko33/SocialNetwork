using BusinessLogic;
using Domain.Entities;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Web.Models;

namespace Web.Controllers
{
    [Authorize]
    public class MessagesController : Controller
    {
        private DataManager dataManager;
        public MessagesController(DataManager dataManager)
        {
            this.dataManager = dataManager;
        }

        private int GetCurrentUserId()
        {
            return (int)Membership.GetUser().ProviderUserKey;
        }

        public ActionResult Index(int id = 0)
        {
            if (id == 0)
            {
                if (TempData["id"] != null)
                {
                    id = (int)TempData["id"];
                }
            }
            List <MessagesViewModel> models = new List<MessagesViewModel>();
            
            foreach (IncomingMessage message in dataManager.Messages.GetIncomingMessagesByUserId(GetCurrentUserId()))
            {
                if (message.UserFromId == id)
                {
                    models.Add(
                        new MessagesViewModel
                        {
                            Id = message.Id,
                            UserFromId = message.UserFromId,
                            UserToId = message.UserId,
                            Text = message.Text,
                            CreatedDate = message.CreatedDate,
                            UserFrom = dataManager.Users.GetUserById(message.UserFromId),
                        }
                        );
                }
            }
            foreach (OutgoingMessage message in dataManager.Messages.GetOutgoingMessagesByUserId(GetCurrentUserId()))
            {
                if (message.UserToId == id)
                {
                    models.Add(
                        new MessagesViewModel
                        {
                            Id = message.Id,
                            UserFromId = message.UserId,
                            UserToId = message.UserToId,
                            Text = message.Text,
                            CreatedDate = message.CreatedDate,
                            UserFrom = dataManager.Users.GetUserById(message.UserId),
                        }
                        ); 
                }
            }
            models.Sort();
            models.Insert(0,
                new MessagesViewModel
                {
                    UserToId = id,
                    UserFromId = GetCurrentUserId(),
                    UserFrom = new User() { Id = -1 }
                });
            TempData["id"] = id;
            return View(models);

        }

        [HttpPost]
        public ActionResult NewMessage(List<MessagesViewModel> messages)
        {
            MessagesViewModel message = messages[0];
            if (message.Text != null)
            {
                dataManager.Messages.SaveOutgoingMessage(
                    new OutgoingMessage
                    {
                        UserId = message.UserFromId,
                        UserToId = message.UserToId,
                        CreatedDate = DateTime.UtcNow,
                        Text = message.Text
                    });
                dataManager.Messages.SaveIncomingMessage(
                    new IncomingMessage
                    {
                        UserId = message.UserToId,
                        UserFromId = message.UserFromId,
                        CreatedDate = DateTime.UtcNow,
                        Text = message.Text
                    });
                TempData["id"] = message.UserToId;
                return RedirectToAction("Index", "Messages");
            }
            return RedirectToAction("Index", "Messages");
        }
    }
}