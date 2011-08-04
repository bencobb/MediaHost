using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Timers;
using System.Web;
using System.Web.Mvc;
using MediaHost.Helpers;
using PerformanceCounter = MediaHost.Helpers.PerformanceCounter;

namespace MediaHost.Controllers
{
    public sealed class PerformanceAttribute : FilterAttribute, IActionFilter, IResultFilter, IExceptionFilter
    {
        private string _actionName;
        private string _controllerName;
        private string _userName;
        private string _ip;
        private Stopwatch _stopWatch;
        private string _busyKey;

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            
            _stopWatch = Stopwatch.StartNew();

            _actionName = filterContext.ActionDescriptor.ActionName;
            _controllerName = filterContext.Controller.GetType().Name;
            
            _userName = "temp";
            _ip = filterContext.HttpContext.Request.UserHostAddress;

            _busyKey = Guid.NewGuid().ToString("N");
            PerformanceCounter.LogBusyQueue(_busyKey, _actionName, _controllerName, _userName);
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            
        }

        public void OnResultExecuting(ResultExecutingContext filterContext)
        {
            
        }

        public void OnResultExecuted(ResultExecutedContext filterContext)
        {
            long latencyMilliseconds = _stopWatch.ElapsedMilliseconds;

            PerformanceCounter.RemoveFromBusyQueue(_busyKey);
            PerformanceCounter.Log(_controllerName, _actionName, _userName, _ip, latencyMilliseconds, false);
        }

        public void OnException(ExceptionContext filterContext)
        {
            long latencyMilliseconds = _stopWatch.ElapsedMilliseconds;

            PerformanceCounter.Log(_controllerName, _actionName, _userName, _ip, latencyMilliseconds, true);
        }
    }
}