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
    /// Interaction logic for delTrainP.xaml
    /// </summary>
    public partial class delTrainP : Page
    {
        public delTrainP()
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
                        TrainsList.Items.Add(reader.GetString("sn"));

                    }
                }
            }
            catch (MySqlException ex) { MessageBox.Show("خطأ في الاتصال بقاعدة البيانات", "تحذير", MessageBoxButton.OK, MessageBoxImage.Error); }
        }

        private void DelTrainBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (TrainsList.SelectedItem != null)
                {

                    var result = MessageBox.Show($"؟ {TrainsList.SelectedItem} هل تريد حذف القاطرة", "تأكيد", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                    if (result == MessageBoxResult.Yes)
                    {
                        try
                        {
                            using (var connection = new MySqlConnection(connectionState.ConnectionString))
                            {
                                connection.Open();
                                var command2 = new MySqlCommand("DELETE FROM `screens` where sn='" + TrainsList.SelectedItem + "'", connection);
                                command2.ExecuteNonQuery();
                                MessageBox.Show($"تم حذف القاطرة {TrainsList.SelectedItem} بنجاح", "تقرير", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                                TrainsList.Items.Remove(TrainsList.SelectedItem);
                                connection.Close();
                                try
                                {
                                    NavigationService.Navigate(new System.Uri("/View/addNewTrain.xaml", UriKind.Relative));
                                }
                                catch (Exception ex) { MessageBox.Show(ex.Message); }

                            }

                        }
                        catch (Exception ex) { MessageBox.Show(ex.Message); }
                    }
                }
                else
                {

                    MessageBox.Show("قم بتحديد القطارة التي تريد حذفها", "خطأ", MessageBoxButton.OK, MessageBoxImage.Error);

                }
            }
            catch (Exception ex) { }

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
