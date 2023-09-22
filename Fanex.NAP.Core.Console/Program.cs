namespace Fanex.NAP.Core.Console
{
    using System;
    using Fanex.NAP.Core.Service.Impl;

    public static class Program
    {
        public static void Main(string[] args)
        {
            var userService = new UserService();
            var users = userService.GetBySite(1, null);

            Console.ReadLine();
        }
    }
}