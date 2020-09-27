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
    public class SearchController : Controller
    {
        private DataManager dataManager;
        public SearchController(DataManager dataManager)
        {
            this.dataManager = dataManager;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Index(string search)
        {
            string[] words = search.Split(new char[] { ' ' });
            List<User> modelList = new List<User>();
            SearchViewModel model = new SearchViewModel();

            foreach (var word in words)
            {
                IEnumerable<User> tempModel =
                dataManager.Users.GetUsers().Where(
                    x =>
                    x.LastName.ToLowerInvariant().StartsWith(word.ToLowerInvariant()) ||
                    x.FirstName.ToLowerInvariant().StartsWith(word.ToLowerInvariant()) || 
                    x.MiddleName.ToLowerInvariant().StartsWith(word.ToLowerInvariant()));
                foreach (var item in tempModel)
                {
                    if (dataManager.Friends.UsersAreFriends((int)Membership.GetUser().ProviderUserKey, item.Id))
                    {
                        if (!model.Friends.Contains(item))
                        {
                            model.Friends.Add(item);
                        }
                    }
                    else if (dataManager.FriendRequests.RequestIsSent((int)Membership.GetUser().ProviderUserKey, item.Id))
                    {
                        if (!model.RequestsIn.Contains(item))
                        {
                            model.RequestsIn.Add(item);
                        }
                    }
                    else if (dataManager.FriendRequests.RequestIsSent(item.Id, (int)Membership.GetUser().ProviderUserKey))
                    {
                        if (!model.RequestsOut.Contains(item))
                        {
                            model.RequestsOut.Add(item);
                        }
                    }
                    else
                    {
                        if (!model.Users.Contains(item) && (int)Membership.GetUser().ProviderUserKey!=item.Id)
                        {
                            model.Users.Add(item);
                        }
                    }
                }
            }
            return View(model);
        }
    }
}