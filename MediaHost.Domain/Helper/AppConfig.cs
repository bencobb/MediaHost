using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Configuration;

namespace MediaHost.Domain.Helper
{
    public class AppConfig
    {
        public static string AWS_Id
        {
            get
            {
                return ConfigurationManager.AppSettings["AWS_Id"];
            }
        }

        public static string AWS_Secret
        {
            get
            {
                return ConfigurationManager.AppSettings["AWS_Secret"];
            }
        }

        public static string AWS_Bucket
        {
            get
            {
                return ConfigurationManager.AppSettings["AWS_Bucket"];
            }
        }

        public static string AWS_BucketStreaming
        {
            get
            {
                return ConfigurationManager.AppSettings["AWS_BucketStreaming"];
            }
        }

        public static string StreamingServer
        {
            get
            {
                return ConfigurationManager.AppSettings["StreamingServer"];
            }
        }
    }
}
