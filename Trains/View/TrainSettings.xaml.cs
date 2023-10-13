using MaterialDesignThemes.Wpf;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
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
using Trains.DAL;

namespace Trains.View
{
    /// <summary>
    /// Interaction logic for TrainSettings.xaml
    /// </summary>
    public partial class TrainSettings : Page
    {
        public static String MainFLow;
        public TrainSettings()
        {
            InitializeComponent();
            lableTrain.Text = TrainsAd.sn;
            String m2="", m3="", m4 = "", m5 = "", m6 = "" ;
            try
            {
                using (var connection = new MySqlConnection(connectionState.ConnectionString))
                {
                    connection.Open();
                    var command = new MySqlCommand("SELECT * FROM stable where sn='" + TrainsAd.sn + "' order by time desc limit 1 ", connection);
                    var reader = command.ExecuteReader();
                    var data = new List<sTable>();

                    while (reader.Read())
                    {
                        data.Add(new sTable
                        {
                            sn = reader.GetString("sn"),
                            m1 = reader.GetString("m1")+ " Km",
                            m2 = reader.GetString("m2")+ " Km",
                            m3 = reader.GetString("m3")+ " Km",
                            m4 = reader.GetString("m4")+ " Km",
                            m5 = reader.GetString("m5")+ " Km",
                            m6 = reader.GetString("m6")+ " Km",
                            T1 = reader.GetInt32("T1")/3600 + " H",
                            T2 = reader.GetInt32("T2") /3600+ " H",
                            T3 = reader.GetInt32("T3") / 3600 + " H",
                            T4 = reader.GetInt32("T4") / 3600 + " H",
                            T5 = reader.GetInt32("T5") / 3600 + " H",
                            T6 = reader.GetInt32("T6") / 3600 + " H",
                            L = reader.GetString("L")+"%",

                        });

                        DataContext = data;
                        m2 = reader.GetString("m2");
                        m3 = reader.GetString("m3");
                        m4=  reader.GetString("m4");
                        m5 = reader.GetString("m5");
                        m6 = reader.GetString("m6");
                    }
                }
            }
            catch (MySqlException ex) { MessageBox.Show("خطأ في الاتصال بقاعدة البيانات", "تحذير", MessageBoxButton.OK, MessageBoxImage.Error); }
            try
            {
                using (var connection2 = new MySqlConnection(connectionState.ConnectionString))
                {
                    connection2.Open();
                    var command2 = new MySqlCommand("SELECT * FROM ftable where sn='" + TrainsAd.sn + "'", connection2);
                    var reader2 = command2.ExecuteReader();
                    var data2 = new List<ftable>();
                    while (reader2.Read())
                    {
                        data2.Add(new ftable
                        {
                            sn = reader2.GetString("sn"),
                            mM2 = reader2.GetString("mM2") + " Km",
                            mM3 = reader2.GetString("mM3") + " Km",
                            mM4 = reader2.GetString("mM4") + " Km",
                            mM5 = reader2.GetString("mM5") + " Km",
                            mM6 = reader2.GetString("mM6") + " Km",
                            mT2 = reader2.GetString("mT2") + " H",
                            mT3 = reader2.GetString("mT3") + " H",
                            mT4 = reader2.GetString("mT4") + " H",
                            mT5 = reader2.GetString("mT5") + " H",
                            mT6 = reader2.GetString("mT6") + " H",
                            flow = reader2.GetString("flow"),

                        });
                        M2MaxCard.Text = data2[0].mM2;
                        M3MaxCard.Text = data2[0].mM3;
                        M4MaxCard.Text = data2[0].mM4;
                        M5MaxCard.Text = data2[0].mM5;
                        M6MaxCard.Text = data2[0].mM6;
                        MainFLow = data2[0].flow;
                    }
                    try
                    {
                        if (Double.Parse(m2)  > Double.Parse(reader2.GetString("mM2"))) { M2MaxCards.Background = new SolidColorBrush(Color.FromRgb(250, 112, 112)); }
                        if (Double.Parse(m3)  > Double.Parse(reader2.GetString("mM3"))) { M3MaxCards.Background = new SolidColorBrush(Color.FromRgb(250, 112, 112)); }
                        if (Double.Parse(m4)  > Double.Parse(reader2.GetString("mM4"))) { M4MaxCards.Background = new SolidColorBrush(Color.FromRgb(250, 112, 112)); }
                        if (Double.Parse(m5)  > Double.Parse(reader2.GetString("mM5"))) { M5MaxCards.Background = new SolidColorBrush(Color.FromRgb(250, 112, 112)); }
                        if (Double.Parse(m6)  > Double.Parse(reader2.GetString("mM6"))) { M6MaxCards.Background = new SolidColorBrush(Color.FromRgb(250, 112, 112)); }
                    }
                    catch (Exception ex)
                    { }
                }
            }
            catch (MySqlException exx) { MessageBox.Show(exx.Message); }


        }
        MySqlConnection connection = new MySqlConnection(connectionState.ConnectionString);



        public void delNotification(string sn,string problem)
        {

          
             try
             {
                 using (var connection = new MySqlConnection(connectionState.ConnectionString))
                 {
                     connection.Open();
                     var command2 = new MySqlCommand("DELETE FROM `notifications` where sn='" +sn + "' and problem='"+problem+"'", connection);
                     command2.ExecuteNonQuery();
                  

                 }

             }
             catch (Exception ex) { MessageBox.Show(ex.Message); }
        



        }


        private void backBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                NavigationService.Navigate(new System.Uri("/View/TrainsAd.xaml", UriKind.Relative));
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void resetm2_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("؟ M2 هل تريد تأكيد عملية إعادة ضبط العداد", "تأكيد", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    using (var connection = new MySqlConnection(connectionState.ConnectionString))
                    {
                        connection.Open();
                        var command2 = new MySqlCommand("UPDATE `stable` SET `m2`= '0' where sn='" + TrainsAd.sn + "' and  time = (SELECT time FROM stable where sn = '" + TrainsAd.sn + "' ORDER BY time DESC LIMIT 1)", connection);
                        command2.ExecuteNonQuery();
                        delNotification(TrainsAd.sn, "m2");
                        MessageBox.Show("بنجاح M2 تم إعادة ضبط العداد", "تقرير", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                        m2txt.Text = "0 Km";
                        connection.Close();

                    }

                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }
            }

        }

        private void resetm3_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("؟ M3 هل تريد تأكيد عملية إعادة ضبط العداد", "تأكيد", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    using (var connection = new MySqlConnection(connectionState.ConnectionString))
                    {
                        connection.Open();
                        var command2 = new MySqlCommand("UPDATE `stable` SET `m3`= '0' where sn='" + TrainsAd.sn + "' and  time = (SELECT time FROM stable where sn = '" + TrainsAd.sn + "' ORDER BY time DESC LIMIT 1)", connection);
                        command2.ExecuteNonQuery();
                        delNotification(TrainsAd.sn, "m3");

                        MessageBox.Show("بنجاح M3 تم إعادة ضبط العداد", "تقرير", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                        m3txt.Text = "0 Km";
                        connection.Close();

                    }

                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }
            }
        }

        private void resetm4_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("؟ M4 هل تريد تأكيد عملية إعادة ضبط العداد", "تأكيد", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    using (var connection = new MySqlConnection(connectionState.ConnectionString))
                    {
                        connection.Open();
                        var command2 = new MySqlCommand("UPDATE `stable` SET `m4`= '0' where sn='" + TrainsAd.sn + "' and  time = (SELECT time FROM stable where sn = '" + TrainsAd.sn + "' ORDER BY time DESC LIMIT 1)", connection);
                        command2.ExecuteNonQuery();
                        delNotification(TrainsAd.sn, "m4");

                        MessageBox.Show("بنجاح M4 تم إعادة ضبط العداد", "تقرير", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                        m4txt.Text = "0 Km";
                        connection.Close();

                    }

                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }
            }
        }

        private void resetm5_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("؟ M5 هل تريد تأكيد عملية إعادة ضبط العداد", "تأكيد", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    using (var connection = new MySqlConnection(connectionState.ConnectionString))
                    {
                        connection.Open();
                        var command2 = new MySqlCommand("UPDATE `stable` SET `m5`= '0' where sn='" + TrainsAd.sn + "' and  time = (SELECT time FROM stable where sn = '" + TrainsAd.sn + "' ORDER BY time DESC LIMIT 1)", connection);
                        command2.ExecuteNonQuery();
                        delNotification(TrainsAd.sn, "m5");

                        MessageBox.Show("بنجاح M5 تم إعادة ضبط العداد", "تقرير", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                        m5txt.Text = "0 Km";
                        connection.Close();

                    }

                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }
            }
        }

        private void resetm6_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("؟ M6 هل تريد تأكيد عملية إعادة ضبط العداد", "تأكيد", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    using (var connection = new MySqlConnection(connectionState.ConnectionString))
                    {
                        connection.Open();
                        var command2 = new MySqlCommand("UPDATE `stable` SET `m6`= '0' where sn='" + TrainsAd.sn + "' and  time = (SELECT time FROM stable where sn = '" + TrainsAd.sn + "' ORDER BY time DESC LIMIT 1)", connection);
                        command2.ExecuteNonQuery();
                        delNotification(TrainsAd.sn, "m6");

                        MessageBox.Show("بنجاح M6 تم إعادة ضبط العداد", "تقرير", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                        m6txt.Text = "0 Km";
                        connection.Close();

                    }

                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }
            }
        }

        private void editM2Btn_Click(object sender, RoutedEventArgs e)
        {
            if (m2MaxNew.Text != "")
            {
                var result = MessageBox.Show("؟ M2 هل تريد تغير القيمة العظمى للعداد", "تأكيد", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        connection.Open();
                        var command = new MySqlCommand("UPDATE `ftable` SET `mM2`='" + m2MaxNew.Text + "' where sn='" + TrainsAd.sn + "'", connection);
                        command.ExecuteNonQuery();
                        MessageBox.Show("بنجاح M2 تم تغير القيمة العظمى للعداد", "تقرير", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                        M2MaxCard.Text = m2MaxNew.Text + " Km";
                        connection.Close();

                        m2MaxNew.Clear();

                    }
                    catch (Exception ex) { MessageBox.Show(ex.Message); }
                }
            }
            else { MessageBox.Show("قم بإدخال قيمة", "خطأ", MessageBoxButton.OK, MessageBoxImage.Error); }
        }

        private void editM3Btn_Click(object sender, RoutedEventArgs e)
        {
            if (m3MaxNew.Text != "") { 
            var result = MessageBox.Show("؟ M3 هل تريد تغير القيمة العظمى للعداد", "تأكيد", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    connection.Open();
                    var command = new MySqlCommand("UPDATE `ftable` SET `mM3`='" + m3MaxNew.Text + "' where sn='" + TrainsAd.sn + "'", connection);
                    command.ExecuteNonQuery();
                    MessageBox.Show("بنجاح M3 تم تغير القيمة العظمى للعداد", "تقرير", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                    M3MaxCard.Text = m3MaxNew.Text + " Km";
                    connection.Close();

                    m3MaxNew.Clear();

                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }
            }
        }
                    else { MessageBox.Show("قم بإدخال قيمة", "خطأ", MessageBoxButton.OK, MessageBoxImage.Error); }

}

private void editM4Btn_Click(object sender, RoutedEventArgs e)
        {
            if (m4MaxNew.Text != "")
            {

                var result = MessageBox.Show("؟ M4 هل تريد تغير القيمة العظمى للعداد", "تأكيد", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        connection.Open();
                        var command = new MySqlCommand("UPDATE `ftable` SET `mM4`='" + m4MaxNew.Text + "' where sn='" + TrainsAd.sn + "'", connection);
                        command.ExecuteNonQuery();
                        MessageBox.Show("بنجاح M4 تم تغير القيمة العظمى للعداد", "تقرير", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                        M4MaxCard.Text = m4MaxNew.Text + " Km";
                        connection.Close();

                        m4MaxNew.Clear();

                    }
                    catch (Exception ex) { MessageBox.Show(ex.Message); }
                }
            }
            else { MessageBox.Show("قم بإدخال قيمة", "خطأ", MessageBoxButton.OK, MessageBoxImage.Error); }


        }

        private void editM5Btn_Click(object sender, RoutedEventArgs e)
        {
            if (m5MaxNew.Text != "")
            {

                var result = MessageBox.Show("؟ M5 هل تريد تغير القيمة العظمى للعداد", "تأكيد", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        connection.Open();
                        var command = new MySqlCommand("UPDATE `ftable` SET `mM5`='" + m5MaxNew.Text + "' where sn='" + TrainsAd.sn + "'", connection);
                        command.ExecuteNonQuery();
                        MessageBox.Show("بنجاح M5 تم تغير القيمة العظمى للعداد", "تقرير", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                        M5MaxCard.Text = m5MaxNew.Text + " Km";
                        connection.Close();
                        m5MaxNew.Clear();

                    }
                    catch (Exception ex) { MessageBox.Show(ex.Message); }
                }
            }
            else { MessageBox.Show("قم بإدخال قيمة", "خطأ", MessageBoxButton.OK, MessageBoxImage.Error); }

        }

        private void editM6Btn_Click(object sender, RoutedEventArgs e)
        {
            if (m6MaxNew.Text != "")
            {
                var result = MessageBox.Show("؟ M6 هل تريد تغير القيمة العظمى للعداد", "تأكيد", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        connection.Open();
                        var command = new MySqlCommand("UPDATE `ftable` SET `mM6`='" + m6MaxNew.Text + "' where sn='" + TrainsAd.sn + "'", connection);
                        command.ExecuteNonQuery();
                        MessageBox.Show("بنجاح M6 تم تغير القيمة العظمى للعداد", "تقرير", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                        M6MaxCard.Text = m6MaxNew.Text + " Km";
                        connection.Close();

                        m6MaxNew.Clear();
                    }
                    catch (Exception ex) { MessageBox.Show(ex.Message); }
                }
            }
            else { MessageBox.Show("قم بإدخال قيمة", "خطأ", MessageBoxButton.OK, MessageBoxImage.Error); }

        }

        //private void RefBtn_Click(object sender, RoutedEventArgs e)
        //{
        //    if (numUpDown.Text != "")
        //    {
        //        if (int.Parse(numUpDown.Text) > 10)
        //        {
        //            var result = MessageBox.Show($" هل تريد تغير زمن التخزين ل  {numUpDown.Text} ؟", "تأكيد", MessageBoxButton.YesNo, MessageBoxImage.Warning);
        //            if (result == MessageBoxResult.Yes)
        //            {
        //                try
        //                {
        //                    connection.Open();
        //                    var command = new MySqlCommand("UPDATE `ftable` SET `refresh`='" + numUpDown.Text + "' where sn='" + TrainsAd.sn + "'", connection);
        //                    command.ExecuteNonQuery();
        //                    MessageBox.Show("تم تغير تغير زمن التخزين بنجاح", "تقرير", MessageBoxButton.OK, MessageBoxImage.Asterisk);
        //                    refreshTxt.Text = numUpDown.Text;
        //                    connection.Close();
        //                }
        //                catch (Exception ex) { MessageBox.Show(ex.Message); }
        //            }
        //            else { numUpDown.Text = "12"; }
        //        }
        //        else { MessageBox.Show("! قم بإدخال قيمة أكبر او تساوي 12", "خطأ", MessageBoxButton.OK, MessageBoxImage.Error); }
        //    }
        //    else { MessageBox.Show("قم بإدخال قيمة","خطأ",MessageBoxButton.OK,MessageBoxImage.Error); }

        //}

        private void calibrationBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {

              
                NavigationService.Navigate(new System.Uri("/View/calibration.xaml", UriKind.Relative));
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
           
        }

        private void timesBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                NavigationService.Navigate(new System.Uri("/View/TimeSetting.xaml", UriKind.Relative));
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void noti_Click(object sender, RoutedEventArgs e)
        {

        }
    }

}
