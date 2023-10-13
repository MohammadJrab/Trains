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
using System.Windows.Shapes;
using Trains.DAL;

namespace Trains.View
{
    /// <summary>
    /// Interaction logic for login.xaml
    /// </summary>
    public partial class login : Window
    {
        private String username, password, sql, sql2;
        private connectionState conn = new connectionState();
        private MySqlCommand command;

        public login()
        {
            InitializeComponent();
            userTxtbx.Focus();
        }


        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            DragMove();
        }

        private void Exit_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            App.Current.MainWindow.Close();

        }

        private void Min_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            App.Current.MainWindow.WindowState = WindowState.Minimized;

        }

        private void loginBtn_Click(object sender, RoutedEventArgs e)
        {
            username = userTxtbx.Text;
            password = passwordTxbx.Password;
            if (String.IsNullOrEmpty(username) || String.IsNullOrEmpty(password))
            {

                MessageBox.Show("اسم المستخدم او كلمة المرور فارغة");
            }
            else
            {

                sql = "Select username,password from users where username='" + username + "'and password='" + password + "'";
                sql2 = "Select permission from users where username='" + username + "'";

                if (conn.OpenConnection() == true)
                {
                    try
                    {

                        command = new MySqlCommand(sql, conn.get_Connection());
                        object a = command.ExecuteScalar();
                        if (a == null)
                        {

                            MessageBox.Show("خطأ في اسم المستخدم او كلمة السر");
                            userTxtbx.Focus();

                        }
                        else
                        {

                            command = new MySqlCommand(sql2, conn.get_Connection());
                            object b = command.ExecuteScalar();

                                MainWindow mn = new MainWindow();
                                users.perUser=b.ToString();
                                this.Close();
                                mn.Show();
                            


                        }

                    }
                    catch (MySqlException ex)
                    {
                        MessageBox.Show(ex.Message);
                    }

                }
                else if (conn.OpenConnection() == false)
                {
                    MessageBox.Show("خطأ بالسيرفر", "تحذير", MessageBoxButton.OK, MessageBoxImage.Error);
                    userTxtbx.Focus();

                }
            }
            userTxtbx.Text = "";
            passwordTxbx.Password = "";
            conn.close_conn();
        }
    }
}
