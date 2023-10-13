using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
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
    /// Interaction logic for usersManager.xaml
    /// </summary>
    public partial class usersManager : Page
    {
        public usersManager()
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
                    var command = new MySqlCommand("SELECT * FROM users where permission='مستخدم' ", connection);
                    var reader = command.ExecuteReader();

                    //DataTable dataTable = new DataTable();
                    //dataTable.Load(reader);
                    var data = new List<users>();
                    while (reader.Read())
                    {
                        data.Add(new users
                        {
                            username = reader.GetString("username"),
                            password = reader.GetString("password"),
                            permission = reader.GetString("permission"),
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

        private void addUserBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                NavigationService.Navigate(new System.Uri("/View/addUserWin.xaml", UriKind.Relative));
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void EditUserBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                NavigationService.Navigate(new System.Uri("/View/editUser.xaml", UriKind.Relative));
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void deleteUserBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                NavigationService.Navigate(new System.Uri("/View/delUser.xaml", UriKind.Relative));
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
    }
}
