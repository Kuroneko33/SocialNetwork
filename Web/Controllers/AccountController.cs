using BusinessLogic;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.SessionState;
using Web.Models;

namespace Web.Controllers
{
    public class AccountController : Controller
    {
        private DataManager dataManager;
        public AccountController(DataManager dataManager)
        {
            this.dataManager = dataManager;
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult Index(LoginRegisterViewModel model)
        {
            LoginViewModel lModel = model.Login;
            if (lModel == null)
            {
                if (TempData["ViewData"] != null)
                {
                    model = TempData["Model"] as LoginRegisterViewModel;
                    ViewData = (ViewDataDictionary)TempData["ViewData"];
                }
                if (model.Register == null)
                {
                    model.Register = new RegisterViewModel();
                }
                return View(model);
            }
            
            if (ModelState.IsValid)
            {
                if (dataManager.MembershipProvider.ValidateUser(lModel.UserName, lModel.Password))
                {
                    FormsAuthentication.SetAuthCookie(lModel.UserName, false);
                    return RedirectToAction("Index", "Home");
                }
            }
            ModelState.AddModelError("", "Неудачная попытка входа на сайт");
            model.Register = new RegisterViewModel();
            model.Login = new LoginViewModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult Register(LoginRegisterViewModel model)
        {
            RegisterViewModel rModel = model.Register;
            if (ModelState.IsValid)
            {
                MembershipCreateStatus status = dataManager.MembershipProvider.CreateUser(
                    rModel.UsrName,
                    rModel.Password,
                    rModel.Email,
                    rModel.FirstName,
                    rModel.LastName,
                    rModel.MiddleName);
                if (status == MembershipCreateStatus.Success)
                    return RedirectToAction("Index", "Home");
                ModelState.AddModelError("", GetMembershipCreateStatusResultText(status));
            }
            ModelState.AddModelError("", "Неудачная попытка регистрации на сайте \nпроверьте отмеченные поля");
            TempData["Model"] = model;
            TempData["ViewData"] = ViewData;
            return RedirectToAction("Index", "Home", model);
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index");
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

        private string GetMembershipCreateStatusResultText(MembershipCreateStatus status)
        {
            if (status == MembershipCreateStatus.DuplicateEmail)
                return "Пользователь с таким email уже существует";
            if (status == MembershipCreateStatus.DuplicateUserName)
                return "Пользователь с таким именем уже существует";
            return "Неизвестная ошибка";
        }
    }
}