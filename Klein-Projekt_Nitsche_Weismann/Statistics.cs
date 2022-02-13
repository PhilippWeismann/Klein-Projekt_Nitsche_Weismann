using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klein_Projekt_Nitsche_Weismann
{
    static class Statistics
    {

        #region Members
        static int _minDownloadTime;
        static double _averageDownloadTime;
        static int _maxDownloadTime;
        static int _currentDownloadTime;
        private static int _countSpeedtest;     //count how often a test saved in the data

        private static string _filpath = @"..\..\..\Statistic.csv";

        private static double[] _statisticAsArray = new double[5];  //[0] MinimumDownloadTime
                                                                    //[1] AverageDownloadTime
                                                                    //[2] MaximumDownloadTime
                                                                    //[3] last/current DownloadTime
                                                                    //[4] countSpeedtest

        //static int _length;
        #endregion

        #region Properties
        public static int MinimumDownloadTime
        {
            get
            {
                return _minDownloadTime;
            }
            set
            {
                if (value > 0)
                {
                    _minDownloadTime = value;
                }
                else
                {
                    new Exception("Invalid Minimum Download Time Value!");
                }
            }
        }

        public static int MaximumDownloadTime
        {
            get
            {
                return _maxDownloadTime;
            }
            set
            {
                if (value > _minDownloadTime)
                {
                    _maxDownloadTime = value;
                }
                else
                {
                    new Exception("Invalid Maximum Download Time Value!");
                }
            }
        }

        public static int CurrentDownloadTime
        {
            get
            {
                return _currentDownloadTime;
            }
            set
            {
                if (value > _minDownloadTime && value < _maxDownloadTime)
                {
                    _currentDownloadTime = value;
                }
                else
                {
                    new Exception("Invalid Current Download Time Value!");
                }
            }
        }

        public static double AverageDownloadTime
        {
            get
            {
                return _averageDownloadTime;
            }
            set
            {
                if (value > _minDownloadTime && value < _maxDownloadTime)
                {
                    _averageDownloadTime = value;
                }
                else
                {
                    new Exception("Invalid Average Download Time Value!");
                }
            }
        }

        public static int CountSpeedtest
        {
            get
            {
                return _countSpeedtest;
            }
            set
            {
                if (value > 0)
                {
                    _countSpeedtest = value;
                }
                else
                {
                    new Exception("Invalid Speedtest Count Value!");
                }
            }
        }

        #endregion

        //No Konstruktor at static classes

        #region methods
        public static void AddNewData(int downloadTimeValue)
        {
            ReadStatisticCsv();
            MinimumDownloadTime = Convert.ToInt32(_statisticAsArray[0]);
            AverageDownloadTime = _statisticAsArray[1];
            MaximumDownloadTime = Convert.ToInt32(_statisticAsArray[2]);
            CurrentDownloadTime = Convert.ToInt32(_statisticAsArray[3]);
            CountSpeedtest = Convert.ToInt32(_statisticAsArray[4]);

            if (CountSpeedtest > 0)
            {
                AverageDownloadTime = ((AverageDownloadTime * CountSpeedtest) + downloadTimeValue) / (CountSpeedtest + 1);
                if (downloadTimeValue > MaximumDownloadTime)
                {
                    MaximumDownloadTime = downloadTimeValue;
                }
                if (downloadTimeValue < MinimumDownloadTime)
                {
                    MinimumDownloadTime = downloadTimeValue;
                }
            }
            else        //nur beim ersten SpeedTest
            {
                AverageDownloadTime = downloadTimeValue;
                MaximumDownloadTime = downloadTimeValue;
                MinimumDownloadTime = downloadTimeValue;
            }
            CurrentDownloadTime = downloadTimeValue;
            CountSpeedtest += 1;
            StatisticToCsv();
        }
        public static double[] ReadStatisticCsv()
        {
            //List<double> statisticAsList = new List<double>();

            StreamReader myStreamReader = new StreamReader(_filpath);
            double line;
            int counter = 0;

            while (myStreamReader.Peek() != -1)
            {
                double.TryParse(myStreamReader.ReadLine(), out line);

                if (counter < 5)
                {
                    try
                    {
                        //statisticAsList.Add(line);
                        _statisticAsArray[counter] = line;
                    }
                    catch (Exception)
                    {
                        throw new Exception();
                    }
                }
                counter++;
            }
            myStreamReader.Close();

            //_statisticAsArray = statisticAsList.ToArray();

            return _statisticAsArray;
        }
        public static void StatisticToCsv()
        {
            using (StreamWriter myStreamWriter = new StreamWriter(_filpath))
            {
                myStreamWriter.WriteLine(MinimumDownloadTime);
                myStreamWriter.WriteLine(AverageDownloadTime);
                myStreamWriter.WriteLine(MaximumDownloadTime);
                myStreamWriter.WriteLine(CurrentDownloadTime);
                myStreamWriter.WriteLine(CountSpeedtest);
            }
        }

        public static bool IsValueGreaterThanAverage(double value)
        {
            bool output = false;

            //if (Average < 0.000001)     // if Average = 0         !!unnötig, da ja zuerst ein Testgemacht werden muss!!
            //{
            //    ReadStatisticCsv();
            //}

            if (value > AverageDownloadTime)
            {
                output = true;
            }

            return output;
        }

        public static string AsString()
        {
            string outputString;

            if (CountSpeedtest == 0)     // if the user will see the satistic before a speedtest
            {
                ReadStatisticCsv();
            }

            outputString = "Statistic Speedtest:\n\n";

            outputString += "The Average of the last " + CountSpeedtest + " Speedtests is: " + AverageDownloadTime + " MBit/s";
            outputString += "\nThe fastest Speedtest was: " + MaximumDownloadTime + " MBit/s";
            outputString += "\nThe slowest Speedtest was: " + MinimumDownloadTime + " MBit/s";

            return outputString;
        }

        private static double ConvertDownloadTimeToMbitperSecond()
        {
            double mbits = 0;

            //code zum umrechnen

            return mbits;
        
        }

        public static string BarString(int length)
        {
            int lengthWithoutMarker = length - 3;
            char marker = '█';
            char blurred = '░';

            int span = MaximumDownloadTime - MinimumDownloadTime;

            double stretchfactor = (double)lengthWithoutMarker / (double)span;

            int signsBelowCurrentTime = (int) Math.Round(stretchfactor * (CurrentDownloadTime - MinimumDownloadTime));

            int signsUpperCurrentTime = lengthWithoutMarker - signsBelowCurrentTime;


            string progressbarstring = "";

            progressbarstring += marker;
            for (int i = 0; i < signsBelowCurrentTime + 1; i++)
            {
                progressbarstring += blurred;
            }
            progressbarstring += marker;
            for (int i = 0; i < signsUpperCurrentTime + 1; i++)
            {
                progressbarstring += blurred;
            }
            progressbarstring += marker;

            return progressbarstring;
        }

        #endregion
    }
}