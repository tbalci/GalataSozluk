using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using Tuanna.Servis.Core.Ekmekcim.Models;

namespace Tuanna.Servis.Core.Ekmekcim
{
    public static class Extensions
    {
        public static T ToModel<T>(this string payload)
        {
            return JsonConvert.DeserializeObject<T>(payload);
        }

        public static string RequestMethod(this HttpRequest request)
        {
            return request.Path.Substring(1, request.Path.Length - 1).Split('/')[1];
        }

        public static string RequestPayload(this HttpRequest request)
        {
            StringBuilder sb = new StringBuilder();
            int streamLength; int streamRead;
            Stream s = request.InputStream;
            StreamReader reader = new StreamReader(s);
            string text = reader.ReadToEnd();
            return text;

            //streamLength = Convert.ToInt32(s.Length);
            // byte[] streamArray = new byte[streamLength];
            // streamRead = s.Read(streamArray, 0, streamLength);

            // for (int i = 0; i < streamLength; i++)
            // {
            //     sb.Append(Convert.ToChar(streamArray[i]));
            // }

            // return sb.ToString();
        }
    }
}