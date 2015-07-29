namespace Fanex.NAP.Core.Console
{
    using System;
    using Fanex.NAP.Core.Services;

    public class Program
    {
        public static void Main(string[] args)
        {
            var groupService = new GroupService();

            var group = groupService.Get(2);
            
            Console.ReadLine();
        }
    }
}
