using BusinessLogic;
using Domain.Entities;
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

        public ActionResult Index()
        {
            int id = (int)Membership.GetUser().ProviderUserKey;
            User user = dataManager.Users.GetUserById(id);

            EditViewModel model = new EditViewModel();
            if (TempData["ViewData"] != null)
            {
                model = TempData["Model"] as EditViewModel;
                ViewData = (ViewDataDictionary)TempData["ViewData"];
            }
            else
            {
                //тут перевод инфы юзеров в этот формат из user.BDate
                //int BDay = 0;
                //string BMonth = "август";
                //int BYear = 1999;

                model = new EditViewModel
                {
                    UserName = user.UserName,
                    Password = user.Password,
                    ConfirmPassword = user.Password,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    MiddleName = user.MiddleName,
                    //BDay = BDay,
                    //BMonth = BMonth,
                    //BYear = BYear,
                    //Phone = user.Phone,
                    //City = user.City
                };
            }

            return View(model);
        }

        [HttpGet]
        public ActionResult Save(EditViewModel model, int BYear, string BMonth, int BDay)
        {
            int id = (int)Membership.GetUser().ProviderUserKey;
            User user = dataManager.Users.GetUserById(id);

            TempData["Model"] = model;
            TempData["ViewData"] = ViewData;

            if (ModelState.IsValid)
            {
                model.BYear = BYear;
                model.BMonth = BMonth;
                model.BDay = BDay;

                user.UserName = model.UserName;
                user.Password = model.Password;
                user.Email = model.Email;
                user.FirstName = model.FirstName;
                user.MiddleName = model.MiddleName;
                //user.BDate = посчитать;
                //user.Phone = model.Phone;
                //user.City = model.City;

                dataManager.Users.SaveUser(user);
                ModelState.AddModelError("", "Сохранение прошло успешно");
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", "Не все обязательные параметры введены верно");
            return RedirectToAction("Index");
        }
    }
}