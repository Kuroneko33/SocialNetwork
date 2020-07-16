using BusinessLogic;
using Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

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

        [HttpPost]
        public ActionResult Index(string search)
        {
            //переделать
            string[] words = search.Split(new char[] { ' ' });
            List<User> modelList = new List<User>();

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
                    if (!modelList.Contains(item))
                    {
                        modelList.Add(item);
                    }
                }
            }

            IEnumerable<User> model = modelList;

            return View(model);
        }
    }
}