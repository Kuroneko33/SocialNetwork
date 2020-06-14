using BusinessLogic;
using Domain.Entities;
using Microsoft.Ajax.Utilities;
using System.Web.Mvc;
using System.Web.Security;
using Web.Models;

namespace Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private DataManager dataManager;
        public HomeController(DataManager dataManager)
        {
            this.dataManager = dataManager;
        }

        public ActionResult Index(int id = 0)
        {
            //Если в адрес Url не был представлен Id пользователя, то 
            //явным образом вычисляем этот Id и
            //перенаправляем пользователя на то же действие,
            //однако добавляем в адрес вычисленный Id
            if (id == 0)
                return RedirectToAction("Index", new { id = Membership.GetUser().ProviderUserKey });

            User user = dataManager.Users.GetUserById(id);

            //Определение страницы по Id
            UserViewModel model = new UserViewModel
            {
                User = user,
                UserIsMe = id == (int)Membership.GetUser().ProviderUserKey,
                UserIsMyFriend =
                        dataManager.Friends.UsersAreFriends(
                            (int)Membership.GetUser().ProviderUserKey, user.Id),
                FriendRequestIsSent =
                        dataManager.FriendRequests.RequestIsSent(
                            user.Id,
                            (int)Membership.GetUser().ProviderUserKey)
            };
            
            return View(model);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}