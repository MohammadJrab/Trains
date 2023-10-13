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
    /// Interaction logic for addUserWin.xaml
    /// </summary>
    public partial class addUserWin : Page
    {
        public addUserWin()
        {
            InitializeComponent();
        }

        private void addUserBtn_Click(object sender, RoutedEventArgs e)
        {
            if (usertxt.Text != "" && passtxt.Text != "")
            {
                if (usertxt.Text.Length >= 1 && passtxt.Text.Length >= 2)
                {
                    {
                        var result = MessageBox.Show($"؟ {usertxt.Text} هل تريد إضافة المستخدم", "تأكيد", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                        if (result == MessageBoxResult.Yes)
                        {
                           
                                using (var connection = new MySqlConnection(connectionState.ConnectionString))
                                {
                                    connection.Open();
                                    var command = new MySqlCommand("insert into train.users(username,password,permission) values('" + this.usertxt.Text + "','" + this.passtxt.Text + "','مستخدم');", connection);
                                var reader = command.ExecuteNonQuery();
                                MessageBox.Show($"تم إضافة المستخدم  {usertxt.Text} بنجاح", "تقرير", MessageBoxButton.OK, MessageBoxImage.Asterisk);

                                try
                                {
                                        NavigationService.Navigate(new System.Uri("/View/usersManager.xaml", UriKind.Relative));
                                    }
                                    catch (Exception ex) { MessageBox.Show(ex.Message); }
                                }
                            
                        }
                    }
                }
                else { MessageBox.Show("يجب ان يكون عدد أحرف اسم المستخدم أكبر من حرف ", "خطأ", MessageBoxButton.OK, MessageBoxImage.Error); }

            }
            else { MessageBox.Show("قم بإدخال اسم المستخدم وكلمة المرور", "خطأ", MessageBoxButton.OK, MessageBoxImage.Error); }

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
