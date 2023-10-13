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
using static System.Net.Mime.MediaTypeNames;

namespace Trains.View
{
    /// <summary>
    /// Interaction logic for addNewTrain.xaml
    /// </summary>
    public partial class addNewTrain : Page
    {
        private connectionState conn = new connectionState();

        public addNewTrain()
        {
            InitializeComponent();

            refreshTable();
        }
        private void refreshTable()
        {
            try
            {
                using (var connection = new MySqlConnection(connectionState.ConnectionString))
                {
                    connection.Open();
                    var command = new MySqlCommand("SELECT * FROM screens ", connection);
                    var reader = command.ExecuteReader();

                    //DataTable dataTable = new DataTable();
                    //dataTable.Load(reader);
                    var data = new List<screens>();
                    while (reader.Read())
                    {
                        data.Add(new screens
                        {
                            id = reader.GetString("id"),
                            sn = reader.GetString("sn"),
                        });
                        DataContext = data;

                    }
                    dataGrid.ItemsSource = data;

                }
            }
            catch (MySqlException ex) { MessageBox.Show($"{ex.Message}", "تحذير", MessageBoxButton.OK, MessageBoxImage.Error); }


        }


        private void backBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                NavigationService.Navigate(new System.Uri("/View/HomeAdmin.xaml", UriKind.Relative));
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void EditTrainBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                NavigationService.Navigate(new System.Uri("/View/editTrainP.xaml", UriKind.Relative));
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void deleteTrainBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                NavigationService.Navigate(new System.Uri("/View/delTrainP.xaml", UriKind.Relative));
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void addTrainBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                NavigationService.Navigate(new System.Uri("/View/addTrainP.xaml", UriKind.Relative));
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
    }
}
