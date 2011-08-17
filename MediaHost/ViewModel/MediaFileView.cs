using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using MediaHost.Domain.Models;
using System.Web.Mvc;

namespace MediaHost.ViewModel
{
    public class MediaFileView
    {
        public class Add
        {
            public long EntityId { get; set; }
            public IEnumerable<Playlist> Playlists { get; set; }
            public IEnumerable<SelectListItem> Playlists_ListItem
            {
                get
                {
                    var retval = from a in Playlists
                                 select new SelectListItem
                                 {
                                     Text = a.Name,
                                     Value = a.Id.ToString()
                                 };

                    return retval.AddDefault();
                }
            }
        }
    }
}