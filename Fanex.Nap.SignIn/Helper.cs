namespace Fanex.Nap.SignIn
{
    using System;
    using System.Configuration;
    using System.Web;

    public static class Helper
    {
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
            if (IsGlobalAbsoluteUrl(redirectUrl))
            {
                return redirectUrl;
            }

            var rootUrl = GetRootUrl();
            var localAbsoluteUrl = GetLocalAbsoluteUrl(redirectUrl);

            return string.IsNullOrEmpty(rootUrl)
                ? VirtualPathUtility.Combine(VirtualPathUtility.ToAbsolute("~/"), localAbsoluteUrl)
                : CombineUrl(rootUrl, localAbsoluteUrl);
        }

        public static string GetLocalAbsoluteUrl(string redirectUrl)
        {
            var url = string.IsNullOrEmpty(redirectUrl) ? VirtualPathUtility.ToAbsolute("~/") : redirectUrl.Trim();

            return "/" + url.TrimStart('/');
        }

        public static string GetHomeUrl()
        {
            var rootUrl = GetRootUrl();
            return string.IsNullOrEmpty(rootUrl)
                ? VirtualPathUtility.ToAbsolute("~/")
                : CombineUrl(rootUrl, VirtualPathUtility.ToAbsolute("~/"));
        }

        private static string GetRootUrl()
        {
            var rootUrl = ConfigurationManager.AppSettings["RootUrl"] ?? string.Empty;

            return rootUrl.TrimEnd('/');
        }

        public static string GetPropagationAuthUrl()
        {
            var propagationAuth = ConfigurationManager.AppSettings["PropagationAuth"] ?? string.Empty;

            propagationAuth = propagationAuth.Trim().TrimEnd('/');

            if (string.IsNullOrEmpty(propagationAuth) || IsLocalAbsoluteUrl(propagationAuth))
            {
                return propagationAuth;
            }

            return IsVirtualPath(propagationAuth)
                ? VirtualPathUtility.ToAbsolute(propagationAuth)
                : propagationAuth;
        }

        private static bool IsLocalAbsoluteUrl(string url)
        {
            return url.StartsWith("/");
        }

        private static bool IsGlobalAbsoluteUrl(string url)
        {
            return Uri.TryCreate(url, UriKind.Absolute, out _);
        }

        private static bool IsVirtualPath(string url)
        {
            return url.StartsWith("~");
        }

        private static string CombineUrl(string baseUrl, string relativeUrl)
        {
            return new Uri(new Uri(baseUrl), relativeUrl).AbsoluteUri;
        }
    }
}
