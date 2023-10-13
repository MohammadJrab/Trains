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
    /// Interaction logic for addTrainP.xaml
    /// </summary>
    public partial class addTrainP : Page
    {
        public addTrainP()
        {
            InitializeComponent();
        }

        private void addTrainBtn_Click(object sender, RoutedEventArgs e)
        {

            if (idTraintxt.Text != "" && nameTraintxt.Text != "")
            {
                if (idTraintxt.Text.Length >= 1 && nameTraintxt.Text.Length >= 2)
                {
                    {
                        var result = MessageBox.Show($"؟ {nameTraintxt.Text}  هل تريد إضافة القاطرة", "تأكيد", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                        if (result == MessageBoxResult.Yes)
                        {
                            try
                            {
                                using (var connection = new MySqlConnection(connectionState.ConnectionString))
                                {
                                    connection.Open();
                                    var command = new MySqlCommand("insert into train.screens(id,sn) values('" + this.idTraintxt.Text + "','" + this.nameTraintxt.Text + "');", connection);
                                    var command2 = new MySqlCommand("insert into train.ftable(sn) values('" + this.nameTraintxt.Text + "');", connection);
                                    var command3 = new MySqlCommand("insert into train.resetsettings(sn) values('" + this.nameTraintxt.Text + "');", connection);

                                    command.ExecuteNonQuery();
                                    command2.ExecuteNonQuery();
                                    command3.ExecuteNonQuery();

                                    MessageBox.Show($"تم إضافة القاطرة  {nameTraintxt.Text} بنجاح", "تقرير", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                                    try
                                    {
                                        NavigationService.Navigate(new System.Uri("/View/addNewTrain.xaml", UriKind.Relative));
                                    }
                                    catch (Exception ex) { MessageBox.Show(ex.Message); }
                                }
                            }
                            catch (Exception ex) { MessageBox.Show("! رقم القاطرة موجود مسبقا", "خطأ", MessageBoxButton.OK, MessageBoxImage.Error); ; }
                        }
                    }
                }
                else { MessageBox.Show("يجب ان يكون عدد أحرف اسم القاطرة أكبر من حرف ", "خطأ", MessageBoxButton.OK, MessageBoxImage.Error); }

            }
            else { MessageBox.Show("قم بإدخال رقم القاطرة واسمها", "خطأ", MessageBoxButton.OK, MessageBoxImage.Error); }
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
