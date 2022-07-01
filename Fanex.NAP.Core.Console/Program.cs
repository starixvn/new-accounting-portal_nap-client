namespace Fanex.NAP.Core.Console
{
    using System;
    using Fanex.NAP.Core.Service.Impl;

    public static class Program
    {
        public static void Main(string[] args)
        {
            var security = new SecurityService();
            security.ReCheckIn(8, 99);

            Console.ReadLine();
        }
    }
}