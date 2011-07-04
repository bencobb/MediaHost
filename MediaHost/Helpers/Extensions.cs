using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace MediaHost.Helpers
{
    public static class Extensions
    {
        public static T FromJsonToObj<T>(this string json)
        {
            var ms = new MemoryStream();
            ms.Write(Encoding.ASCII.GetBytes(json), 0, json.Length);
            var ser = new DataContractJsonSerializer(typeof(T));

            ms.Position = 0;
            var retval = (T)ser.ReadObject(ms);

            ms.Close();

            return retval;
        }

        public static string ToJsonFromObj<T>(this T obj)
        {
            DataContractJsonSerializer ser = null;
            Type dataType = obj.GetType();
            ser = dataType.IsGenericType ? new DataContractJsonSerializer(dataType, dataType.GetGenericArguments()) : new DataContractJsonSerializer(dataType);

            var ms = new MemoryStream();

            ser.WriteObject(ms, obj);
            ms.Position = 0;

            var streamReader = new StreamReader(ms);
            var retval = streamReader.ReadToEnd();

            streamReader.Close();
            ms.Close();

            return retval;
        }
    }
}