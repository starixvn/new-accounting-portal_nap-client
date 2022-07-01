using System;
using System.Configuration;
using System.Web;

namespace Fanex.NAP.Core.SignIn
{
    public static class Helper
    {
        public static readonly string RootUrl = GetRootUrl();

        public static readonly string HomeUrl = GetHomeUrl();

        public static readonly string PropagationAuthUrl = GetPropagationAuthUrl();


        public static string GetAbsoluteUrl(string redirectUrl)
        {
            var url = GetLocalAbsoluteUrl(redirectUrl);

            return string.IsNullOrEmpty(RootUrl)
                ? url
                : CombineUrl(RootUrl, url);
        }

        private static string GetLocalAbsoluteUrl(string redirectUrl)
        {
            var url = string.IsNullOrEmpty(redirectUrl) ? "~/" : redirectUrl.Trim();

            if (IsAbsoluteUrl(url))
            {
                return url;
            }

            return IsVirtualPath(url)
               ? VirtualPathUtility.ToAbsolute(url)
               : url;
        }

        private static string GetHomeUrl()
        {
            var homeUrl = VirtualPathUtility.ToAbsolute("~/");

            return string.IsNullOrEmpty(RootUrl) ? homeUrl : CombineUrl(RootUrl, homeUrl);
        }

        private static string GetRootUrl()
        {
            var rootUrl = ConfigurationManager.AppSettings["RootUrl"] ?? string.Empty;

            return rootUrl.TrimEnd('/');
        }

        private static string GetPropagationAuthUrl()
        {
            var propagationAuth = ConfigurationManager.AppSettings["PropagationAuth"] ?? string.Empty;

            propagationAuth = propagationAuth.Trim().TrimEnd('/');

            if (string.IsNullOrEmpty(propagationAuth) || IsAbsoluteUrl(propagationAuth))
            {
                return propagationAuth;
            }

            return IsVirtualPath(propagationAuth)
                ? VirtualPathUtility.ToAbsolute(propagationAuth)
                : propagationAuth;
        }

        private static bool IsAbsoluteUrl(string url)
        {
            return url.StartsWith("/")
                || Uri.TryCreate(url, UriKind.Absolute, out _);
        }

        private static bool IsVirtualPath(string url)
        {
            return url.StartsWith("~/");
        }

        private static string CombineUrl(string baseUrl, string relativeUrl)
        {
            return new Uri(new Uri(baseUrl), relativeUrl).AbsoluteUri;
        }
    }
}
