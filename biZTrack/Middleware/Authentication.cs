using System;
using System.Linq;
using System.Web.Mvc;
using System.Net;
using biZTrack.Interfaces;
using biZTrack.DataAccess;
using biZTrack.Static;

namespace biZTrack.Middleware
{
    public class Authentication : ActionFilterAttribute
    {
        private IAuthenticate IAuthenticate = null;
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            //Log("OnActionExecuting", filterContext.RouteData);
            authenticate(filterContext);
        }

        public void OnException(ExceptionContext filterContext)
        {
            string message = filterContext.RouteData.Values["controller"].ToString() + " -> " +
               filterContext.RouteData.Values["action"].ToString() + " -> " +
               filterContext.Exception.Message + " \t- " + DateTime.Now.ToString() + "\n";
        }

        public bool authenticate(ActionExecutingContext filterContext)
        {
            var headers = filterContext.HttpContext.Request.Headers;
            string[] keys = headers.AllKeys;

            if (filterContext.HttpContext.Request.RequestType != "OPTIONS")
            {
                if (keys.Contains("MySecretToken"))
                {
                    string authKey = headers.GetValues("MySecretToken").First();
                    var decryptedString = StaticClass.DecryptStringAES(authKey);
                    IAuthenticate = new DAAuthenticate();
                    if (authKey != "null")
                    {
                        if (IAuthenticate.Authenticate(decryptedString))
                        {
                            return true;
                        }
                        else
                        {
                            filterContext.Result = new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
                            return false;
                        }
                    }
                    else
                    {
                        filterContext.Result = new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
                        return false;
                    }
                }
                else
                {
                    filterContext.Result = new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
                    return false;
                }
            }
            else
            {
                JsonResult Res = new JsonResult();
                Res.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
                filterContext.Result = Res;
                return false;
            }


        }
        
    }
}
