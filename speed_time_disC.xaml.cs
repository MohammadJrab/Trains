using LiveCharts;
using LiveCharts.Wpf;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using MessageBox = System.Windows.Forms.MessageBox;
using SaveFileDialog = Microsoft.Win32.SaveFileDialog;
using ControlzEx.Standard;
using iTextSharp.text;
using System.Net.NetworkInformation;
using PrintDialog = System.Windows.Controls.PrintDialog;
using FontFamily = System.Windows.Media.FontFamily;
using Brushes = System.Windows.Media.Brushes;
using Separator = LiveCharts.Wpf.Separator;
using Syncfusion.Windows.Shared;
using System.Collections.ObjectModel;
using System.Configuration;
using Microsoft.SharePoint.Client;
using Toolkit.Data.Sources.SQLData;
using File = System.IO.File;
using LiveCharts.Defaults;
using System.Runtime.Serialization;
using System.ComponentModel;
using LiveCharts.Definitions.Charts;
using System.Data;
using System.Windows.Markup;
using System.Globalization;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;
using Trains.DAL;

namespace Trains.View
{

    /// <summary>
    /// Interaction logic for speed_time_disC.xaml
    /// </summary>
    public partial class speed_time_disC : Page
    {

        public speed_time_disC()
        {
            InitializeComponent();
            String snn = TrainsList.sn;
            snnLbl.Text = snn + " : القاطرة";

            var viewModel = new MainViewModel();
            DataContext = viewModel;
          
        }
    

    public class MainViewModel
    {
        public ChartValues<ObservablePoint> SeriesValues { get; set; }
          public ChartValues<ObservablePoint> SeriesValues2 { get; set; }

            public Func<double, string> XLabelFormatter { get; set; }
        public Func<double, string> YLabelFormatter { get; set; }
            public Func<double, string> XLabelFormatter2 { get; set; }

            public ObservableCollection<string> XLabels { get; set; }
            public DateTime XMinValue { get; set; }
            public DateTime XMaxValue { get; set; }
            DateTime minTime;
            DateTime maxTime;
            public MainViewModel()
        {
            SeriesValues = new ChartValues<ObservablePoint>();
                XLabels = new ObservableCollection<string>();

             MySqlConnection connection = new MySqlConnection(connectionState.ConnectionString);
            connection.Open();

            // Execute the SQL query to retrieve the data
            string query = "SELECT m1,speed,time FROM `stable` WHERE sn = '" + TrainsList.sn + "' and m1 BETWEEN '01' and '40' and m1>0  and F>0  ORDER BY m1 ASC ;";
             MySqlCommand command = new MySqlCommand(query, connection);
             MySqlDataReader reader = command.ExecuteReader();
        
                while (reader.Read())
            {
                double xValue = reader.GetDouble(0); 
                double yValue = reader.GetDouble(1); 
                    DateTime xValue2 = reader.GetDateTime(2); 
                    XLabels.Add(xValue2.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture));
              
                    SeriesValues.Add(new ObservablePoint(xValue, yValue));

                }
                reader.Close();
                MySqlCommand command2 = new MySqlCommand("SELECT Min(time), Max(Time) FROM `stable` WHERE sn = '" + TrainsList.sn + "'  and m1 BETWEEN '01' and '40' and m1 > 0  and F > 0  ORDER BY m1 ASC ;", connection);
                MySqlDataReader reader2 = command2.ExecuteReader();
                while (reader2.Read())
                {
                minTime = reader2.GetDateTime(0);
                maxTime = reader2.GetDateTime(1);

                }


                XLabelFormatter = value => value.ToString("N1");
            YLabelFormatter = value => value.ToString("N1");

                XLabelFormatter2 = value => new DateTime((long)value).ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                XMinValue = minTime;
                XMaxValue = maxTime;
            }
        }


    private SeriesCollection seriesCollection;
        List<double> speeds = new List<double>();
        List<double> m1Doub = new List<double>();
        List<string> m1 = new List<string>();
        List<DateTime> times = new List<DateTime>();
        private List<string> _labels;

        string sql = "SELECT * FROM `stable` WHERE sn = '" + TrainsList.sn + "' ";
        int step = 0;


        //private void loadData()
        //{
        //    try
        //    {
        //        if (dateRdn.IsChecked == true)
        //        {
        //            sql = "SELECT * FROM `stable` WHERE sn = '" + TrainsList.sn + "' and time BETWEEN '" + startDate.Text + "' and '" + endDate.Text + "' and  m1>0  and F>0  ORDER BY time ASC ";
        //        }
        //        else if (distanceRdn.IsChecked == true)
        //        {
        //            sql = "SELECT * FROM `stable` WHERE sn = '" + TrainsList.sn + "' and m1 BETWEEN '" + numUpDown.Text + "' and '" + numUpDownTo.Text + "' and m1>0  and F>0    ORDER BY m1 ASC ";


        //        }
        //        seriesCollection = new SeriesCollection();

        //        using (var connection = new MySqlConnection(connectionState.ConnectionString))
        //        {
        //            connection.Open();
        //            using (var command = new MySqlCommand(sql, connection))
        //            {
        //                using (var reader = command.ExecuteReader())
        //                {
        //                    while (reader.Read())
        //                    {
        //                        speeds.Add(reader.GetDouble("speed"));
        //                        times.Add(reader.GetDateTime("time"));
        //                        m1.Add((reader.GetDouble("m1")).ToString());
        //                        m1Doub.Add(reader.GetDouble("m1"));
        //                    }

        //                    reader.Close();

        //                    LineSeries lineSeries = new LineSeries
        //                    {
        //                        Title = "(Km/h) السرعة",
        //                        Values = new ChartValues<double>(speeds),
        //                        PointGeometry = null

        //                    };
        //                    speedChart.Series.Clear();
        //                    seriesCollection.Add(lineSeries);
        //                }

        //                speedChart.Series = seriesCollection;

        //                speedChart.AxisY.Add(new Axis
        //                {
        //                    MinValue = 0,
        //                    MaxValue = 120,
        //                    Title = "(Km/h) السرعة",
        //                    DisableAnimations = true,
        //                    LabelFormatter = value => value.ToString()
        //                });


        //                speedChart.AxisX.Add(new Axis
        //                {
        //                    FontSize = 16,
        //                    Foreground = Brushes.White,
        //                    FontWeight = FontWeights.Bold,

        //                    Title = "(Km) المسافة",
        //                    DisableAnimations = true,
        //                    Labels = m1.ToArray(),


        //                });


        //                speedChart.AxisX.Add(new Axis
        //                {
        //                    FontSize = 16,
        //                    Foreground = Brushes.White,
        //                    FontWeight = FontWeights.Bold,
        //                    Position = LiveCharts.AxisPosition.RightTop,
        //                    Title = "الزمن",
        //                    DisableAnimations = true,
        //                    Labels = times.Select(x => x.ToString("HH:mm:ss")).ToArray(),UseLayoutRounding=true,
        //                }); ;









        //            }
        //        }
        //    }
        //    catch (Exception ex) { MessageBox.Show(ex.Message); }
        //}





        private void backBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                NavigationService.Navigate(new System.Uri("/View/selectGraphes.xaml", UriKind.Relative));
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }



        private void enterBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                if (startDate.Text != "" && endDate.Text != "" && dateRdn.IsChecked == true)
                {
                    //speedChart.AxisY.Clear();
                    //speedChart.AxisX.Clear();
                    //m1.Clear();
                    //speeds.Clear();
                    //times.Clear();

                    //loadData();


                }
                else if (numUpDown.Text != "" && numUpDownTo.Text != "" && distanceRdn.IsChecked == true)
                {

                    //speedChart.AxisY.Clear();
                    //speedChart.AxisX.Clear();
                    //m1.Clear();
                    //speeds.Clear();
                    //times.Clear();

                    //loadData();

                }
                else { MessageBox.Show("يرجى تحديد التاريخ أو المسافة"); }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }


        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "PNG (*.png)|*.png";
                if (saveFileDialog.ShowDialog() == true)
                {
                    string filePath = saveFileDialog.FileName;
                    if (!filePath.EndsWith(".png"))
                    {
                        filePath += ".png";

                    }
                    //WriteToPng(speedChart, filePath);


                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        public void WriteToPng(UIElement element, string filename)
        {
            try
            {
                var rect = new Rect(element.RenderSize);
                var visual = new DrawingVisual();

                using (var dc = visual.RenderOpen())
                {
                    dc.DrawRectangle(new VisualBrush(element), null, rect);
                }

                var bitmap = new RenderTargetBitmap(
                    (int)rect.Width, (int)rect.Height, 96, 96, PixelFormats.Default);
                bitmap.Render(visual);

                var encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(bitmap));

                using (var file = File.OpenWrite(filename))
                {
                    encoder.Save(file);
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void PrintBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var printDialog = new PrintDialog();
                if (printDialog.ShowDialog() == true)
                {
                    //printDialog.PrintVisual(speedChart, "السرعة والمسافة");
                }
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
        }
        private void dateRdn_Checked(object sender, RoutedEventArgs e)
        {

            if (numUpDown != null)
            {
                numUpDown.IsEnabled = false;
                numUpDownTo.IsEnabled = false;
                numUpDown.Text = "";
                numUpDownTo.Text = "";
            }
            startDate.IsEnabled = true;
            endDate.IsEnabled = true;
            sql = "SELECT * FROM `stable` WHERE sn = '" + TrainsList.sn + "' and time BETWEEN '" + startDate.Text + "' and '" + endDate.Text + "' and  m1>0 ORDER BY time ASC ";

        }

        private void distanceRdn_Checked(object sender, RoutedEventArgs e)
        {
            if (startDate != null)
            {
                startDate.IsEnabled = false;
                endDate.IsEnabled = false;
                startDate.Text = "";
                endDate.Text = "";
            }

            numUpDown.IsEnabled = true;
            numUpDownTo.IsEnabled = true;
            sql = "SELECT * FROM `stable` WHERE sn = '" + TrainsList.sn + "' and m1 BETWEEN '" + numUpDown.Text + "' and '" + numUpDownTo.Text + "'  ORDER BY time ASC ";


        }

    }
}

