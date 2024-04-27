using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using static System.Net.Mime.MediaTypeNames;
using System.Reflection.PortableExecutable;
using System.ComponentModel;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace todolistUpdated
{
    class Hehe
    {
        public static void Main(string[] args)
        {
            Calendar calendar = new Calendar();
            while (true)
            {
                Console.WriteLine("--Menu--");
                Console.WriteLine("Dodaj task'a [1]");
                Console.WriteLine("Edytuj task'a [2]");
                Console.WriteLine("Podgląd task'ów [3]");
                Console.WriteLine("Usuń task'a [4]");
                Console.WriteLine("Wyjdź z programu [5]");

                var inputMenu = Console.ReadLine();

                switch (inputMenu)
                {
                    case "1":
                        Console.Clear();
                        calendar.AddTask();
                        continue;

                    case "2":
                        Console.Clear();
                        calendar.EditTask();
                        break;

                    case "3":
                        Console.Clear();
                        calendar.PreviewTasks();
                        break;

                    case "4":
                        Console.Clear();
                        calendar.RemoveTask();
                        break;

                    case "5":
                        System.Environment.Exit(1);
                        break;

                    default:
                        Console.Clear();
                        break;
                }
            }
        }
    }
}
     