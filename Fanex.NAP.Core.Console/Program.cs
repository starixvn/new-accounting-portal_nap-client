namespace Fanex.NAP.Core.Console
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Configuration;
    using Fanex.NAP.Core.Service.Impl;
    using Models.ParameterObject;

    public class Program
    {
        public static void Main(string[] args)
        {
            var groupService = new GroupService();
            var userService = new UserService();

            var functionPolicies = userService.GetPermissions(633564, 12, 662);
            //var user = userService.Get("sa");

            //var user = (new SecurityService()).Login(new LoginParameter
            //{
            //    UserId = 8,
            //    SiteId = 98,
            //    LoginDate = DateTime.Today
            //});

            //var onuserid = (new SecurityService()).SSOLogin(new SSOLoginParameter
            //{
            //    CheckOnUserId = 1,
            //    SiteId = 12,
            //    LoginDate = DateTime.Today
            //});
            //var permission = userService.GetPermission(8, 15);
            //var menuItem = (new SiteService()).GetMenuItemBySiteId(12, "UAT");
            //var result = UpdatePassword();
            //var result1 = UpdateProfile();

            Console.ReadLine();
        }

        private static bool UpdateProfile()
        {
            var parameter = new UpdateUserInfoParameter
            {
                CustId = 8,
                FirstName = "Test215",
                LastName = "Test215"
            };

            var result = (new SecurityService()).UpdateProfile(NapSettings.Settings.SiteId, parameter);

            return result;
        }

        private static bool UpdatePassword()
        {
            var result = (new SecurityService()).UpdatePassword(NapSettings.Settings.SiteId, 1, "56E8A2D78170E87C928E480797CC73");
            return result;
        }

        public static Task<string> DoAsync()
        {
            return Task.Factory.StartNew(() =>
            {
                Thread.Sleep(5000);
                return "Hello world!";
            });
        }
    }
}