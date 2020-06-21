using BusinessLogic;
using Domain.Entities;
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

        public ActionResult Index()
        {
            List<IncomingMessageViewModel> models = new List<IncomingMessageViewModel>();
            foreach (IncomingMessage message in dataManager.Messages.GetIncomingMessagesByUserId(GetCurrentUserId()))
            {
                models.Add(
                    new IncomingMessageViewModel
                    {
                        Message = message,
                        UserFrom = dataManager.Users.GetUserById(message.UserFromId)
                    });
            }
            return View(models);
        }

        public ActionResult Outgoing()
        {
            List<OutgoingMessageViewModel> models = new List<OutgoingMessageViewModel>();
            foreach (OutgoingMessage message in dataManager.Messages.GetOutgoingMessagesByUserId(GetCurrentUserId()))
            {
                models.Add(
                    new OutgoingMessageViewModel
                    {
                        Message = message,
                        UserTo = dataManager.Users.GetUserById(message.UserToId)
                    });
            }
            return View(models);
        }

        public ActionResult NewMessage(int userToId)
        {
            return View(
                new OutgoingMessage
                {
                    UserId = GetCurrentUserId(),
                    UserToId = userToId,
                    CreatedDate = DateTime.Now
                });
        }

        [HttpPost]
        public ActionResult NewMessage(OutgoingMessage message)
        {
            if (ModelState.IsValid)
            {
                //после отправки исходящего создаём соответствующее входящее сообщение 
                //для другого пользователя 
                dataManager.Messages.SaveOutgoingMessage(message);
                dataManager.Messages.SaveIncomingMessage(
                    new IncomingMessage
                    {
                        UserId = message.UserToId,
                        UserFromId = message.UserId,
                        CreatedDate = DateTime.Now,
                        Text = message.Text
                    });
                return RedirectToAction("Index", "Home");
            }
            return View(message);
        }
    }
}