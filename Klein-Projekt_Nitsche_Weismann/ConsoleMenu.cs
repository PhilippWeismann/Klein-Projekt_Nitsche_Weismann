using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webshop_Nitsche_Weismann
{
    class ConsoleMenu
    {
        #region Members
        Option[] _options;
        #endregion

        #region Konstruktor
        public ConsoleMenu(Option[] options)
        {
            _options = options;
        }
        #endregion

        #region Properties
        public Option[] Options
        {
            get
            {
                return _options;
            }

            private set
            {
                _options = value;
            }
        }
        #endregion


        public string WriteMenu(int selectedOption)
        {
            string outputstring="";

            for (int i = 0; i < Options.Length; i++)
            {
                if (i == selectedOption)
                {
                    Console.BackgroundColor = ConsoleColor.DarkBlue;
                    Console.Write("> ");
                    Console.WriteLine(Options[i].Name);
                    Console.BackgroundColor = ConsoleColor.Black;
                }
                else
                {
                    Console.Write("  ");
                    Console.WriteLine(Options[i].Name);
                }
            }

            return outputstring;
        }
        public void MenuLoopInConsole()
        {
            bool chosen = false;
            int selectedOption = 0;

            for (int i = 0; i < Options.Length; i++)
            {
                Console.WriteLine("");
            }

            do
            {
                ClearConsoleLines(Options.Length+1);
                WriteMenu(selectedOption);
                //Interaction with console is absoluteley neccessary
                ConsoleKeyInfo k;
                do
                {
                     k = Console.ReadKey();
                    ClearCurrentConsoleLine();
                } while (!CheckValidMenuKey(k));

                switch (k.Key)
                {
                    case ConsoleKey.DownArrow:
                        chosen = false;
                        selectedOption++;
                        break;

                    case ConsoleKey.UpArrow:
                        chosen = false;
                        selectedOption--;
                        break;

                    case ConsoleKey.Enter:
                        chosen = true;
                        break;
                }

                if (selectedOption == Options.Length)
                {
                    selectedOption=0;
                }
                if (selectedOption == -1)
                {
                    selectedOption = Options.Length-1;
                }
            } while (!chosen);

            Options[selectedOption].Action();
        }
        public bool CheckValidMenuKey(ConsoleKeyInfo k)
        {
            if (k.Key == ConsoleKey.UpArrow || k.Key == ConsoleKey.DownArrow || k.Key == ConsoleKey.Enter)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static void ClearCurrentConsoleLine()
        {
            int currentLineCursor = Console.CursorTop;
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, currentLineCursor);
        }
        public static void ClearConsoleLines(int numberOfLinesToClear)
        {
            int currentLine = Console.CursorTop;

            for (int i = 0; i < numberOfLinesToClear; i++)
            {
                if (currentLine-i == -1)
                {
                    currentLine++;
                }

                Console.SetCursorPosition(0, currentLine-i);
                Console.Write(new string(' ', Console.WindowWidth));
                Console.SetCursorPosition(0, currentLine-i);
            }
        }
    }

    public class Option
    {
        public string Name { get; }
        public Action Action { get; }

        public Option(string name, Action action)
        {
            Name = name;
            Action = action;
        }
    }
}

