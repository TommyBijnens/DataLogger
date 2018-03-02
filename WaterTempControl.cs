using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Datalogger
{
    public class WaterTempControl: INotifyPropertyChanged
    {
        public string path = @"C:\inetpub\wwwroot\"; //SERVER
        //public string path = @"C:\My Documents\Visual Studio 2015\Projects\test - 9 - P2\Website_TS\";//LAPTOP
        public string name { get; set; }
        public string url { get; set; }
        public int timer { get; set; }
        public string x1String { get; set; }
        public string y1String { get; set; }
        public string x2String { get; set; }
        public string y2String { get; set; }
        public int bufferGap { get; set; }
        public string bufferURL { get; set; }
        public string OutdoorTempString { get; set; }
        private double calculatedTemp;
        public double CalculatedTemp
        {
            get { return calculatedTemp; }
            set
            {
                calculatedTemp = value;
                this.RaisePropertyChanged("calculatedTemp");
            }
        }
       




        protected void RaisePropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {

                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;


        public WaterTempControl(string n, string u, int t, string x1, string y1, string x2, string y2, string outdoorTemp, string bufferurl,  int _buffergap)
        {
            name = n;
            url = u;
            timer = t;
            x1String = x1;
            y1String = y1;
            x2String = x2;
            y2String = y2;
            OutdoorTempString = outdoorTemp;
            bufferGap = _buffergap;
            bufferURL = bufferurl;



        }

        public virtual void DoLog()
        {
            try
            {
                string weatherResult = getFromWebService(OutdoorTempString);
                WeatherResult w = JsonConvert.DeserializeObject<WeatherResult>(weatherResult);
                double outdoorTemp = w.main.temp;
          //      string temp = getFromWebService(x1String).Replace("\"","");
                double temp_x1 = Convert.ToDouble(getFromWebService(x1String).Replace("\"", "").Replace(".",","));
                double temp_y1 = Convert.ToDouble(getFromWebService(y1String).Replace("\"", "").Replace(".", ","));
                double temp_x2 = Convert.ToDouble(getFromWebService(x2String).Replace("\"", "").Replace(".", ","));
                double temp_y2 = Convert.ToDouble(getFromWebService(y2String).Replace("\"", "").Replace(".", ","));

                double result = 0;

                if (outdoorTemp <= temp_x1) result = temp_y1;
                else if (outdoorTemp >= temp_x2) result = temp_y2;
                else {
                    double rico = (temp_y2 - temp_y1) / (temp_x2 - temp_x1);
                    result = Math.Round(temp_y1 + (rico * (outdoorTemp - temp_x1)));
                        }

                CalculatedTemp = result;
                string newTempString = ((int)result).ToString();
                string newTempStringBuffer = ((int)(result - bufferGap)).ToString();


                var setTemp = getFromWebService(url+ newTempString);
                var setBuffer = getFromWebService(bufferURL + newTempStringBuffer);

            }
            catch (Exception e) { /*MessageBox.Show("Log failed " + e.ToString());*/Console.WriteLine(e.ToString()); }

        }


      


        public string getFromWebService(string url)
        {
            string result = string.Empty;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.AutomaticDecompression = DecompressionMethods.GZip;
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                result = reader.ReadToEnd();
            }
            return result;
        }

        public class WeatherResult
        {
            public Main main;
        }

        public class Main
        {
            public double temp;
        }

    }
}
