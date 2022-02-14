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
            Settings();
            Mainmenu();
        }

        public static void Mainmenu()
        {
            // Main Menu
            bool exit = false;
            ConsoleMenu Mainmenu = new ConsoleMenu(new Option[]{
                new Option("Do a Speedtest", () => Speedtest()),
                new Option("Show Statistics", () => ShowStatistics()),
                new Option("Reset Statistics", () => Statistics.ResetStatitic()),
                new Option("Exit", () => exit = true)
            });

            //Main Loop
            do
            {
                Console.Clear();
                Console.WriteLine("Internet / Network Speedtest\n\nNavigate with Arrow Up - Arrow Down - Keys\n");
                Mainmenu.MenuLoopInConsole();
            } while (!exit);
        }

        public static void Speedtest()
        {
            Console.Clear();

            Console.WriteLine("Please wait... Speedtest is running...");

            long downloadTime = DataDownloader.DownloadTimeOfFile();

            if (downloadTime > 0)
            {
                Console.Clear();
                Console.WriteLine("Your current Network-Speed is: " + String.Format("{0:0.00}", Statistics.ConvertDownloadTimeToMbitperSecond(downloadTime)) + " Mbit/s ... [Downloadtime: " + downloadTime + " ms - Filesize: 1000000 Byte]\n");
                PrintBarStringToConsole(80);
                Console.WriteLine("\n\nPress any Key to return to Main Menu...");
            }
            else
            {
                Console.WriteLine("An Error occured during the Speed Test. Please try again.");
            }



            Console.ReadKey();
        }
        public static void ShowStatistics()
        {
            Console.Clear();

            Console.WriteLine(Statistics.AsString() + "\n\n\nPress any Key to return to Main Menu...");

            Console.ReadLine();
        }

        public static void PrintBarStringToConsole(int printLength)
        {
            string barString = Statistics.BarString(printLength);
            int counter = 0;

            foreach (char sign in barString)
            {

                if (sign == '█')
                {
                    counter++;

                    if (counter == 1 || counter == 3)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write(sign);
                        Console.ForegroundColor = ConsoleColor.White;

                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.Write(sign);
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                }
                else if (sign == '░')
                {
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.Write(sign);
                    Console.ForegroundColor = ConsoleColor.White;
                }
                else if (sign == '▓')
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write(sign);
                    Console.ForegroundColor = ConsoleColor.White;
                }
                else
                {
                    Console.Write(sign);
                }

            }

            Console.ForegroundColor = ConsoleColor.White;


        }
        public static void Settings()
        {
            //Setting to Print €-Sign
            Console.OutputEncoding = Encoding.Default;
            Console.OutputEncoding = Encoding.UTF8;

            //Console Settings
            Console.Title = "Network/Internet - SpeedTest | © Simon Nitsche & Philipp Weismann";
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.SetWindowSize(140, 20);
        }// Settings for Console-Appearance
    }
}
