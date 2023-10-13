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
    /// Interaction logic for delUser.xaml
    /// </summary>
    public partial class delUser : Page
    {
        public delUser()
        {
            InitializeComponent();
            try
            {
                using (var connection = new MySqlConnection(connectionState.ConnectionString))
                {
                    connection.Open();
                    var command = new MySqlCommand("SELECT * FROM users where permission='مستخدم' ", connection);
                    var reader = command.ExecuteReader();
                    var data = new List<users>();
                    while (reader.Read())
                    {
                        data.Add(new users
                        {
                            username = reader.GetString("username"),

                        });
                        DataContext = data;
                        UsersList.Items.Add(reader.GetString("username"));

                    }
                }
            }
            catch (MySqlException ex) { MessageBox.Show("خطأ في الاتصال بقاعدة البيانات", "تحذير", MessageBoxButton.OK, MessageBoxImage.Error); }
        }

        private void delUserBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (UsersList.SelectedItem != null)
                {

                    var result = MessageBox.Show($"؟ {UsersList.SelectedItem} هل تريد حذف المستخدم  ", "تأكيد", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                    if (result == MessageBoxResult.Yes)
                    {
                        try
                        {
                            using (var connection = new MySqlConnection(connectionState.ConnectionString))
                            {
                                connection.Open();
                                var command2 = new MySqlCommand("DELETE FROM `users` where username='" + UsersList.SelectedItem + "'", connection);
                                command2.ExecuteNonQuery();
                                MessageBox.Show($"تم حذف المستخدم {UsersList.SelectedItem} بنجاح", "تقرير", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                                UsersList.Items.Remove(UsersList.SelectedItem);
                                connection.Close();
                                try
                                {
                                    NavigationService.Navigate(new System.Uri("/View/usersManager.xaml", UriKind.Relative));
                                }
                                catch (Exception ex) { MessageBox.Show(ex.Message); }

                            }

                        }
                        catch (Exception ex) { MessageBox.Show(ex.Message); }
                    }
                }
                else
                {

                    MessageBox.Show("قم بتحديد المستخدم التي تريد حذفه", "خطأ", MessageBoxButton.OK, MessageBoxImage.Error);

                }
            }
            catch (Exception ex) { }
        }

        private void backBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                NavigationService.Navigate(new System.Uri("/View/usersManager.xaml", UriKind.Relative));
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }

        }
    }
}
