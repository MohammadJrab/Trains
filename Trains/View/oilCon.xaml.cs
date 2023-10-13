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
using SaveFileDialog = Microsoft.Win32.SaveFileDialog;
using ControlzEx.Standard;
using iTextSharp.text;
using System.Net.NetworkInformation;
using PrintDialog = System.Windows.Controls.PrintDialog;
using DocumentFormat.OpenXml.EMMA;
using Button = System.Windows.Controls.Button;
using MessageBox = System.Windows.MessageBox;
using System.Globalization;
using Trains.DAL;

namespace Trains.View
{
    /// <summary>
    /// Interaction logic for oilCon.xaml
    /// </summary>
    public partial class oilCon : Page
    {
        public oilCon()
        {
            InitializeComponent();
            String snn = TrainsList.sn;
            snnLbl.Text = snn + " : القاطرة";
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

            if (startDate.Text != "" && endDate.Text != "")
            {
                try
                {
                    using (var connection = new MySqlConnection(connectionState.ConnectionString))
                    {
                        connection.Open();
                        var command = new MySqlCommand("SELECT sum(F)  as sumflow,sum(seconds) as seconds,min(time) as minTime,max(time) as maxTime,min(m1) as sM1,max(m1) as eM1 FROM `stable` where sn='" + TrainsList.sn + "' and time BETWEEN '" + startDate.Text + "' and '" + endDate.Text + "' and F>0 ORDER BY time ASC ", connection);
                        var reader = command.ExecuteReader();
                        var data = new List<trains>();

                        if (reader.HasRows == true)
                        {
                            while (reader.Read())
                            {
                                data.Add(new trains
                                {
                                    flow = reader.IsDBNull(data.Count) ? null : reader.GetString("sumflow"),
                                    seconds = reader.IsDBNull(data.Count) ? null : reader.GetString("seconds"),
                                    minTime = reader.IsDBNull(data.Count) ? null : reader.GetString("minTime"),
                                    maxTime = reader.IsDBNull(data.Count) ? null : reader.GetString("maxTime"),
                                    sM1= reader.IsDBNull(data.Count) ? null : reader.GetString("sM1"),
                                    eM1 = reader.IsDBNull(data.Count) ? null : reader.GetString("eM1"),


                                });



                            }

        


                            if ( data[0].flow != null)
                            {
                                data[0].minTime.ToString().Substring(0, 1);
                                data[0].minTime.ToString().Substring(2, 2);
                                data[0].maxTime.ToString().Substring(4, 4);
                                data[0].maxTime.ToString().Substring(5, 5);
                                data[0].maxTime.ToString().Substring(9, 1);
                                data[0].maxTime.ToString().Substring(11, 2);
                                data[0].maxTime.ToString().Substring(11, 2);
                                data[0].maxTime.ToString().Substring(14, 2);




                                DateTime sDate = DateTime.Parse(data[0].minTime.ToString());
                                DateTime eDate = DateTime.Parse(data[0].maxTime.ToString());
                                TimeSpan timeElapsed = eDate - sDate;
                                sDateTxt.Text = data[0].minTime.ToString();
                                eDateTxt.Text = data[0].maxTime.ToString();
                                double total = timeElapsed.TotalHours;
                                int hours = (int)total; 
                                int minutes = (int)((total - hours) * 60);
                                int seconds = (int)(((total - hours) * 60 - minutes) * 60);

                                string formattedTime = string.Format("{0:D2}:{1:D2}:{2:D2}", hours, minutes, seconds);

                                string resultTime = formattedTime;


                                timeH.Text =  resultTime;
                                disTxt.Text = Convert.ToString(double.Parse(data[0].eM1) - double.Parse(data[0].sM1)+" Km");
                                double fl = double.Parse(data[0].flow) / 1000;
                                string result = fl.ToString("#,0.00");
                                flowTxt.Text = result + " L";
                                startDate.Text = data[0].minTime.Substring(5, 4) + "-" + data[0].minTime.Substring(0, 2) + "-" + data[0].minTime.Substring(2, 2) + " " + data[0].minTime.Substring(10, 8);
                                endDate.Text = data[0].maxTime.Substring(5, 4) + "-" + data[0].maxTime.Substring(0, 2) + "-" + data[0].maxTime.Substring(2, 2) + " " + data[0].maxTime.Substring(10, 8);

                            }
                            else
                            {
                                MessageBox.Show("لايوجد قيمة ضمن هذا النطاق", "خطأ", MessageBoxButton.OK, MessageBoxImage.Error);
                                startDate.Text = "";
                                endDate.Text = "";
                            }

                        }
                        else
                        {

                            MessageBox.Show("لايوجد بيانات خلال الفترة المحددة", "خطأ", MessageBoxButton.OK, MessageBoxImage.Error);
                            startDate.Text = "";
                            endDate.Text = "";

                        }



                    }
                }
                catch (MySqlException ex)
                { /*MessageBox.Show("خطأ في الاتصال بقاعدة البيانات", "تحذير", MessageBoxButton.OK, MessageBoxImage.Error);*/


                    MessageBox.Show(ex.Message);
                }

            }
            else
            {

                MessageBox.Show("يجب ان يكون قيم التواريخ  غير فارغة", "خطأ", MessageBoxButton.OK, MessageBoxImage.Error);
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
    }
}
