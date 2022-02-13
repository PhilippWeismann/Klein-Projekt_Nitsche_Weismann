using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop_Nitsche_Weismann; //for the menu

namespace Klein_Projekt_Nitsche_Weismann
{
    class Program
    {
        static void Main(string[] args)
        {
            // Program Flow:
            // measure Data
            // Add Data to Statistics
            // generate progress bar out of statistics




            #region testarea
            ////Test AddNewData
            //Statistics.AddNewData(10);
            //Statistics.AddNewData(20);
            //Statistics.AddNewData(15);

            ////Test IsGreaterThanAverage
            //if (Statistics.IsValueGreaterThanAverage(16))
            //{
            //    Console.WriteLine("Yes");
            //}
            //else
            //{
            //    Console.WriteLine("No");
            //}
            //if (Statistics.IsValueGreaterThanAverage(7))
            //{
            //    Console.WriteLine("Yes");
            //}
            //else
            //{
            //    Console.WriteLine("No");
            //}

            ////Test ToString()
            //Console.WriteLine(Statistics.AsString());
            //Console.ReadLine();


            Console.WriteLine("Download dauert ba mia ca 5200ms... bitte warten...");
            Console.WriteLine("Downloadtime: " + DataDownloader.DownloadTimeOfFile() + "ms");
            Console.ReadKey();
            PrintBarStringToConsole(80);
            Console.ReadKey();

            for (int i = 0; i < 5; i++)
            {
                Console.WriteLine("Downloadtime: " + DataDownloader.DownloadTimeOfFile() + "ms");
                PrintBarStringToConsole(80);
            }
            #endregion

            Mainmenu();
        }

        public static void Mainmenu()
        {
            // Main Menu
            bool exit = false;
            ConsoleMenu Mainmenu = new ConsoleMenu(new Option[]{
                new Option("Do a Speedtest", () => Speedtest()),
                new Option("Show Statistics", () => ShowStatistics()),
                new Option("Exit", () => exit = true)
            });

            //Main Loop
            do
            {
                Console.Clear();
                Console.WriteLine("Navigate with Arrow Up - Arrow Down - Keys\n");
                Mainmenu.MenuLoopInConsole();
            } while (!exit);
        }

        public static void Speedtest()
        {

        }
        public static void ShowStatistics()
        {
            Console.Clear();

            Console.WriteLine(Statistics.AsString() + "\n\n\nPress a Key to continue ...");

            Console.ReadLine();
        }

        public static void PrintBarStringToConsole(int printLength)
        {
            //Console.Write("\t");
            string barString = Statistics.BarString(printLength-2);
            int counter = 0;

            foreach (char sign in barString)
            {
                counter ++;

                if (sign == '█')
                {
                    if (counter == 1 || counter == printLength)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write(sign);
                        Console.ForegroundColor = ConsoleColor.DarkGreen;

                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.Write(sign);
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                    }
                }
                else
                {
                    Console.Write(sign);
                }

            }

            Console.ForegroundColor = ConsoleColor.White;


        }
    }
}
