using System;

namespace MediaHost.Domain.Models
{
    public class Pager
    {
        #region Fields and Properties

        public int PageIndex { get; set; }

        /// <summary>If set as zero, mean pull all records</summary>
        public int PageSize { get; set; }

        public string OrderBy { get; set; }

        public bool IsAsc { get; set; }

        string _urlRef;

        public string PagingUrl
        {
            get
            {
                return _urlRef.Replace("&amp;", "&");
            }
            set
            {
                //if (System.Web.HttpContext.Current != null)
                //    _urlRef = System.Web.HttpContext.Current.Server.HtmlEncode(value);

                _urlRef = value;
            }
        }

        public string RenderToID { get; set; }

        public int TotalItems { get; set; }

        /// <summary>Get property</summary>
        public int TotalPages
        {
            get
            {
                if (this.PageSize > 0)
                    return (int)Math.Ceiling((decimal)this.TotalItems / this.PageSize);
                return 1;
            }
        }

        public int FirstResult
        {
            get
            {
                if (PageSize == 0)
                    return 0;

                return ((PageIndex - 1) * PageSize);
            }
        }

        #endregion Fields and Properties

        public Pager()
        {
            PageIndex = 1;
            PageSize = 10;
            OrderBy = string.Empty;
            IsAsc = true;
            PagingUrl = string.Empty;
        }
    }
}