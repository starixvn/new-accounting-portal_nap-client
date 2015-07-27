namespace Fanex.NAP.Core.Console
{
    using System;
    using Fanex.NAP.Core.Configuration;
    using Fanex.NAP.Core.Service.Impl;    

    public class Program
    {
        public static void Main(string[] args)
        {
            var userService = new UserService();
            var functionService = new FunctionService();
            var groupService = new GroupService();
            var maintenanceService = new MaintenanceService();
            var menuItemService = new MenuItemService();

            var user = userService.GetById(8);

            var user1 = userService.GetById(8);

            userService.UserLogOut(11);

            var functionPolicies = functionService.GetByUserAndSiteIncludeGroup(8, NapSettings.Settings.SiteId, NapSettings.Settings.Environment);

            var group = groupService.GetById(2);

            var downTime = maintenanceService.GetDowntimeInfo();

            var menuItems = menuItemService.GetByUserAndSiteIncludeGroup(8, 67);

            Console.ReadLine();
        }
    }
}
