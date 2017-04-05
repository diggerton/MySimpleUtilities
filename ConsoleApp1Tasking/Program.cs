using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp1Tasking
{
    class Program
    {
        public static bool stopProcessor = false;
        public static bool Terminate = false;
        static void Main(string[] args)
        {
            //do
            //{
            //    Console.Write(Console.Read());

            //} while (true);


            #region MyRegion
            //Console.CancelKeyPress += (sender, e) =>
            //{

            //    Console.WriteLine("Exiting...");
            //    Environment.Exit(0);
            //};

            //Console.WriteLine("Press ESC to Exit");

            //var taskKeys = new Task(ReadKeys);
            //var taskProcessFiles = new Task(ProcessFiles);

            //taskKeys.Start();
            //taskProcessFiles.Start();

            //var tasks = new[] { taskKeys };
            //Task.WaitAll(tasks); 
            #endregion

            #region MyRegion
            //    Console.ForegroundColor = ConsoleColor.Green;
            //    Console.WriteLine("Split Analyzer starts");
            //    Console.ForegroundColor = ConsoleColor.Red;
            //    Console.WriteLine("Press Esc to quit.....");
            //    Thread MainThread = new Thread(new ThreadStart(startProcess));
            //    Thread ConsoleKeyListener = new Thread(new ThreadStart(ListerKeyBoardEvent));
            //    MainThread.Name = "Processor";
            //    ConsoleKeyListener.Name = "KeyListener";
            //    MainThread.Start();
            //    ConsoleKeyListener.Start();

            //    while (true)
            //    {
            //        if (Terminate)
            //        {
            //            Console.WriteLine("Terminating Process...");
            //            MainThread.Abort();
            //            ConsoleKeyListener.Abort();
            //            Thread.Sleep(2000);
            //            Thread.CurrentThread.Abort();
            //            return;
            //        }

            //        if (stopProcessor)
            //        {
            //            Console.WriteLine("Ending Process...");
            //            MainThread.Abort();
            //            ConsoleKeyListener.Abort();
            //            Thread.Sleep(2000);
            //            Thread.CurrentThread.Abort();
            //            return;
            //        }
            //    }
            //}

            //public static void ListerKeyBoardEvent()
            //{
            //    do
            //    {
            //        if (Console.ReadKey(true).Key == ConsoleKey.Escape)
            //        {
            //            Terminate = true;
            //        }
            //    } while (true);
            //}

            //public static void startProcess()
            //{
            //    int i = 0;
            //    while (true)
            //    {
            //        if (!stopProcessor && !Terminate)
            //        {
            //            Console.ForegroundColor = ConsoleColor.White;
            //            Console.WriteLine("Processing...." + i++);
            //            Thread.Sleep(3000);
            //        }
            //        if (i == 10)
            //            stopProcessor = true;

            //    } 
            #endregion
        }


        private static void ProcessFiles()
        {
            var files = Enumerable.Range(1, 100).Select(n => "File" + n + ".txt");

            var taskBusy = new Task(BusyIndicator);
            taskBusy.Start();

            foreach (var file in files)
            {
                Thread.Sleep(1000);
                Console.WriteLine("Processing file {0}", file);
            }
        }

        private static void BusyIndicator()
        {
            var busy = new ConsoleBusyIndicator();
            busy.UpdateProgress();
        }

        private static void ReadKeys()
        {
            ConsoleKeyInfo key = new ConsoleKeyInfo();

            while (!Console.KeyAvailable && key.Key != ConsoleKey.Escape)
            {

                key = Console.ReadKey(true);

                switch (key.Key)
                {
                    case ConsoleKey.UpArrow:
                        Console.WriteLine("UpArrow was pressed");
                        break;
                    case ConsoleKey.DownArrow:
                        Console.WriteLine("DownArrow was pressed");
                        break;

                    case ConsoleKey.RightArrow:
                        Console.WriteLine("RightArrow was pressed");
                        break;

                    case ConsoleKey.LeftArrow:
                        Console.WriteLine("LeftArrow was pressed");
                        break;

                    case ConsoleKey.Escape:
                        break;

                    default:
                        if (Console.CapsLock && Console.NumberLock)
                        {
                            Console.WriteLine(key.KeyChar);
                        }
                        break;
                }
            }
        }
    }

    internal class ConsoleBusyIndicator
    {
        int _currentBusySymbol;

        public char[] BusySymbols { get; set; }

        public ConsoleBusyIndicator()
        {
            BusySymbols = new[] { '|', '/', '-', '\\' };
        }
        public void UpdateProgress()
        {
            while (true)
            {
                Thread.Sleep(100);
                var originalX = Console.CursorLeft;
                var originalY = Console.CursorTop;

                Console.Write(BusySymbols[_currentBusySymbol]);

                _currentBusySymbol++;

                if (_currentBusySymbol == BusySymbols.Length)
                {
                    _currentBusySymbol = 0;
                }

                Console.SetCursorPosition(originalX, originalY);
            }
        }
    }
}
