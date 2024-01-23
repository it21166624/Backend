using biZTrack.DataAccess;
using biZTrack.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace biZTrack.Controllers
{
    public class BudgetShopController : Controller
    {
        IBudgetShop IBudgetShop = null;

        [HttpGet]
        public ActionResult GetBudgetShopPriceList()
        {
            IBudgetShop = new DABudgetShop();
            return Json(IBudgetShop.GetBudgetShopPriceList(), JsonRequestBehavior.AllowGet);
        }
    }
}