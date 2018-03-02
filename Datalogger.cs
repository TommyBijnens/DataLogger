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
        public ObservableCollection<WaterTempControl> WaterControls { get; set; }

        public DataloggerClass()
        {
            //WENDY - PC\SQLEXPRESS
                 Dataloggers = new ObservableCollection<Logger>();
                 Thermostats = new ObservableCollection<Thermostat>();
                 WaterControls = new ObservableCollection<WaterTempControl>();
            
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

           
            //15 - 18   19 - 21//
            Thermostat t1 = new Thermostat("Verwarming Tommy Mazout", "http://192.168.0.153/", @"http://192.168.0.125/LocalProxy.svc/GetService/http:__192.168.0.150:8088_rest_devices_6_attributes_5_value", 1, @"http://192.168.0.125/LocalProxy.svc/WENDY-PC/getParameter/Temp_T1_M_min", @"http://192.168.0.125/LocalProxy.svc/WENDY-PC/getParameter/Temp_T1_M_max");
            Thermostat t2 = new Thermostat("Verwarming Tommy Hout",   "http://192.168.0.153/", @"http://192.168.0.125/LocalProxy.svc/GetService/http:__192.168.0.150:8088_rest_devices_6_attributes_6_value", 1, @"http://192.168.0.125/LocalProxy.svc/WENDY-PC/getParameter/Temp_T1_H_min", @"http://192.168.0.125/LocalProxy.svc/WENDY-PC/getParameter/Temp_T1_H_max");

            Thermostat t3 = new Thermostat("Verwarming Bruno & Sandy Mazout", "http://192.168.0.155/", @"http://192.168.0.125/LocalProxy.svc/GetService/http:__192.168.0.150:8088_rest_devices_6_attributes_1_value", 1, @"http://192.168.0.125/LocalProxy.svc/WENDY-PC/getParameter/Temp_T2_M_min", @"http://192.168.0.125/LocalProxy.svc/WENDY-PC/getParameter/Temp_T2_M_max");
            Thermostat t4 = new Thermostat("Verwarming Bruno & Sandy Hout",   "http://192.168.0.155/", @"http://192.168.0.125/LocalProxy.svc/GetService/http:__192.168.0.150:8088_rest_devices_6_attributes_2_value", 1, @"http://192.168.0.125/LocalProxy.svc/WENDY-PC/getParameter/Temp_T2_H_min", @"http://192.168.0.125/LocalProxy.svc/WENDY-PC/getParameter/Temp_T2_H_max");



            Thermostats.Add(t1);
            Thermostats.Add(t2);
            Thermostats.Add(t3);
            Thermostats.Add(t4);

            WaterTempControl w1 = new WaterTempControl("CV water temp",
                @"http://192.168.0.125/LocalProxy.svc/GetService/http:__192.168.0.150:8088_rest_devices_1_attributes_5_value*set=",
                5,
                @"http://192.168.0.125/LocalProxy.svc/WENDY-PC/getParameter/TempWater_X1",
                @"http://192.168.0.125/LocalProxy.svc/WENDY-PC/getParameter/TempWater_Y1",
                @"http://192.168.0.125/LocalProxy.svc/WENDY-PC/getParameter/TempWater_X2",
                @"http://192.168.0.125/LocalProxy.svc/WENDY-PC/getParameter/TempWater_Y2",
                @"http://api.openweathermap.org/data/2.5/weather?q=Mechelen&APPID=b694cc2ce9e6372989471765efe92429&units=metric",
                //buffer Temp//
                @"http://192.168.0.125/LocalProxy.svc/GetService/http:__192.168.0.150:8088_rest_devices_1_attributes_8_value*set=",
                5
                );

            WaterControls.Add(w1);

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

                foreach (WaterTempControl W in WaterControls)
                {
                    if (counter % W.timer == 0)
                        W.DoLog();

                }

                counter++;

             

            }
            catch (Exception e1) { Console.WriteLine(e1.ToString());/* MessageBox.Show("DoLog failed " + e1.ToString()); */}
        }


    }

     
}
