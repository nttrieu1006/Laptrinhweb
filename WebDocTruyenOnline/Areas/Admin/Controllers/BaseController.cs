using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using WebDocTruyenOnline.Common;
using WebDocTruyenOnline.Models;

namespace WebDocTruyenOnline.Areas.Admin.Controllers
{

    public class BaseController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var session = (UserLogin)Session[CommonConstants.USER_SESSION];
            if(session == null)
            {
                filterContext.Result = new RedirectToRouteResult(new
                    RouteValueDictionary(new { controller = "Account", action = "Login", Area = ""}));
            }
            base.OnActionExecuting(filterContext);
        }
    }
}