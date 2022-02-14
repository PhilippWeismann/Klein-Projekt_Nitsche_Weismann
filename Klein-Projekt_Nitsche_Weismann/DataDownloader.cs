using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Klein_Projekt_Nitsche_Weismann
{
    static class DataDownloader
    {
        static string downloadLink = "https://dl.dropboxusercontent.com/s/xqii6dc6s3ko6k2/DownloadTxt_10hoch6_Bytes_2.txt?dl=0"; //txt-Datei mit 1Mio bytes

        static string tempSavingPath = @"..\..\temp.txt";

        static WebClient myWebclient = new WebClient();

        static Stopwatch stopwatch = new Stopwatch();

        public static long DownloadTimeOfFile()
        {
            int currentDownloadTime = 0;

            try
            {
                stopwatch.Start();
                myWebclient.DownloadFile(downloadLink, tempSavingPath);
                stopwatch.Stop();

                currentDownloadTime = Convert.ToInt32(stopwatch.ElapsedMilliseconds);

                stopwatch.Reset();

                Statistics.AddNewData(currentDownloadTime);
            }
            catch (Exception)
            {
                currentDownloadTime = -1;
            }
            return currentDownloadTime;
        
        }
    }
}
