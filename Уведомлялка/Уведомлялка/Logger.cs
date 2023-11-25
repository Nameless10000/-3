using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Уведомлялка
{
    public static class Logger
    {
        public static async Task Info(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            await Console.Out.WriteAsync("[INFO]\t");
            Console.ForegroundColor = ConsoleColor.White;
            await Console.Out.WriteLineAsync(msg);
        }

        public static async Task Error(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Black;
            Console.BackgroundColor = ConsoleColor.Red;
            await Console.Out.WriteAsync("[ERROR]\t");
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Black;
            await Console.Out.WriteLineAsync(msg);
        }

        public static async Task Warning(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            await Console.Out.WriteAsync("[WARNING]\t");
            Console.ForegroundColor = ConsoleColor.White;
            await Console.Out.WriteLineAsync(msg);
        }
    }
}
