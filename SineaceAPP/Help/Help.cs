using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SineaceAPP.Help
{
    public static class Help
    {
        public static string GetParameter(this string uri, string key)
        {
            var parameters = HttpUtility.ParseQueryString(uri);
            return parameters[key];
        }
    }
}
