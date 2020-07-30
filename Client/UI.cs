using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public static class UI
    {
        public static List<string> names = new List<string>();

        public static void DisplayMessage(string message)
        {
            int colorIndex = ParseName(message);
            SetColor(colorIndex);
            Console.WriteLine(message);
            Console.ResetColor();
        }
        public static string GetInput()
        {
            return Console.ReadLine();
        }
        public static int ParseName(string message)
        {
            int index = message.IndexOf(':');
            if(index == -1)
            {
                return -1;
            }
            string name = message.Substring(0, index);
            if(names.Contains(name))
            {
                return names.IndexOf(name);
            }
            else
            {
                names.Add(name);
                return names.IndexOf(name);
            }
        }
        public static void SetColor(int colorIndex)
        {
            if(colorIndex == -1)
            {
                Console.BackgroundColor = ConsoleColor.White;
                Console.ForegroundColor = ConsoleColor.Black;
                return;
            }
            switch(colorIndex % 6)
            {
                case 0:
                    Console.ForegroundColor = ConsoleColor.Blue;
                    break;
                case 1:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case 2:
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
                case 3:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case 4:
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    break;
                case 5:
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    break;
                default:
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.ForegroundColor = ConsoleColor.Black;
                    break;
            }
        }

    }
}
