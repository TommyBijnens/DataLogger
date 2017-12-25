using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Datalogger
{
    public sealed class Database
    {
        static readonly Database _instance = new Database();
        //private string connectionString = "server=PSBE-LT0656\\SQLEXPRESS;Trusted_Connection = yes;";
        private string connectionString = "server=WENDY-PC\\SQLEXPRESS;Trusted_Connection = yes;";//SERVER
       
        public static Database Instance
        {
            get
            {
                return _instance;
            }
        }
        Database()
        {
            try
            {
            }
            catch (Exception e) { Console.WriteLine(e.ToString()); /* MessageBox.Show("Cannot open network connection " + e.ToString()); */}

        }

        public Counter getCounter(string name)
        {
            string counter = "0";
            string tempCounter = "0";
            try
            {
                SqlConnection myConnection = new SqlConnection(connectionString);
            myConnection.Open();
            SqlCommand myCommand = new SqlCommand("SELECT TOP 1 Counter, TempCounter FROM CVLogger.dbo."+name+" ORDER BY Id DESC", myConnection);
            SqlDataReader myReader = myCommand.ExecuteReader();
            while (myReader.Read())
            {
                counter = (myReader["Counter"].ToString());
                tempCounter = (myReader["TempCounter"].ToString());
            }
            myReader.Close();
            myConnection.Close();
            
            }
            catch (Exception e) { Console.WriteLine(e.ToString()); /* MessageBox.Show("getCounter Failed for " + name + " --> " + e.ToString());*/ }
            return new Counter(int.Parse(counter), int.Parse(tempCounter));
        }

        public void setCounter(int tempCounter, string name)
        {
            try
            {
                SqlConnection myConnection = new SqlConnection(connectionString);
            myConnection.Open();
            Counter counter = getCounter(name).CounterIncreased(tempCounter);
            SqlCommand myCommand = new SqlCommand("INSERT INTO CVLogger.dbo."+name+"(Counter, TempCounter) VALUES (" + counter.value + ", " + counter.tempValue + ")", myConnection);
            myCommand.ExecuteNonQuery();
            myConnection.Close();
            }
            catch (Exception e) { Console.WriteLine(e.ToString());  /*MessageBox.Show("setCounter Failed for " + name + " --> " + e.ToString()); */}
        }

        public int getValue(string name)
        {
            string value = "0";
            try
            {
                SqlConnection myConnection = new SqlConnection(connectionString);
            myConnection.Open();
            SqlCommand myCommand = new SqlCommand("SELECT TOP 1 Counter, TempCounter FROM CVLogger.dbo." + name + " ORDER BY Id DESC", myConnection);
            SqlDataReader myReader = myCommand.ExecuteReader();
            
            while (myReader.Read())
            {
                value = (myReader["Value"].ToString());
            }
            myReader.Close();
            myConnection.Close();
            }
            catch (Exception e) { Console.WriteLine(e.ToString());  /*MessageBox.Show("getValue Failed for " + name + " --> " + e.ToString()); */}
            return int.Parse(value);
        }

        public void setValue(int value, string name)
        {
            try
            {
                SqlConnection myConnection = new SqlConnection(connectionString);
            myConnection.Open();
            SqlCommand myCommand = new SqlCommand("INSERT INTO CVLogger.dbo." + name + "(Value) VALUES (" + value + ")", myConnection);
            myCommand.ExecuteNonQuery();
            myConnection.Close();
            }
            catch (Exception e) { Console.WriteLine(e.ToString());/* MessageBox.Show("setValue Failed for "+ name + " --> " + e.ToString());*/ }
        }

    }

       


}
