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
    /// Interaction logic for editAdminAC.xaml
    /// </summary>
    public partial class editAdminAC : Page
    {
        public editAdminAC()
        {
            InitializeComponent();
            try
            {
                using (var connection = new MySqlConnection(connectionState.ConnectionString))
                {
                    connection.Open();
                    var command = new MySqlCommand("SELECT * FROM users where permission='مدير' ", connection);
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
            MySqlConnection connection = new MySqlConnection(connectionState.ConnectionString);

            if (passtxt.Text != "")
            {
                var result = MessageBox.Show("هل تريد تغير كلمة المرور", "تأكيد", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        connection.Open();
                        var command = new MySqlCommand("update train.users set password='" + this.passtxt.Text + "' where username='" + this.UsersList.Text + "'; ", connection);
                        command.ExecuteNonQuery();
                        MessageBox.Show("تم تغير كلمة المرور بنجاح", "تقرير", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                        connection.Close();
                        try
                        {
                            NavigationService.Navigate(new System.Uri("/View/HomeAdmin.xaml", UriKind.Relative));
                        }
                        catch (Exception ex) { MessageBox.Show(ex.Message); }


                    }
                    catch (Exception ex) { MessageBox.Show(ex.Message); }
                }
            }
            else { MessageBox.Show("قم بإدخال كلمة المرور الجديدة", "خطأ", MessageBoxButton.OK, MessageBoxImage.Error); }
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