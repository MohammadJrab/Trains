using LiveCharts.Wpf;
using LiveCharts;
using MySql.Data.MySqlClient;
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
using System.Collections.ObjectModel;
using Trains.DAL;

namespace Trains.View
{
    /// <summary>
    /// Interaction logic for speedTImeDis.xaml
    /// </summary>
    public partial class speedTImeDis : Page
    {

        public speedTImeDis()
        {
            InitializeComponent();


            // Connect to database
            // set DataContext to view model
            DataContext = new MyViewModel();
        }
    }

    public class DataPoint
    {
        public DateTime Date { get; set; }
        public double Speed { get; set; }
        public double Distance { get; set; }
    }

    public class MyViewModel
    {
        public ObservableCollection<DataPoint> Data { get; set; }

        public MyViewModel()
        {
            // populate data collection from database
            Data = new ObservableCollection<DataPoint>();
            string sql = "SELECT time,speed,m1 FROM `stable` WHERE sn = '" + TrainsList.sn + "' and m1 BETWEEN '01' and '40' and m1>0  and F>0  ORDER BY m1 ASC ";
            using (var connection = new MySqlConnection(connectionState.ConnectionString))
            {
                connection.Open();
                using (var command = new MySqlCommand(sql, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                       
                    
                        while (reader.Read())
                        {
                            var date = reader.GetDateTime(0);
                            var speed = reader.GetDouble(1);
                            var distance = reader.GetDouble(2);

                            Data.Add(new DataPoint { Date = date, Speed = speed, Distance = distance });
                        }
                    }
                }
            }
        }
    }
}