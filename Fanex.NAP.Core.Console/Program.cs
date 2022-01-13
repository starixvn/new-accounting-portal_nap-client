namespace Fanex.NAP.Core.Console
{
    using System;
    using System.Linq;
    using System.Threading;
    using Fanex.NAP.Core.Service.Impl;
    using Newtonsoft.Json;

    public class Program
    {
        public static void Main(string[] args)
        {
            var menus = new UserService().GetUserLevel(8, 99);
            
            
            
            while (true)
            {
                //var thread = new Thread(DoAsync);
                //thread.Start();
                DoAsync();
                Thread.Sleep(500);
                GC.Collect();
            }

            Console.ReadLine();
        }


        public static void DoAsync()
        {
            var count = 0;
            for (var i = 0; i < 100; i++)
            {
                
                //using (var service = new UserService())
                //{
                //    service.GetUserPolicies("ManageUser", 1).ToArray();
                //}
            }
            Console.WriteLine(count);
        }
    }
}