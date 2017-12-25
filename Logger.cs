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
    public class Logger: INotifyPropertyChanged
    {
        public string path = @"C:\inetpub\wwwroot\"; //SERVER
        //public string path = @"C:\My Documents\Visual Studio 2015\Projects\test - 9 - P2\Website_TS\";//LAPTOP
        public string name { get; set; }
        public string url { get; set; }
        public int timer { get; set; }
        int maxLog = 30;
        public List<LogValue> logList { get; set; }
        private int lastLog;
        public int LastLog
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




        public Logger(string n, string u, int t)
        {
            name = n;
            url = u;
            timer = t;
            try
            {
                string jSon;
                jSon = File.ReadAllText(path + name + ".json", Encoding.UTF8);
                logList = JsonConvert.DeserializeObject<List<LogValue>>(jSon);
            }
            catch { logList = new List<LogValue>(); }
            name = n;
            url = u;
            timer = t;


        }

        public virtual void DoLog()
        {
            try
            {

                // logList.Add(new Datalogger.Log("123"));
                LogValue newValue = new LogValue(int.Parse(getFromWebService(url)));
                Database.Instance.setValue(newValue.value, name);
                logList.Add(newValue);
                LastLog = newValue.value;
                while (logList.Count > maxLog)
                {
                    logList.RemoveAt(0);
                }
                string json = JsonConvert.SerializeObject(logList);
                DirectoryInfo di = Directory.CreateDirectory(path);
                File.WriteAllText(path + name + ".json", json);
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



    }


    class CounterLogger: Logger
    {
        int oldValue = int.MaxValue;
        int newValue;
        int maxLog = 240;
        public CounterLogger(string n, string u, int t) : base(n, u, t)
        {
        }
        public override void DoLog()
        {

            newValue = (int.Parse(getFromWebService(url)));
            Database.Instance.setCounter(newValue, name);
            int difference = newValue - oldValue;
            if (difference < 0) difference = 0;
            LogValue newLog = new LogValue(difference);

            oldValue = newValue;
            //newValue.value = difference;
          
            logList.Add(newLog);
            LastLog = newLog.value;

            while (logList.Count > maxLog)
            {
                logList.RemoveAt(0);
            }


            string json = JsonConvert.SerializeObject(logList);
            DirectoryInfo di = Directory.CreateDirectory(path);
            File.WriteAllText(path + name + ".json", json);

        }


    }

    public class LogValue
    {
        public int value;
        public DateTime time;

        public LogValue(int input)
        {
            value = input;
            time = DateTime.Now;
        }
    }
}
