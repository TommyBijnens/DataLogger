using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Datalogger
{
    public class DataloggerClass
    {
        int cyclyTimeSeconds = 0;
        int cycleTimeMinutes = 1;
        int counter = 0;
    
        string startUrl = @"http://192.168.0.150:8088/rest/devices"; //online
        //string startUrl = @"http://localhost:59392/service1.svc/rest/devices"; //offline


        public ObservableCollection<Logger> Dataloggers { get; set; }
        public ObservableCollection<Thermostat> Thermostats { get; set; }

        public DataloggerClass()
        {
            //WENDY - PC\SQLEXPRESS
                 Dataloggers = new ObservableCollection<Logger>();
                 Thermostats = new ObservableCollection<Thermostat>();
                   
                Logger a = new Logger("Temp1", startUrl+@"/1/attributes/1/value", 1);// 5 min --> 5*30 = 2.5u
                Logger b = new Logger("Temp2", startUrl + @"/1/attributes/2/value", 1);
                Logger c = new Logger("Temp3", startUrl + @"/1/attributes/3/value", 1);
                Logger d = new Logger("TempB", startUrl + @"/1/attributes/4/value", 1);
                Dataloggers.Add(a);
                Dataloggers.Add(b);
                Dataloggers.Add(c);
                Dataloggers.Add(d);

                CounterLogger c1 = new CounterLogger("BS_H", startUrl + @"/4/attributes/1/value", 5); //1u
                CounterLogger c2 = new CounterLogger("BS_M", startUrl + @"/4/attributes/2/value", 5);
                CounterLogger c3 = new CounterLogger("BS_TOT", startUrl + @"/4/attributes/3/value", 5);
                CounterLogger c4 = new CounterLogger("T_H", startUrl + @"/4/attributes/4/value", 5);
                CounterLogger c5 = new CounterLogger("T_M", startUrl + @"/4/attributes/5/value", 5);
                CounterLogger c6 = new CounterLogger("T_TOT", startUrl + @"/4/attributes/6/value", 5);

                Dataloggers.Add(c1);
                Dataloggers.Add(c2);
                Dataloggers.Add(c3);
                Dataloggers.Add(c4);
                Dataloggers.Add(c5);
                Dataloggers.Add(c6);

           

            Thermostat t1 = new Thermostat("Verwarming Tommy Mazout", "http://192.168.0.153/", @"http://192.168.0.125/LocalProxy.svc/GetService/http:__192.168.0.150:8088_rest_devices_6_attributes_5_value", 1, 15, 18);
            Thermostat t2 = new Thermostat("Verwarming Tommy Hout",   "http://192.168.0.153/", @"http://192.168.0.125/LocalProxy.svc/GetService/http:__192.168.0.150:8088_rest_devices_6_attributes_6_value", 1, 19, 21);

            Thermostats.Add(t1);
            Thermostats.Add(t2);

            DoLog(null, null);
              System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(DoLog);
            dispatcherTimer.Interval = new TimeSpan(0, cycleTimeMinutes, cyclyTimeSeconds);
            dispatcherTimer.Start();
            
            // DoLog();

        }

        public void DoLog(object sender, EventArgs e)
        {
            try
            {
                foreach (Logger L in Dataloggers)
                {
                    if (counter % L.timer == 0)
                        L.DoLog();

                }
                foreach (Thermostat T in Thermostats)
                {
                    if (counter % T.timer == 0)
                        T.DoLog();

                }

                counter++;

             

            }
            catch (Exception e1) { Console.WriteLine(e1.ToString());/* MessageBox.Show("DoLog failed " + e1.ToString()); */}
        }


    }

     
}
