using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klein_Projekt_Nitsche_Weismann
{
    class Statistics
    {
        #region Variables
        private static double _average;
        private static double _maxValue;
        private static double _minValue;
        private static int _countSpeedtest;     //count how often a test saved in the data

        private static string _filpath = @"..\..\..\Statistic.csv";

        //private static List<double> _statisticAsList;
        private static double[] _statisticAsArray = new double[4];  //[0] average, [1] maxValue, [2] minValue, [4] countSpeedtest
        #endregion

        #region Properties
        public static double Average
        {
            get
            {
                return _average;
            }
            set
            {
                _average = value;
            }
        }
        public static double MaxValue
        {
            get
            {
                return _maxValue;
            }
            set
            {
                _maxValue = value;
            }
        }
        public static double MinValue
        {
            get
            {
                return _minValue;
            }
            set
            {
                _minValue = value;
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
                _countSpeedtest = value;
            }
        }
        #endregion

        #region methods
        public static void AddNewData(double value)
        {
            ReadStatisticCsv();
            Average = _statisticAsArray[0];
            MaxValue = _statisticAsArray[1];
            MinValue = _statisticAsArray[2];
            CountSpeedtest = Convert.ToInt32(_statisticAsArray[3]);

            if (CountSpeedtest > 0)
            {
                Average = ((Average * CountSpeedtest) + value) / (CountSpeedtest + 1);
                if (value > MaxValue)
                {
                    MaxValue = value;
                }
                if (value < MinValue)
                {
                    MinValue = value;
                }
            }
            else        //nur beim ersten SpeedTest
            {
                Average = value;
                MaxValue = value;
                MinValue = value;
            }
            CountSpeedtest += 1;
            StatisticToCsv();
        }
        public static void ReadStatisticCsv()
        {
            List<double> statisticAsList = new List<double>();

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
                        statisticAsList.Add(line);
                    }
                    catch (Exception)
                    {
                        throw new Exception();
                    }
                }
                counter++;
            }
            myStreamReader.Close();

            _statisticAsArray = statisticAsList.ToArray();
        }
        public static void StatisticToCsv()
        {
            using (StreamWriter myStreamWriter = new StreamWriter(_filpath))
            {
                myStreamWriter.WriteLine(Average);
                myStreamWriter.WriteLine(MaxValue);
                myStreamWriter.WriteLine(MinValue);
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

            if (value > Average)
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

            outputString += "The Average of the last " + CountSpeedtest + " Speedtests is: " + Average + " MBit/s";
            outputString += "\nThe fastest Speedtest was: " + MaxValue + " MBit/s";
            outputString += "\nThe slowest Speedtest was: " + MinValue + " MBit/s";

            return outputString;
        }
        #endregion
    }
}