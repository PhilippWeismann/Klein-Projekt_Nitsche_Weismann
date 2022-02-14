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

        private static int _filesize = 1000000; //constant filsize of file which is downloaded [1000000 Byte]

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
                //if (value > 0)
                if (value >= 0)
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
                //if (value >= _minDownloadTime)
                if (value >= 0)
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
                //if (value >= _minDownloadTime && value <= _maxDownloadTime)
                if (value >= 0)
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
                //if (value >= _minDownloadTime && value <= _maxDownloadTime)
                if (value >= 0)
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
        public static void ResetStatitic()
        {
            MinimumDownloadTime = 0;
            MaximumDownloadTime = 0;
            AverageDownloadTime = 0;
            CountSpeedtest = 0;

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

            outputString += "The Average of the last " + CountSpeedtest + " Speedtests is: " + String.Format("{0:0.00}", ConvertDownloadTimeToMbitperSecond(AverageDownloadTime)) + " MBit/s";
            outputString += "\nThe fastest Speedtest was: " + String.Format("{0:0.00}", ConvertDownloadTimeToMbitperSecond((double)MinimumDownloadTime)) + " MBit/s";
            outputString += "\nThe slowest Speedtest was: " + String.Format("{0:0.00}", ConvertDownloadTimeToMbitperSecond((double)MaximumDownloadTime)) + " MBit/s";

            return outputString;
        }

        public static double ConvertDownloadTimeToMbitperSecond(double downloadTimeIn_ms)
        {
            double DownloadSpeed = 0; //Mbit/s
            int filesizeInMBit = _filesize * 8 / 1000000;

            DownloadSpeed = filesizeInMBit / (downloadTimeIn_ms/1000);

            return DownloadSpeed;
        
        }

        public static string BarString(int length)
        {
            try
            {
                if (CountSpeedtest < 3)
                {
                    return "Statistic Bar is only available after 3 Download Speed Measurements were taken.\nThere were currently " + CountSpeedtest + " Download Speed Measurements taken.";
                }
                else
                {
                    char marker = '█';
                    char blurred = '░';
                    char avgmarker = '▓';
                    int blurredLength = length - 4;
                    string avgstring = "";

                    string beginstring = "Max: " + String.Format("{0:0.00}", ConvertDownloadTimeToMbitperSecond(MinimumDownloadTime)) + "MBit/s  ";

                    int span = MaximumDownloadTime - MinimumDownloadTime;
                    double scalingfactor = (double)blurredLength / (double)span;


                    #region Bar_Line
                    string barstring = "";
                    barstring += beginstring;


                    if (CurrentDownloadTime > AverageDownloadTime)
                    {
                        int blurredBetweenMinAndAvg = Convert.ToInt32(scalingfactor * (AverageDownloadTime - MinimumDownloadTime));
                        int blurredBetweenAvgAndCurr = Convert.ToInt32(scalingfactor * (CurrentDownloadTime - AverageDownloadTime));
                        int blurredBetweenCurrAndMax = blurredLength - (blurredBetweenMinAndAvg + blurredBetweenAvgAndCurr);

                        barstring += marker; //mintime = maxdownload

                        for (int i = 0; i < blurredBetweenMinAndAvg; i++)
                        {
                            barstring += blurred;
                        }

                        barstring += avgmarker; //avg

                        for (int i = 0; i < blurredBetweenAvgAndCurr; i++)
                        {
                            barstring += blurred;
                        }

                        barstring += marker; // current

                        for (int i = 0; i < blurredBetweenCurrAndMax; i++)
                        {
                            barstring += blurred;
                        }

                        barstring += marker; //maxtime = mindownload
                    }
                    else
                    {
                        int blurredBetweenMinAndCurr = Convert.ToInt32(scalingfactor * (CurrentDownloadTime - MinimumDownloadTime));
                        int blurredBetweenCurrAndAvg = Convert.ToInt32(scalingfactor * (AverageDownloadTime - CurrentDownloadTime));
                        int blurredBetweenAvgAndMax = blurredLength - (blurredBetweenMinAndCurr + blurredBetweenCurrAndAvg);

                        barstring += marker; //mintime = maxdownload

                        for (int i = 0; i < blurredBetweenMinAndCurr; i++)
                        {
                            barstring += blurred;
                        }

                        barstring += marker; // current

                        for (int i = 0; i < blurredBetweenCurrAndAvg; i++)
                        {
                            barstring += blurred;
                        }

                        barstring += avgmarker; //avg

                        for (int i = 0; i < blurredBetweenAvgAndMax; i++)
                        {
                            barstring += blurred;
                        }

                        barstring += marker; //maxtime = mindownload
                    }



                    barstring += "  Min: " + String.Format("{0:0.00}", ConvertDownloadTimeToMbitperSecond(MaximumDownloadTime)) + "MBit/s  ";

                    barstring += "\n";
                    #endregion



                    #region AVG_Line

                    string descriptionstringAvg = "Avg: " + String.Format("{0:0.00}", ConvertDownloadTimeToMbitperSecond(AverageDownloadTime)) + "MBit/s  ";

                    int scaledAvg = beginstring.Length + Convert.ToInt32((AverageDownloadTime - MinimumDownloadTime) * scalingfactor);

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

                    #region Current_Description_Line
                    string descriptionstring = "Current: " + String.Format("{0:0.00}", ConvertDownloadTimeToMbitperSecond(CurrentDownloadTime)) + "MBit/s  " + "\n\n";

                    int scaledCurrent = beginstring.Length + Convert.ToInt32((CurrentDownloadTime - MinimumDownloadTime) * scalingfactor);

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
            }
            catch (Exception)
            {
                return "Oops... An Error occured. Statistics Plot cannot be displayed\n\n";
            }
            
            
        }

        #endregion
    }
}