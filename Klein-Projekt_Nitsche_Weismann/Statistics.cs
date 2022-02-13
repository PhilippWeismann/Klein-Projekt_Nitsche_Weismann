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

        private static string _filepath = @"..\..\..\Statistic.csv";

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
                if (value >= _minDownloadTime)
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
                if (value >= _minDownloadTime && value <= _maxDownloadTime)
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
                if (value >= _minDownloadTime && value <= _maxDownloadTime)
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
                if (value >= 0)
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
            ReadStatisticCsv(';');

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
                MinimumDownloadTime = downloadTimeValue;
                MaximumDownloadTime = downloadTimeValue;
                AverageDownloadTime = downloadTimeValue;
            }
            CurrentDownloadTime = downloadTimeValue;
            CountSpeedtest += 1;
            StatisticToCsv();
        }
        public static void ReadStatisticCsv(char seperator)
        {

            string[] parts;

            StreamReader myStreamReader = new StreamReader(_filepath);

            string line;

            int counter = 1;

            while (myStreamReader.Peek() != -1)
            {
                line = myStreamReader.ReadLine();

                if (counter > 1)
                {
                    try
                    {
                        parts = line.Split(seperator);

                        for (int i = 0; i < parts.Length; i++)
                        {
                            parts[i] = parts[i].Replace('.', ',');
                        }

                        
                        MinimumDownloadTime = Convert.ToInt32(parts[0]);
                        MaximumDownloadTime = Convert.ToInt32(parts[2]);
                        AverageDownloadTime = Convert.ToDouble(parts[1]);
                        CurrentDownloadTime = Convert.ToInt32(parts[3]);
                        CountSpeedtest = Convert.ToInt32(parts[4]);

                    }
                    catch (Exception)
                    {
                        // throw auskommentiert , das Programm weiterlaufen soll um zu berechnen (Fail silent)
                        // throw würde Aktiv werden wenn eine Exception bei den Set-Properties auftritt (Wertebereich)

                        /*throw*/
                        new Exception("Statistic Data is not readable");
                    }
                }

                counter++;
            }
            myStreamReader.Close();
        }


        public static void StatisticToCsv()
        {
            using (StreamWriter myStreamWriter = new StreamWriter(_filepath))
            {

                myStreamWriter.WriteLine("Min;Avg;Max;Current;Count");
                string writeString = MinimumDownloadTime + ";" + AverageDownloadTime + ";" + MaximumDownloadTime + ";" + CurrentDownloadTime + ";" + CountSpeedtest + ";";
                myStreamWriter.WriteLine(writeString);
                
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
                ReadStatisticCsv(';');
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
            char marker = '█';
            char blurred = '░';
            char avgmarker = '▓';
            int blurredLength = length - 4;
            string avgstring = "";

            string beginstring = "Min: " + MinimumDownloadTime + "ms  ";

            int span = MaximumDownloadTime - MinimumDownloadTime;
            double scalingfactor = (double)length / (double)span;
            int scaledMin = beginstring.Length;
            int scaledAvg = scaledMin + Convert.ToInt32((AverageDownloadTime - MinimumDownloadTime) * scalingfactor);
            int scaledCurrent = scaledMin + Convert.ToInt32((CurrentDownloadTime - MinimumDownloadTime) * scalingfactor);

            #region AVG_Line

            string descriptionstringAvg = "Avg: " + String.Format("{0:0.00}", AverageDownloadTime) + "ms";

            int spacesBeforeAvgDescription = scaledAvg - (int)Math.Round((double)descriptionstringAvg.Length / 2);

            if (spacesBeforeAvgDescription < 0)
            {
                spacesBeforeAvgDescription = 0;
            }

            for (int i = 0; i < spacesBeforeAvgDescription; i++)
            {
                avgstring += " ";
            }
            avgstring += "  " + descriptionstringAvg;
            avgstring += "\n";

            #endregion

            #region Bar_Line
            string barstring = "";
            barstring += beginstring;

            for (int i = 0; i < blurredLength; i++)
            {
                barstring += blurred;
            }

            if (scaledCurrent > scaledAvg)
            {
                barstring = barstring.Insert(scaledMin, marker.ToString());
                barstring = barstring.Insert(scaledAvg + 1, avgmarker.ToString());
                barstring = barstring.Insert(scaledCurrent + 2, marker.ToString());
                barstring += marker;
            }
            else if (scaledCurrent < scaledAvg)
            {
                barstring = barstring.Insert(scaledMin, marker.ToString());
                barstring = barstring.Insert(scaledCurrent + 1, marker.ToString());
                barstring = barstring.Insert(scaledAvg + 2, avgmarker.ToString());
                barstring += marker;
            }
            else
            {
                barstring = barstring.Insert(scaledMin, marker.ToString());
                barstring = barstring.Insert(scaledCurrent + 1, marker.ToString());
                barstring += marker;
            }


            barstring += "  Max: " + MaximumDownloadTime + "ms";

            barstring += "\n";
            #endregion

            #region Current_Description_Line
            string descriptionstring = "Current: " + CurrentDownloadTime + "ms" + "\n\n";

            int spacesBeforeCurrentTimeDescription = scaledCurrent - (int)Math.Round((double)descriptionstring.Length / 2);

            if (spacesBeforeCurrentTimeDescription < 0)
            {
                spacesBeforeCurrentTimeDescription = 0;
            }

            for (int i = 0; i < spacesBeforeCurrentTimeDescription; i++)
            {
                barstring += " ";
            }

            barstring += "  " + descriptionstring;
            #endregion


            return avgstring + barstring;
        }

        #endregion
    }
}