using System;
using System.Configuration;
using System.Web;
using System.Web.Security;

namespace Fanex.NAP.Core.SignIn
{
    public static class Helper
    {
        private static char[] trimChars = new[] { '~', '/' };

        public static string GetAbsoluteRedirectUrl(HttpRequestBase request, string redirectUrl)
        {
            if (string.IsNullOrEmpty(redirectUrl))
            {
                redirectUrl = CombineUrl(GetAppUrl(request), FormsAuthentication.DefaultUrl);
            }

            if (!IsAbsoluteUrl(redirectUrl))
            {
                redirectUrl = CombineUrl(GetAppUrl(request), redirectUrl);
            }

            return redirectUrl;
        }

        private static bool IsAbsoluteUrl(string url)
        {
            Uri result;
            return Uri.TryCreate(url, UriKind.Absolute, out result);
        }

        private static string GetAppUrl(HttpRequestBase request)
        {
            if (string.IsNullOrEmpty(ConfigurationManager.AppSettings["RootUrl"]))
            {
                return request.Url.GetLeftPart(UriPartial.Authority) + VirtualPathUtility.ToAbsolute("~/");
            }

            return CombineUrl(ConfigurationManager.AppSettings["RootUrl"], request.ApplicationPath);
        }

        private static string CombineUrl(string baseUrl, string relativeUrl)
        {
            return new Uri(new Uri(baseUrl), relativeUrl).AbsoluteUri;
        }
    }
}
