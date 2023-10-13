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
using Trains.DAL;

namespace Trains.View
{
    /// <summary>
    /// Interaction logic for editTrainP.xaml
    /// </summary>
    public partial class editTrainP : Page
    {
        public editTrainP()
        {
            InitializeComponent();
            try
            {
                using (var connection = new MySqlConnection(connectionState.ConnectionString))
                {
                    connection.Open();
                    var command = new MySqlCommand("SELECT * FROM screens", connection);
                    var reader = command.ExecuteReader();
                    var data = new List<trains>();
                    while (reader.Read())
                    {
                        data.Add(new trains
                        {
                            sn = reader.GetString("sn"),

                        });
                        DataContext = data;
                        TrainListC.Items.Add(reader.GetString("sn"));

                    }
                }
            }
            catch (MySqlException ex) { MessageBox.Show("خطأ في الاتصال بقاعدة البيانات", "تحذير", MessageBoxButton.OK, MessageBoxImage.Error); }
        }

        private void editTrainBtn_Click(object sender, RoutedEventArgs e)
        {
            MySqlConnection connection = new MySqlConnection(connectionState.ConnectionString);

            if (NewNametxt.Text != "")
            {
                var result = MessageBox.Show("هل تريد تغير اسم القاطرة", "تأكيد", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        connection.Open();
                        var command = new MySqlCommand("update train.screens set sn='" + this.NewNametxt.Text + "' where sn='" + this.TrainListC.Text + "'; ", connection);
                        command.ExecuteNonQuery();
                        MessageBox.Show("تم تغير اسم القاطرة بنجاح", "تقرير", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                        connection.Close();
                        try
                        {
                            NavigationService.Navigate(new System.Uri("/View/addNewTrain.xaml", UriKind.Relative));
                        }
                        catch (Exception ex) { MessageBox.Show(ex.Message); }


                    }
                    catch (Exception ex) { MessageBox.Show(ex.Message); }
                }
            }
            else { MessageBox.Show("قم بإدخال الاسم الجديد للقاطرة", "خطأ", MessageBoxButton.OK, MessageBoxImage.Error); }

        }

        private void backBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                NavigationService.Navigate(new System.Uri("/View/addNewTrain.xaml", UriKind.Relative));
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
    }
}
