namespace Fanex.NAP.Core.Console
{
    using System;
    using System.Linq;
    using System.Threading;
    using Fanex.NAP.Core.Service.Impl;

    public class Program
    {
        public static void Main(string[] args)
        {
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
                count += new UserService().GetUserPolicies("ManageUser", 1).Count();
                //using (var service = new UserService())
                //{
                //    service.GetUserPolicies("ManageUser", 1).ToArray();
                //}
            }
            Console.WriteLine(count);
        }
    }
}