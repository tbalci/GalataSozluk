using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tuanna.Servis.Core.Ekmekcim
{
    public class RequestHelper
    {
        public static string InvokeAndSerialize(string command, string payload)
        {
            return JsonConvert.SerializeObject(Invoker.Instance.InvokeMethod(command, payload));
        }

        public static string InvokeAndSerialize(string command)
        {
            return JsonConvert.SerializeObject(Invoker.Instance.InvokeMethod(command));
        }

        public static string Error(Exception ex)
        {
            object err = new { Ok = false, Message = ex.Message };
            return JsonConvert.SerializeObject(err);
        }
    }
}