using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SqlClient;

namespace Datalogger
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DataloggerClass dataLogger;
        public MainWindow()
        {
            InitializeComponent();
            try
            {
                dataLogger = new DataloggerClass();
                this.DataContext = dataLogger;
            }
            catch (Exception e) { Console.WriteLine(e.ToString()); /*MessageBox.Show(e.ToString());*/ }
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
           // dataLogger.DoLog();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
           

            try
            {

                MessageBox.Show("Well done! " + Database.Instance.getValue("Temp1"));

            }
            catch (SqlException ex)
            {
                MessageBox.Show("You failed!" + ex.Message);
            }




        }
    }
}
