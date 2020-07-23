using BusinessLogic;
using Domain.Entities;
using System;
using System.IO;
using System.Web;
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
            { 
                try
                {
                    if (Membership.GetUser() == null)
                        throw new Exception();
                    int userId = (int)Membership.GetUser().ProviderUserKey;
                    return RedirectToAction("Index", new { id = userId });
                }
                catch (Exception)
                {
                    FormsAuthentication.SignOut();
                    return RedirectToAction("Index", "Account");
                }
            }
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
                            (int)Membership.GetUser().ProviderUserKey),
                FriendRequestIsSentToMe =
                        dataManager.FriendRequests.RequestIsSent(
                        (int)Membership.GetUser().ProviderUserKey,
                        user.Id)
            };
            
            return View(model);
        }

        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase upload)
        {
            if (ModelState.IsValid)
            {
                if (upload != null)
                {
                    int id = (int)Membership.GetUser().ProviderUserKey;

                    string type = upload.ContentType;
                    type = type.Substring(type.LastIndexOf("/") + 1);
                    string fileName = "AvatarUserId" + id + "." + type;

                    User user = dataManager.Users.GetUserById(id);

                    string oldPhoto = user.Photo;
                    if (oldPhoto != null)
                    {
                        FileInfo fileInf = new FileInfo(Server.MapPath(oldPhoto));
                        if (fileInf.Exists)
                        {
                            fileInf.Delete();
                        }
                    }
                    upload.SaveAs(Server.MapPath("~/Content/images/usersAvatars/" + fileName));
                    user.Photo = "~/Content/images/usersAvatars/" + fileName;
                    dataManager.Users.SaveUser(user);
                }
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Index");
            }
        }
        [HttpPost]
        public ActionResult Delete()
        {
            if (ModelState.IsValid)
            {
                    int id = (int)Membership.GetUser().ProviderUserKey;
                    User user = dataManager.Users.GetUserById(id);

                    string oldPhoto = user.Photo;
                    if (oldPhoto != null)
                    {
                        FileInfo fileInf = new FileInfo(Server.MapPath(oldPhoto));
                        if (fileInf.Exists)
                        {
                            fileInf.Delete();
                        }
                    }
                    user.Photo = null;
                    dataManager.Users.SaveUser(user);
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Index");
            }
        }
    }
}