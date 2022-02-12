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
        private static double _averageDownloadTime;
        static double _minDownloadTime;
        static double _maxDownloadTime;
        static double _currentDownloadTime;
        private static int _countSpeedtest;     //count how often a test saved in the data

        private static string _filpath = @"..\..\..\Statistic.csv";

        private static double[] _statisticAsArray = new double[5];  //[0] average, [1] maxValue, [2] minValue, [4] last/current DownloadTime, [4] countSpeedtest

        //static int _length;
        #endregion

        #region Properties
        public static double MinimumDownloadTime
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

        public static double MaximumDownloadTime
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

        public static double CurrentDownloadTime
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
        public static void AddNewData(double value)
        {
            ReadStatisticCsv();
            AverageDownloadTime = _statisticAsArray[0];
            MaximumDownloadTime = Convert.ToInt32(_statisticAsArray[1]);
            MinimumDownloadTime = Convert.ToInt32(_statisticAsArray[2]);
            CountSpeedtest = Convert.ToInt32(_statisticAsArray[4]);

            if (CountSpeedtest > 0)
            {
                AverageDownloadTime = ((AverageDownloadTime * CountSpeedtest) + value) / (CountSpeedtest + 1);
                if (value > MaximumDownloadTime)
                {
                    MaximumDownloadTime = value;
                }
                if (value < MinimumDownloadTime)
                {
                    MinimumDownloadTime = value;
                }
            }
            else        //nur beim ersten SpeedTest
            {
                AverageDownloadTime = value;
                MaximumDownloadTime = value;
                MinimumDownloadTime = value;
            }
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

                if (counter < 4)
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
                myStreamWriter.WriteLine(AverageDownloadTime);
                myStreamWriter.WriteLine(MaximumDownloadTime);
                myStreamWriter.WriteLine(MinimumDownloadTime);
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

        static string DisplayProgessBarInConsole(int length)
        {







            return "";
        }

        static void ReadMinMiaxValuesFromDatabaseTxt()
        {
            MinimumDownloadTime = Statistics.ReadStatisticCsv()[0];
            MinimumDownloadTime = Statistics.ReadStatisticCsv()[0];
            MinimumDownloadTime = Statistics.ReadStatisticCsv()[0];



        }
        #endregion
    }
}