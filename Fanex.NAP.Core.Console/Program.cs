namespace Fanex.NAP.Core.Console
{
    using System;
    using System.Threading.Tasks;
    using Fanex.NAP.Core.Services;
    using Fanex.NAP.Core.Web.Attribute;
    using System.Threading;

    public class Program
    {
        public static void Main(string[] args)
        {
            var groupService = new GroupService();

            var group = groupService.Get(2);

            //DoAsync().ContinueWith(task =>
            //{
            //    Console.WriteLine(task.Result);
            //});

            //Console.ReadLine();

            Console.ReadLine();
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
