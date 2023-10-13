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
using PrintDialog = System.Windows.Controls.PrintDialog;
using Button = System.Windows.Controls.Button;
using LiveCharts.Definitions.Charts;
using DocumentFormat.OpenXml.Spreadsheet;
using Page = System.Windows.Controls.Page;
using Colors = System.Windows.Media.Colors;
using Brushes = System.Windows.Media.Brushes;
using LiveCharts.Maps;
using Xceed.Wpf.Toolkit.Core.Converters;
using DocumentFormat.OpenXml.Math;
using Microsoft.Office.Interop.Excel;
using Axis = LiveCharts.Wpf.Axis;
using SeriesCollection = LiveCharts.SeriesCollection;
using System.Runtime.InteropServices.ComTypes;
using Trains.DAL;

namespace Trains.View
{
    /// <summary>
    /// Interaction logic for speed_Br.xaml
    /// </summary>
    public partial class speed_Br : Page
    {
        private SeriesCollection seriesCollection;
        List<double> speeds = new List<double>();
        List<double> br1 = new List<double>();
        List<double> br2 = new List<double>();
        List<DateTime> times = new List<DateTime>();
        public speed_Br()
        {
            InitializeComponent();
            String snn = TrainsList.sn;
            snnLbl.Text = snn + " : القاطرة";
        }
       public SeriesCollection  SeriesCollection;
        string sql = "SELECT * FROM `stable` WHERE sn = '" + TrainsList.sn +"' ";

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
                    sql = "SELECT * FROM `stable` WHERE sn = '" + TrainsList.sn + "' and m1 BETWEEN '" + numUpDown.Text + "' and '" + numUpDownTo.Text + "' and m1>0  and F>0  ORDER BY m1 ASC ";


                }

                using (var connection = new MySqlConnection(connectionState.ConnectionString))
                {
                    seriesCollection = new SeriesCollection();


                     connection.Open();
                    using (var command = new MySqlCommand(sql, connection))
                    {
                        using (var reader =  command.ExecuteReader())
                        {

                            while ( reader.Read())
                            {
                                speeds.Add(reader.GetDouble("speed"));
                                times.Add(reader.GetDateTime("time"));
                                br1.Add(reader.GetDouble("br1") * 10);
                                br2.Add(reader.GetDouble("br2")* 10);

                            }

                            reader.Close();


                            LineSeries Sspeed = new LineSeries
                            {
                                Title = "(Km/h) السرعة",
                                Values = new ChartValues<double>(speeds),
                                PointGeometry = null,


                            };

                            //LineSeries lineSeriesBr1 = new LineSeries
                            //{

                            //    Title = "لجام مباشر",
                            //    Values = new ChartValues<int>(br1),
                            //    PointGeometry = null

                            //};
                            //var Brdirect= new LiveCharts.Wpf.StepLineSeries
                            //{
                            //    Title = "لجام مباشر",

                            //    Values = new ChartValues<int>(br1),
                            //    Fill = null,

                            //};
                            //var series1 = new LiveCharts.Wpf.LineSeries()
                            //{
                            //    Title = "لجام مباشر",
                            //    Fill = null,
                            //    Opacity = 100,
                            //    Values = new ChartValues<int>(br1),
                            //};
                            //var BrAuto = new LiveCharts.Wpf.StepLineSeries
                            //{
                            //    Title = "لجام آلي",
                            //    Values = new ChartValues<int>(br2),
                            //    Fill = null,
                            //};

                            var colSer = new LiveCharts.Wpf.StepLineSeries
                            {
                                  Title = "لجام مباشر",
                                Values = new ChartValues<double>(br2),
                              MinHeight=0,MaxHeight=30,
                                PointGeometry = null,
                                Fill = Brushes.Transparent,
                                Effect = null,



                            };
                            var colSer2 = new LiveCharts.Wpf.StepLineSeries
                            {
                                Title = "لجام آلي",

                                Values = new ChartValues<double>(br1),
                                MinHeight = 0,
                                MaxHeight = 30,
                                PointGeometry = null,
                                Fill = Brushes.Transparent,
                                Effect = null,


                            };

                            speedChart.Series.Clear();

                            seriesCollection.Add(Sspeed);
                            seriesCollection.Add(colSer);
                            seriesCollection.Add(colSer2);



                            //speedChart.Series.Values.AddRange(new Series[3] { lineSeries, series3, lineSeriesBr2 });
                            //speedChart.Series.AddRange(new Series[3] { Sspeed, Brdirect, BrAuto });

                        }

                        speedChart.Series = seriesCollection;

                        speedChart.AxisY.Add(new Axis
                        {   FontSize = 16,
                            Foreground = Brushes.White,
                            FontWeight = FontWeights.Bold,
                            MinValue = 0,
                            MaxValue = 120,
                            Title = "(Km/h) السرعة",
                            LabelFormatter = value => value.ToString()
                        });


                        speedChart.AxisX.Add(new Axis
                        {
                            FontSize = 12,
                            Foreground = Brushes.White,
                            FontWeight = FontWeights.Bold,
                            Title = "الزمن",
                            Labels = times.Select(x => x.ToString("yyyy-MM-dd HH:mm:ss tt")).ToArray()
                        });

                    }
                }

            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }


        private void backBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                NavigationService.Navigate(new System.Uri("/View/selectGraphes.xaml", UriKind.Relative));
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }


        private async void enterBtn_Click(object sender, RoutedEventArgs e)
        {
            ((Button)sender).IsEnabled = false;

            try
            {


                if (startDate.Text != "" && endDate.Text != "" && dateRdn.IsChecked == true)
                {
                    speedChart.AxisY.Clear();
                    speedChart.AxisX.Clear();
                    br1.Clear();
                    br2.Clear();
                    speeds.Clear();
                    times.Clear();

                    loadData();


                }
                else if (numUpDown.Text != "" && numUpDownTo.Text != ""&& distanceRdn.IsChecked==true) 
                {

                    speedChart.AxisY.Clear();
                    speedChart.AxisX.Clear();
                    br1.Clear();
                    br2.Clear();
                    speeds.Clear();
                    times.Clear();

                    loadData();

                }
                else { MessageBox.Show("يرجى تحديد التاريخ أو المسافة"); }



            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                ((Button)sender).IsEnabled = true;
            }

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

        private void PrintBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var printDialog = new PrintDialog();
                if (printDialog.ShowDialog() == true)
                {
                    printDialog.PrintVisual(graph, "السرعة واللجام");
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
                numUpDown.Text="";
                numUpDownTo.Text="";
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
