using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MediaHost.Helpers;

namespace MediaHost.Controllers
{
    public class MetricsController : BaseController
    {
        //
        // GET: /Metrics/

        public ActionResult Index()
        {
            return View();
        }

    }
}
