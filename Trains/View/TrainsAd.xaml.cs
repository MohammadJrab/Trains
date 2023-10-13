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
    /// Interaction logic for TrainsAd.xaml
    /// </summary>
    public partial class TrainsAd : Page
    {
        public TrainsAd()
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
                    }
                    listView.ItemsSource = data;
                }
            }
            catch (MySqlException ex) { MessageBox.Show("خطأ في الاتصال بقاعدة البيانات", "تحذير", MessageBoxButton.OK, MessageBoxImage.Error); }
            listView.SizeChanged += (s, e) =>
            {
                var gridView = listView.View as GridView;
                if (gridView != null)
                {
                    foreach (var column in gridView.Columns)
                    {
                        column.Width = 150;
                    }
                }
            };


        }
        public static String sn;

        private void listView_PreviewMouseDoubleClick_1(object sender, MouseButtonEventArgs e)
        {
            try
            {
                trains train = (trains)listView.SelectedItems[0];
                sn = train.sn;
                NavigationService.Navigate(new System.Uri("/View/TrainSettings.xaml", UriKind.Relative), sn);
            }
            catch (Exception ex) { }

        }

        private void backBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                NavigationService.Navigate(new System.Uri("/View/HomeAdmin.xaml", UriKind.Relative));
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
    }

}
