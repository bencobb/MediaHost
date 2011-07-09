using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MediaHost.Models;

namespace MediaHost.Helpers
{
    public static class PerformanceCounter
    {
        private static Dictionary<string, PerformanceMetric> _metrics = new Dictionary<string, PerformanceMetric>();
        public static Dictionary<string, PerformanceMetric> Metrics
        {
            get { return _metrics; }
        }

        public static List<ActionMetric> GetActionMetrics()
        {
            var retval = new List<ActionMetric>();

            foreach(string key in _metrics.Keys)
            {
                var performanceMetric = _metrics[key];
                var actionMetric = new ActionMetric
                                        {
                                            ActionName = performanceMetric.Action, 
                                            AverageLatency = performanceMetric.AverageLatency,
                                            ControllerName = performanceMetric.Controller,
                                            TotalRequests = performanceMetric.TotalRequests,
                                            TotalExceptions = performanceMetric.TotalExceptions
                                        };

                retval.Add(actionMetric);
            }

            return retval;
        }

        public static void Log(string controller, string action, string username, string ip, long latencyMilliseconds, bool isException)
        {
            PerformanceMetric metric = GetMetric(controller, action);

            string userKey = string.Format("{0}:{1}", username, ip);
            metric.LogRequest(userKey);
            metric.LogLatency(userKey, latencyMilliseconds);

            if(isException)
            {
                metric.LogException(userKey);
            }
        }

        private static PerformanceMetric GetMetric(string controller, string action)
        {
            string counterKey = controller + ":" + action;
            PerformanceMetric metric;
            
            if(_metrics.ContainsKey(counterKey))
            {
                metric = _metrics[counterKey];
            }
            else
            {
                metric = new PerformanceMetric(controller, action);
                _metrics.Add(counterKey, metric);
            }
            return metric;
        }
    }

    public class PerformanceMetric
    {
        private Dictionary<string, long> _requests;
        private Dictionary<string, long> _latency;
        private Dictionary<string, int> _exceptions;

        public string Controller { get; set; }
        public string Action { get; set; }
        public int AverageLatency
        {
            get
            {
                if(TotalRequests > 0)
                {
                    return Convert.ToInt32(_latency.Values.Sum() / TotalRequests);
                }

                return 0;
            }
        }

        public long TotalRequests
        {
            get { return _requests.Values.Sum(); }
        }

        public int TotalExceptions
        {
            get { return _exceptions.Values.Sum(); }
        }

        public PerformanceMetric(string controller, string action)
        {
            Controller = controller;
            Action = action;

            _requests = new Dictionary<string, long>();
            _latency = new Dictionary<string, long>();
            _exceptions = new Dictionary<string, int>();
        }

        public void LogRequest(string userKey)
        {
            if(_requests.ContainsKey(userKey))
            {
                _requests[userKey]++;
            }
            else
            {
                _requests.Add(userKey, 1);
            }
        }

        public void LogLatency(string userKey, long milliseconds)
        {
            if (_latency.ContainsKey(userKey))
            {
                _latency[userKey] += milliseconds;
            }
            else
            {
                _latency.Add(userKey, milliseconds);
            }
        }

        public void LogException(string userKey)
        {
            if (_exceptions.ContainsKey(userKey))
            {
                _exceptions[userKey] ++;
            }
            else
            {
                _exceptions.Add(userKey, 1);
            }
        }
    }
}