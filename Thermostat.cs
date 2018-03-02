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
    public class Thermostat: INotifyPropertyChanged
    {
        public string path = @"C:\inetpub\wwwroot\"; //SERVER
        //public string path = @"C:\My Documents\Visual Studio 2015\Projects\test - 9 - P2\Website_TS\";//LAPTOP
        public string name { get; set; }
        public string url { get; set; }
        public int timer { get; set; }
        private double setTempMin;
        public double SetTempMin
        {
            get { return setTempMin; }
            set
            {
                setTempMin = value;
                this.RaisePropertyChanged("setTempMin");
            }
        }
        private double setTempMax;
        public double SetTempMax
        {
            get { return setTempMax; }
            set
            {
                setTempMax = value;
                this.RaisePropertyChanged("setTempMax");
            }
        }
        public string CommandUrl;
        public string MinUrl { get; set; }
        public string MaxUrl { get; set; }
        public bool WebsiteDriven = false;

        private bool enabled;
        public bool Enabled
        {
            get
            {
                return enabled;
            }
    set {
                enabled = value;
                this.RaisePropertyChanged("enabled");
    }
}




private double lastLog;
        public double LastLog
        {
            get
            {
                return lastLog;
            }
            set {
                lastLog = value;
                this.RaisePropertyChanged("lastlog");
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

        public Thermostat(string n, string u, string _commandUrl, int t, string _minUrl, string _maxUrl):
             this(n, u, _commandUrl, t, 0, 0)
        {
            MinUrl = _minUrl;
            MaxUrl = _maxUrl;
            WebsiteDriven = true;

        }


            public Thermostat(string n, string u, string _commandUrl, int t, int _min, int _max)
        {
            WebsiteDriven = false;
            SetTempMin = _min;
            SetTempMax = _max;
            name = n;
            url = u;
            timer = t;
            Enabled = false;
            CommandUrl = _commandUrl;
            
           


        }

        public virtual void DoLog()
        {
            try
            {
                
                if (WebsiteDriven) SetTempMin = Convert.ToDouble(getFromWebService(MinUrl).Replace("\"","").Replace(".",","));
                if (WebsiteDriven) SetTempMax = Convert.ToDouble(getFromWebService(MaxUrl).Replace("\"", "").Replace(".", ","));
                var running = getFromWebService(CommandUrl);
                Enabled = (running == "1");
                LastLog = double.Parse(getFromWebService(url).Replace(".",","));

                if (Enabled && (LastLog >= SetTempMax)) TurnOff();
                if (!Enabled && (LastLog <= SetTempMin)) TurnOn();


            }
            catch (Exception e) { /*MessageBox.Show("Log failed " + e.ToString());*/Console.WriteLine(e.ToString()); }

        }


        public void TurnOff() {
            Enabled = false;
            string offUrl = CommandUrl + "*set=0";
            string ws = getFromWebService(offUrl);
            //
        }

        public void TurnOn() {
            Enabled = true;
            string onUrl = CommandUrl + "*set=1";
            string ws = getFromWebService(onUrl);
 

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



    }
}
