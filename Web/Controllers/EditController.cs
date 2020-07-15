using BusinessLogic;
using Domain.Entities;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Web.Models;

namespace Web.Controllers
{
    [Authorize]
    public class EditController : Controller
    {
        private DataManager dataManager;
        public EditController(DataManager dataManager)
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
            if (model.UserIsMe)
            {
                return View(model);
            }
            else
                return RedirectToAction("Index", "Home");
        }
    }
}