namespace Fanex.NAP.Core.Console
{
    using Fanex.NAP.Core.Service.Impl;
    using Models.ParameterObject;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public class Program
    {
        public static void Main(string[] args)
        {
            var groupService = new GroupService();
            var userService = new UserService();

            var user = userService.Get(8);
            var menuItem = (new SiteService()).GetMenuItemBySiteId(12, "UAT");
            var result = UpdatePassword();
            result = UpdateProfile();

            Console.ReadLine();
        }

        private static bool UpdateProfile()
        {
            var parameter = new UpdateUserInfoParameter
            {
                CustId = 1081978,
                FirstName = "Test",
                LastName = "Test"
            };

            var result = (new SecurityService()).UpdateProfile(parameter);

            return result;
        }

        private static bool UpdatePassword()
        {
            var result = (new SecurityService()).UpdatePassword(8, "56E8A2D78170E87C928E480797CC73");
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