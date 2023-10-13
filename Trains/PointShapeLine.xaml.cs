using LiveCharts.Wpf;
using LiveCharts;
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
using MySql.Data.MySqlClient;
using Trains.View;

namespace Trains
{
    /// <summary>
    /// Interaction logic for PointShapeLine.xaml
    /// </summary>
    public partial class PointShapeLine : UserControl
    {
        public PointShapeLine()
        {
            InitializeComponent();
            LoadData();
            DataContext = this;
            SeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "speed",
                    Values = new ChartValues<double> { }
                },
              
             
            };

            YFormatter = value => value.ToString("C");

            //modifying the series collection will animate and update the chart
          
            //modifying any series values will also animate and update the chart
            //SeriesCollection[3].Values.Add(5d);

            DataContext = this;
        }

        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> YFormatter { get; set; }




        private List<MeasureModel> chartData;

        private void LoadData()
        {
            chartData = new List<MeasureModel>();

            using (var connection = new MySqlConnection("Server=localhost;Database=train;User=root;Password=;"))
            {
                connection.Open();

                using (var command = new MySqlCommand("SELECT time, speed FROM stable where sn='" + TrainsAd.sn + "'", connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var time = reader.GetDateTime("time");
                            var speed = reader.GetDouble("speed");

                            chartData.Add(new MeasureModel { Time = time, Speed = speed });
                        }
                    }
                }
            }
        }

        public IEnumerable<MeasureModel> ChartData
        {
            get { return chartData; }
        }

        public string TimeFormat(double value)
        {
            return chartData[(int)value].Time.ToString("HH:mm");
        }

        public class MeasureModel
        {
            public DateTime Time { get; set; }
            public double Speed { get; set; }

        }
    }
  }