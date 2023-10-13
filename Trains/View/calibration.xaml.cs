using MySql.Data.MySqlClient;
using MyToolkit.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Trains.DAL;

namespace Trains.View
{
    /// <summary>
    /// Interaction logic for calibration.xaml
    /// </summary>
    public partial class calibration : Page
    {
        public calibration()
        {
            InitializeComponent();
            oldMflowTxt.Text =  Convert.ToString(double.Parse(TrainSettings.MainFLow)*3.6) ;

        }

        private void backBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                NavigationService.Navigate(new System.Uri("/View/TrainSettings.xaml", UriKind.Relative));
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void confirmBtn_Click(object sender, RoutedEventArgs e)
        {
            if (ConValTxt.Text != "")
            {
                var result = MessageBox.Show("هل تريد تأكيد عملية ضبط المعايرة ؟", "تأكيد", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        using (var connection = new MySqlConnection(connectionState.ConnectionString))
                        {
                            connection.Open();
                            var command = new MySqlCommand("UPDATE `ftable` SET `flow`='" + MainFlowResult + "' where sn='" + TrainsAd.sn + "'", connection);
                            command.ExecuteNonQuery();
                            MessageBox.Show("تم ضبط المعايرة للقاطرة بنجاح", "تقرير", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                            connection.Close();

                        }

                    }
                    catch (Exception ex) { MessageBox.Show(ex.Message); }
                }
            }
            else { MessageBox.Show("قم بإدخال قيمة", "خطأ", MessageBoxButton.OK, MessageBoxImage.Error); }


        }

        public Double MainFlowL;
        public String MainFlowResult;
        private void calcBtn_Click(object sender, RoutedEventArgs e)
        {
            if (DateTimePickerS.Text != "" && DateTimePickerE.Text != "" && Consumption.Text != "")
            {
                try
                {
                    using (var connection = new MySqlConnection(connectionState.ConnectionString))
                    {
                        connection.Open();
                        var command = new MySqlCommand("SELECT sum(flowSec)  as mainflow,sum(seconds) as seconds,min(time) as minTime,max(time) as maxTime FROM `stable` where sn='" + TrainsAd.sn + "' and time BETWEEN '" + DateTimePickerS.Text + "' and '" + DateTimePickerE.Text + "' ORDER BY time ASC", connection);
                        var reader = command.ExecuteReader();
                        var data = new List<trains>();
                      
                            if (reader.HasRows == true)
                            {
                                while (reader.Read())
                                {
                                    data.Add(new trains
                                    {
                                        MainFlow = reader.IsDBNull(data.Count) ? null : reader.GetString("mainflow"),
                                        seconds = reader.IsDBNull(data.Count) ? null : reader.GetString("seconds"),
                                        minTime = reader.IsDBNull(data.Count) ? null : reader.GetString("minTime"),
                                        maxTime = reader.IsDBNull(data.Count) ? null : reader.GetString("maxTime"),

                                    });



                            }

                                    //DateTime startDate = DateTime.Parse(DateTimePickerS.Text);
                                    //DateTime endDate = DateTime.Parse(DateTimePickerE.Text);

                                    //TimeSpan timeElapsed = endDate - startDate;
                                    //double seconds = timeElapsed.TotalSeconds;
                            if (Consumption.Text != "" && data[0].MainFlow != null)
                            {
                                MainFlowL = ((double.Parse(data[0].MainFlow) + ((double.Parse(Consumption.Text) * (double)1000)) )/ int.Parse(data[0].seconds));
                                string result = Math.Round(MainFlowL*3.6,2).ToString();
                                MainFlowResult= Math.Round(MainFlowL,2).ToString();
                                ConValTxt.Text = result;
                                DateTimePickerS.Text = data[0].minTime.Substring(5, 4) + "-" + data[0].minTime.Substring(0, 2) + "-" + data[0].minTime.Substring(2, 2) + " " + data[0].minTime.Substring(10, 8);
                                DateTimePickerE.Text = data[0].maxTime.Substring(5, 4) + "-" + data[0].maxTime.Substring(0, 2) + "-" + data[0].maxTime.Substring(2, 2) + " " + data[0].maxTime.Substring(10, 8);
                               
                            }
                            else
                            {
                                MessageBox.Show("لايوجد قيمة ضمن هذا النطاق", "خطأ", MessageBoxButton.OK, MessageBoxImage.Error);
                                DateTimePickerS.Text = "";
                                DateTimePickerE.Text = "";
                                Consumption.Text = "";
                                ConValTxt.Text = "";
                            }   
                                
                            }
                            else
                            {

                                MessageBox.Show("لايوجد بيانات خلال الفترة المحددة", "خطأ", MessageBoxButton.OK, MessageBoxImage.Error);
                                DateTimePickerS.Text = "";
                                DateTimePickerE.Text = "";
                                Consumption.Text = "";
                                ConValTxt.Text = "";

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

                MessageBox.Show("يجب ان يكون قيم التواريخ والاستهلاك غير فارغة", "خطأ", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

       
    }
}
