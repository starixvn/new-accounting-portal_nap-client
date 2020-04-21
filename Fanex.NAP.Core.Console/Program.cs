namespace Fanex.NAP.Core.Console
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Fanex.NAP.Core.Service.Impl;

    public class Program
    {
        public static void Main(string[] args)
        {
            while (true)
            {
                var thread = new Thread(DoAsync);
                thread.Start();

                Thread.Sleep(500);
            }

            Console.ReadLine();
        }


        public static void DoAsync()
        {
            for (var i = 0; i < 20; i++)
            {
                new UserService().GetUserPolicies("ManageUser", 1).ToArray();
                //using (var service = new UserService())
                //{
                //    service.GetUserPolicies("ManageUser", 1).ToArray();
                //}
            }
        }
    }
}