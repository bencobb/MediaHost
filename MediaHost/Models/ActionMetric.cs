using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MediaHost.Models
{
    public class ActionMetric
    {
        public string ActionName { get; set; }
        public string ControllerName { get; set; }
        public long TotalRequests { get; set; }
        public int AverageLatency { get; set; }
        public int TotalExceptions { get; set; }
    }
}