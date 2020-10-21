using System;

namespace F.Economy
{
    public class Logger
    {
        public static void Log(string message, params object[] args)
        {
            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] FEconomy >> {message}", args);
        }
    }
}

