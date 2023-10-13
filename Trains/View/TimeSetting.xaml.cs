using DocumentFormat.OpenXml.Math;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;

namespace Trains.View
{
    /// <summary>
    /// Interaction logic for TimeSetting.xaml
    /// </summary>
    public partial class TimeSetting : Page
    {
        public TimeSetting()
        {
            InitializeComponent();
            lableTrain.Text = TrainsAd.sn;
            String T2 = "", T3 = "", T4 = "", T5 = "", T6 = "";

            try
            {
                using (var connection = new MySqlConnection(connectionState.ConnectionString))
                {
                    connection.Open();
                    var command = new MySqlCommand("SELECT * FROM stable where sn='" + TrainsAd.sn + "' order by time desc limit 1", connection);
                    var reader = command.ExecuteReader();
                    var data = new List<sTable>();

                    while (reader.Read())
                    {
                        data.Add(new sTable
                        {
                            sn = reader.GetString("sn"),

                            T2 = (int.Parse(reader.GetString("T2")) / 3600) + " H",
                            T3 = (int.Parse(reader.GetString("T3")) / 3600)+ " H",
                            T4 = (int.Parse(reader.GetString("T4")) / 3600) + " H",
                            T5 = (int.Parse(reader.GetString("T5")) / 3600 )+ " H",
                            T6 = (int.Parse(reader.GetString("T6")) / 3600 )+ " H",
                        });

                        DataContext = data;
                        T2 = (int.Parse(reader.GetString("T2")) / 3600).ToString();
                        T3 = (int.Parse(reader.GetString("T3")) / 3600).ToString();
                        T4 = (int.Parse(reader.GetString("T4")) / 3600).ToString();
                        T5 = (int.Parse(reader.GetString("T5")) / 3600).ToString();
                        T6 = (int.Parse(reader.GetString("T6")) / 3600).ToString();
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

                            mT2 = reader2.GetString("mT2") + " H",
                            mT3 = reader2.GetString("mT3") + " H",
                            mT4 = reader2.GetString("mT4") + " H",
                            mT5 = reader2.GetString("mT5") + " H",
                            mT6 = reader2.GetString("mT6") + " H",
                        });
                        T2MaxCard.Text = data2[0].mT2;
                        T3MaxCard.Text = data2[0].mT3;
                        T4MaxCard.Text = data2[0].mT4;
                        T5MaxCard.Text = data2[0].mT5;
                        T6MaxCard.Text = data2[0].mT6;
                    }
                    try
                    {
                        if (int.Parse(T2)  > int.Parse(reader2.GetString("mT2"))) { T2MaxCards.Background = new SolidColorBrush(Color.FromRgb(250, 112, 112)); }
                        if (int.Parse(T3)  > int.Parse(reader2.GetString("mT3"))) { T3MaxCards.Background = new SolidColorBrush(Color.FromRgb(250, 112, 112)); }
                        if (int.Parse(T4)  > int.Parse(reader2.GetString("mT4"))) { T4MaxCards.Background = new SolidColorBrush(Color.FromRgb(250, 112, 112)); }
                        if (int.Parse(T5)  > int.Parse(reader2.GetString("mT5"))) { T5MaxCards.Background = new SolidColorBrush(Color.FromRgb(250, 112, 112)); }
                        if (int.Parse(T6)  > int.Parse(reader2.GetString("mT6"))) { T6MaxCards.Background = new SolidColorBrush(Color.FromRgb(250, 112, 112)); }
                    }
                    catch (Exception ex)
                    { }
                }
            }
            catch (MySqlException exx) { MessageBox.Show(exx.Message); }
        }

        MySqlConnection connection = new MySqlConnection(connectionState.ConnectionString);

        private void backBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                NavigationService.Navigate(new System.Uri("/View/TrainSettings.xaml", UriKind.Relative));
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void resetT2_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("؟ T2 هل تريد تأكيد عملية إعادة ضبط العداد", "تأكيد", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    using (var connection = new MySqlConnection(connectionState.ConnectionString))
                    {
                        connection.Open();
                        var command2 = new MySqlCommand("UPDATE `stable` SET `T2`='0' where sn='" + TrainsAd.sn + "' and time = (SELECT time FROM stable where sn='" + TrainsAd.sn + "' ORDER BY time DESC LIMIT 1)", connection);
                        command2.ExecuteNonQuery();
                        delNotification(TrainsAd.sn, "T2");
                        MessageBox.Show("بنجاح T2 تم إعادة ضبط العداد", "تقرير", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                        connection.Close();

                    }

                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }
            }
        }

        private void resetT3_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("؟ T3 هل تريد تأكيد عملية إعادة ضبط العداد", "تأكيد", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    using (var connection = new MySqlConnection(connectionState.ConnectionString))
                    {
                        connection.Open();
                        var command2 = new MySqlCommand("UPDATE `stable` SET `T3`= '0' where sn='" + TrainsAd.sn + "' and  time = (SELECT time FROM stable where sn = '" + TrainsAd.sn + "' ORDER BY time DESC LIMIT 1)", connection);
                        command2.ExecuteNonQuery();
                        delNotification(TrainsAd.sn, "T3");

                        MessageBox.Show("بنجاح T3 تم إعادة ضبط العداد", "تقرير", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                        connection.Close();
                    }
                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }
            }
        }

        private void resetT4_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("؟ T4 هل تريد تأكيد عملية إعادة ضبط العداد", "تأكيد", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    using (var connection = new MySqlConnection(connectionState.ConnectionString))
                    {
                        connection.Open();
                        var command2 = new MySqlCommand("UPDATE `stable` SET `T4`= '0' where sn='" + TrainsAd.sn + "' and  time = (SELECT time FROM stable where sn = '" + TrainsAd.sn + "' ORDER BY time DESC LIMIT 1)", connection);
                        command2.ExecuteNonQuery();
                        delNotification(TrainsAd.sn, "T4");

                        MessageBox.Show("بنجاح T4 تم إعادة ضبط العداد", "تقرير", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                        connection.Close();

                    }

                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }
            }
        }

        private void resetT5_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("؟ T5 هل تريد تأكيد عملية إعادة ضبط العداد", "تأكيد", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    using (var connection = new MySqlConnection(connectionState.ConnectionString))
                    {
                        connection.Open();
                        var command2 = new MySqlCommand("UPDATE `stable` SET `T5`='0' where sn='" + TrainsAd.sn + "' and  time = (SELECT time FROM stable where sn='" + TrainsAd.sn + "' ORDER BY time DESC LIMIT 1)", connection);
                        command2.ExecuteNonQuery();
                        delNotification(TrainsAd.sn, "T5");

                        MessageBox.Show("بنجاح T5 تم إعادة ضبط العداد", "تقرير", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                        connection.Close();

                    }

                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }
            }
        }

        private void resetT6_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("؟ T6 هل تريد تأكيد عملية إعادة ضبط العداد", "تأكيد", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    using (var connection = new MySqlConnection(connectionState.ConnectionString))
                    {
                        connection.Open();
                        var command2 = new MySqlCommand("UPDATE `stable` SET `T6`= '0' where sn='" + TrainsAd.sn + "' and  time = (SELECT time FROM stable where sn = '" + TrainsAd.sn + "' ORDER BY time DESC LIMIT 1)", connection);
                        command2.ExecuteNonQuery();
                        delNotification(TrainsAd.sn, "T6");

                        MessageBox.Show("بنجاح T6 تم إعادة ضبط العداد", "تقرير", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                        connection.Close();

                    }

                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }
            }
        }

        private void editT2Btn_Click(object sender, RoutedEventArgs e)
        {
            if (T2MaxNew.Text != "")
            {
                var result = MessageBox.Show("؟ T2 هل تريد تغير القيمة العظمى للعداد", "تأكيد", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        connection.Open();
                        var command = new MySqlCommand("UPDATE `ftable` SET `mT2`='" + T2MaxNew.Text + "' where sn='" + TrainsAd.sn + "'", connection);
                        command.ExecuteNonQuery();
                        MessageBox.Show("بنجاح T2 تم تغير القيمة العظمى للعداد", "تقرير", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                        //T2MaxCard.Text = T2MaxNew.Text + " H";
                        connection.Close();

                        T2MaxNew.Clear();

                    }
                    catch (Exception ex) { MessageBox.Show(ex.Message); }
                }
            }
            else { MessageBox.Show("قم بإدخال قيمة", "خطأ", MessageBoxButton.OK, MessageBoxImage.Error); }
        }

        private void editT3Btn_Click(object sender, RoutedEventArgs e)
        {
            if (T3MaxNew.Text != "")
            {
                var result = MessageBox.Show("؟ T3 هل تريد تغير القيمة العظمى للعداد", "تأكيد", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        connection.Open();
                        var command = new MySqlCommand("UPDATE `ftable` SET `mT3`='" + T3MaxNew.Text + "' where sn='" + TrainsAd.sn + "'", connection);
                        command.ExecuteNonQuery();
                        MessageBox.Show("بنجاح T3 تم تغير القيمة العظمى للعداد", "تقرير", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                        //T3MaxCard.Text = T3MaxNew.Text + " H";
                        connection.Close();

                        T2MaxNew.Clear();

                    }
                    catch (Exception ex) { MessageBox.Show(ex.Message); }
                }
            }
            else { MessageBox.Show("قم بإدخال قيمة", "خطأ", MessageBoxButton.OK, MessageBoxImage.Error); }
        }

        private void editT4Btn_Click(object sender, RoutedEventArgs e)
        {
            if (T4MaxNew.Text != "")
            {
                var result = MessageBox.Show("؟ T4 هل تريد تغير القيمة العظمى للعداد", "تأكيد", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        connection.Open();
                        var command = new MySqlCommand("UPDATE `ftable` SET `mT4`='" + T4MaxNew.Text + "' where sn='" + TrainsAd.sn + "'", connection);
                        command.ExecuteNonQuery();
                        MessageBox.Show("بنجاح T4 تم تغير القيمة العظمى للعداد", "تقرير", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                        //T4MaxCard.Text = T4MaxNew.Text + " H";
                        connection.Close();

                        T4MaxNew.Clear();

                    }
                    catch (Exception ex) { MessageBox.Show(ex.Message); }
                }
            }
            else { MessageBox.Show("قم بإدخال قيمة", "خطأ", MessageBoxButton.OK, MessageBoxImage.Error); }
        }

        private void editT5Btn_Click(object sender, RoutedEventArgs e)
        {
            if (T5MaxNew.Text != "")
            {
                var result = MessageBox.Show("؟ T5 هل تريد تغير القيمة العظمى للعداد", "تأكيد", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        connection.Open();
                        var command = new MySqlCommand("UPDATE `ftable` SET `mT5`='" + T5MaxNew.Text + "' where sn='" + TrainsAd.sn + "'", connection);
                        command.ExecuteNonQuery();
                        MessageBox.Show("بنجاح T5 تم تغير القيمة العظمى للعداد", "تقرير", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                        //T5MaxCard.Text = T5MaxNew.Text + " H";
                        connection.Close();

                        T5MaxNew.Clear();

                    }
                    catch (Exception ex) { MessageBox.Show(ex.Message); }
                }
            }
            else { MessageBox.Show("قم بإدخال قيمة", "خطأ", MessageBoxButton.OK, MessageBoxImage.Error); }
        }

        private void editT6Btn_Click(object sender, RoutedEventArgs e)
        {
            if (T6MaxNew.Text != "")
            {
                var result = MessageBox.Show("؟ T6 هل تريد تغير القيمة العظمى للعداد", "تأكيد", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        connection.Open();
                        var command = new MySqlCommand("UPDATE `ftable` SET `mT6`='" + T6MaxNew.Text + "' where sn='" + TrainsAd.sn + "'", connection);
                        command.ExecuteNonQuery();
                        MessageBox.Show("بنجاح T6 تم تغير القيمة العظمى للعداد", "تقرير", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                        //T6MaxCard.Text = T6MaxNew.Text + " H";
                        connection.Close();

                        T6MaxNew.Clear();

                    }
                    catch (Exception ex) { MessageBox.Show(ex.Message); }
                }
            }
            else { MessageBox.Show("قم بإدخال قيمة", "خطأ", MessageBoxButton.OK, MessageBoxImage.Error); }
        }
        public void delNotification(string sn, string problem)
        {


            try
            {
                using (var connection = new MySqlConnection(connectionState.ConnectionString))
                {
                    connection.Open();
                    var command2 = new MySqlCommand("DELETE FROM `notifications` where sn='" + sn + "' and problem='" + problem + "'", connection);
                    command2.ExecuteNonQuery();


                }

            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }




        }
    }
}
