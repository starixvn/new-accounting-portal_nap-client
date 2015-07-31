namespace Fanex.NAP.Core.Console
{
    using System;
    using Fanex.NAP.Core.Services;
    using Fanex.NAP.Core.Web.Attribute;

    public class Program
    {
        [NapAuth(FunctionKey = "", OutputCache = true)]
        public static void Main(string[] args)
        {
            var groupService = new GroupService();

            var group = groupService.Get(2);

            Console.ReadLine();
        }
    }
}
