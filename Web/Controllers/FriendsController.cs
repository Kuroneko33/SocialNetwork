using BusinessLogic;
using Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using Web.Models;

namespace Web.Controllers
{
    [Authorize]
    public class FriendsController : Controller
    {
        private DataManager dataManager;
        public FriendsController(DataManager dataManager)
        {
            this.dataManager = dataManager;
        }

        private int GetCurrentUserId()
        {
            return (int)Membership.GetUser().ProviderUserKey;
        }

        public ActionResult Index()
        {
            FriendsViewModel model = new FriendsViewModel();

            //Выбираем всех пользователей
            IEnumerable<User> allUsers = dataManager.Users.GetUsers();

            //Выбираем все заявки в друзья (входящие и исходящие) для пользователя, который залогинен на сайте
            IEnumerable<FriendRequest> allRequests = dataManager.FriendRequests.GetFriendRequests().Where(
                x => 
                x.UserId == GetCurrentUserId() || 
                x.PossibleFiendId == GetCurrentUserId());

            //Из всех заявок выбираем входящие
            IEnumerable<FriendRequest> incomingRequests = allRequests.Where(x => x.UserId == GetCurrentUserId());

            //Из всех заявок выбираем исходящие
            IEnumerable<FriendRequest> outgoingRequests = allRequests.Where(x => x.PossibleFiendId == GetCurrentUserId());


            //По Id из входящих заявок выбираем соответствующих User'ов и передаём их в модель
            model.IncomingRequests = 
                (
                    from aU in allUsers
                    from iR in incomingRequests
                    where aU.Id == iR.PossibleFiendId
                    select aU
                );
            //То же самое для исходящих заявок
            model.OutgoingRequests =
                (
                    from aU in allUsers
                    from oR in outgoingRequests
                    where aU.Id == oR.UserId
                    select aU
                );

            //Выбираем все записи о друзьях для пользователя, который залогинен на сайте
            IEnumerable<Friend> allFriends = dataManager.Friends.GetFriends().Where(x => x.FriendId == GetCurrentUserId());

            //По Id из записей о друзьях выбираем User'ов и передаём их в модель
            model.Friends =
                (
                    from aU in allUsers
                    from aF in allFriends
                    where aU.Id == aF.UserId
                    select aU
                );


            return View(model);
        }

        //Подача заявки
        public ActionResult AddFriendRequest(int id)
        {
            FriendRequest friendRequest = new FriendRequest
            {
                UserId = id,
                PossibleFiendId = GetCurrentUserId()
            };
            dataManager.FriendRequests.AddFriendRequest(friendRequest);

            return Redirect(System.Web.HttpContext.Current.Request.UrlReferrer.ToString());
        }

        //Отмена заявки
        public ActionResult CancelFriendRequest(int id)
        {
            dataManager.FriendRequests.DeleteFriendRequest(
                dataManager.FriendRequests.GetFriendRequests().FirstOrDefault(
                    x => x.UserId == id && x.PossibleFiendId == GetCurrentUserId()));
            return Redirect(System.Web.HttpContext.Current.Request.UrlReferrer.ToString());
        }

        //Отклонение заявки
        public ActionResult DisagreeFriendRequest(int id)
        {
            dataManager.FriendRequests.DeleteFriendRequest(
                dataManager.FriendRequests.GetFriendRequests().FirstOrDefault(
                    x => x.UserId == GetCurrentUserId() && x.PossibleFiendId == id));

            return Redirect(System.Web.HttpContext.Current.Request.UrlReferrer.ToString());
        }

        //Принятие заявки
        public ActionResult ConfirmFriendRequest(int id)
        {
            //Создаём ддублиирующую запись для обоих пользователей
            Friend friend1 = new Friend { UserId = GetCurrentUserId(), FriendId = id };
            Friend friend2 = new Friend { UserId = id, FriendId = GetCurrentUserId() };
            dataManager.Friends.AddFriend(friend1);
            dataManager.Friends.AddFriend(friend2);

            //Удаляем соответствующую заявку в друзья
            dataManager.FriendRequests.DeleteFriendRequest(
                dataManager.FriendRequests.GetFriendRequests().FirstOrDefault(
                        x => 
                        (x.UserId == id && x.PossibleFiendId == GetCurrentUserId()) ||
                        (x.UserId == GetCurrentUserId() && x.PossibleFiendId == id)
                    )
                );

            return Redirect(System.Web.HttpContext.Current.Request.UrlReferrer.ToString());
        }

        //Удаление из друзей
        public ActionResult DeleteFriend(int id)
        {
            //Удаление соответствующих записей у обоих пользователей
            dataManager.Friends.DeleteFriend(
                dataManager.Friends.GetFriends().FirstOrDefault(
                    x => x.UserId == id && x.FriendId == GetCurrentUserId()));
            dataManager.Friends.DeleteFriend(
                dataManager.Friends.GetFriends().FirstOrDefault(
                    x => x.UserId == GetCurrentUserId() && x.FriendId == id));

            return Redirect(System.Web.HttpContext.Current.Request.UrlReferrer.ToString());
        }
    }
}