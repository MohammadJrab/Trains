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
using Brushes = System.Windows.Media.Brushes;
using Trains.DAL;

namespace Trains.View
{
    /// <summary>
    /// Interaction logic for speed_timeC.xaml
    /// </summary>
    public partial class speed_timeC : Page
    {
        private SeriesCollection seriesCollection;
        List<double> speeds = new List<double>();
        List<DateTime> times = new List<DateTime>();
        string sql = "SELECT * FROM `stable` WHERE sn = '" + TrainsList.sn + "' ";

        public speed_timeC()
        {
            InitializeComponent();
            String snn = TrainsList.sn;
            snnLbl.Text = snn+" : القاطرة";
        }
        private void loadData()
        {
            try
            {
                if (dateRdn.IsChecked == true)
                {
                    sql = "SELECT * FROM `stable` WHERE sn = '" + TrainsList.sn + "' and time BETWEEN '" + startDate.Text + "' and '" + endDate.Text + "' and  m1>0  and F>0  ORDER BY time ASC ";
                }
                else if (distanceRdn.IsChecked == true)
                {
                    sql = "SELECT * FROM `stable` WHERE sn = '" + TrainsList.sn + "' and m1 BETWEEN '" + numUpDown.Text + "' and '" + numUpDownTo.Text + "'  and  m1>0  and F>0  ORDER BY m1 ASC ";


                }
                seriesCollection = new SeriesCollection();

                using (var connection = new MySqlConnection(connectionState.ConnectionString))
                {
                    connection.Open();
                    using (var command = new MySqlCommand(sql, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                speeds.Add(reader.GetDouble("speed"));
                                times.Add(reader.GetDateTime("time"));
                            }

                            reader.Close();

                            LineSeries lineSeries = new LineSeries
                            {
                                Title = "(Km/h) السرعة",
                                Values = new ChartValues<double>(speeds),
                                PointGeometry = null

                            };
                            speedChart.Series.Clear();
                            seriesCollection.Add(lineSeries);
                        }

                        speedChart.Series = seriesCollection;

                        // Set Y-axis label
                        speedChart.AxisY.Add(new Axis
                        {
                            FontSize = 16,
                            Foreground = Brushes.White,
                            FontWeight = FontWeights.Bold,
                            MinValue = 0,
                            MaxValue = 120,
                            Title = "(Km/h) السرعة",
                            LabelFormatter = value => value.ToString()
                        });

                        // Set X-axis label and format
                        speedChart.AxisX.Add(new Axis
                        {
                            FontSize = 15,
                            Foreground = Brushes.White,
                            FontWeight = FontWeights.Bold,
                            Title = "الزمن",
                            Labels = times.Select(x => x.ToString("yyyy-MM-dd HH:mm:ss")).ToArray()
                        });
                    }
                }

            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }


        }


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
                    speedChart.AxisY.Clear();
                    speedChart.AxisX.Clear();

                    speeds.Clear();
                    times.Clear();

                    loadData();


                }
                else if (numUpDown.Text != "" && numUpDownTo.Text != "" && distanceRdn.IsChecked == true)
                {

                    speedChart.AxisY.Clear();
                    speedChart.AxisX.Clear();

                    speeds.Clear();
                    times.Clear();

                    loadData();

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
                    WriteToPng(graph, filePath);


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
                    printDialog.PrintVisual(graph, "السرعة والزمن");
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
