using LiveCharts.Wpf;
using LiveCharts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows.Media;
using MySql.Data.MySqlClient;
using Trains.View;
using System.Windows.Markup;
using Microsoft.Windows.Controls;
using LiveCharts.Defaults;
using ObservablePoint = LiveCharts.Defaults.ObservablePoint;

namespace Trains
{
    public class ChartViewModel : INotifyPropertyChanged
    {
        private SeriesCollection _seriesCollection;
        private string _timeFormat;
        private string _speedFormat;

        public SeriesCollection SeriesCollection
        {
            get { return _seriesCollection; }
            set
            {
                _seriesCollection = value;
                OnPropertyChanged("SeriesCollection");
            }
        }

        public string TimeFormat
        {
            get { return _timeFormat; }
            set
            {
                _timeFormat = value;
                OnPropertyChanged("TimeFormat");
            }
        }

        public string SpeedFormat
        {
            get { return _speedFormat; }
            set
            {
                _speedFormat = value;
                OnPropertyChanged("SpeedFormat");
            }
        }

        public ChartViewModel()
        {
            LoadDataFromMySQL();
        }

        private void LoadDataFromMySQL()
        {
            var data = new List<sTable>();

            // Connect to your MySQL database and retrieve the data
            // ...
            using (var connection = new MySqlConnection("Server=localhost;Database=train;User=root;Password=;"))
            {
                connection.Open();

                using (var command = new MySqlCommand("SELECT* FROM `stable` WHERE sn = '" + TrainsList.sn + "' and time BETWEEN '2023-01-27 00:01:00'AND '2023-01-28 23:00:00' and speed> 0", connection))
                {

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            data.Add(new sTable
                            {
                                timeD = reader.GetDateTime("time"),
                                speedD = reader.GetDouble("speed"),

                            });

                        }

                    }
                }
            }

            // Create a line series for the chart
            var lineSeries = new LineSeries
            {
                Values = new ChartValues<ObservablePoint>(),
                PointGeometry = DefaultGeometries.Circle,
                Fill = Brushes.Transparent,
            };

            // Add data points to the line series
            foreach (var dataL in data)
            {
                lineSeries.Values.Add(new ObservablePoint(data[0].timeD.ToOADate(), data[0].speedD));
            }

            SeriesCollection = new SeriesCollection
        {
            lineSeries
        };

            // Define the time and speed formats
            TimeFormat = "{0:yyyy-MM-dd HH:mm:ss}";
            SpeedFormat = "#.##";
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}