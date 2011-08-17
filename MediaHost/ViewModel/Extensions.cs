using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MediaHost.ViewModel
{
    public static class Extensions
    {
        private static IEnumerable<SelectListItem> SelectList_Default
        {
            get
            {
                List<SelectListItem> retval = new List<SelectListItem>();
                retval.Add(new SelectListItem { Value = "0", Text = "-- Select --" });

                return retval;
            }
        }

        public static IEnumerable<SelectListItem> AddDefault(this IEnumerable<SelectListItem> list)
        {
            return SelectList_Default.Union(list);
        }
    }
}