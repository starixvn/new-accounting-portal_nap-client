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

        public const string ReCheckInScript = @"function ReCheckin() {
    var urlRecheckIn = window.rootUrl + 'Auth/ReCheckIn';
    var urlLogout = window.rootUrl + 'Auth/LogOut';
    var urlOffline = window.rootUrl + 'Auth/Offline';
    var xmlhttp = new XMLHttpRequest();
    
    xmlhttp.onreadystatechange = function () {
        if (xmlhttp.readyState === XMLHttpRequest.DONE) {   // XMLHttpRequest.DONE == 4
            var result = JSON.parse(xmlhttp.responseText);
            
            if (xmlhttp.status === 200) {
                if (result.IsKickedOut) {
                    alert('You have logged in from another session or your login session has expired!');
                    window.location.href = urlLogout;
                }
            }
            else {
                if (result.code === 999999) {
                    window.location.href = urlOffline;
                }
            }

            setTimeout(ReCheckin, 60000);
        }
    };

    xmlhttp.open('POST', urlRecheckIn, true);
    xmlhttp.setRequestHeader('X-Requested-With', 'XMLHttpRequest');
    xmlhttp.send();
}
setTimeout(ReCheckin, 60000);";

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
