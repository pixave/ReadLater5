using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ReadLater5.Controllers
{
    public class DashboardController : Controller
    {
        string _UserID;
        // GET: DashboardController
        public ActionResult Index()
        {
            if (this != null)
            {
                if (this.User != null)
                {
                    ClaimsPrincipal currentUser = this.User;
                    var currentUserData = currentUser.FindFirst(ClaimTypes.NameIdentifier);
                    if (currentUserData != null)
                    {
                        var currentUserID = currentUserData.Value;
                        if (currentUserID != null)
                        {
                            _UserID = currentUserID;
                            ViewBag.UserID = _UserID;
                        }

                    }
                }
            }
            return View();
        }

    }
}
