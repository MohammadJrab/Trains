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

namespace Trains.View
{
    /// <summary>
    /// Interaction logic for selectGraphes.xaml
    /// </summary>
    public partial class selectGraphes : Page
    {
        public selectGraphes()
        {
            InitializeComponent();
        }

        private void backBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                NavigationService.Navigate(new System.Uri("/View/TrainsList.xaml", UriKind.Relative));
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void speed_time_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                NavigationService.Navigate(new System.Uri("/View/speed_timeC.xaml", UriKind.Relative));
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void speed_dis_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            
             try
            {
                NavigationService.Navigate(new System.Uri("/View/speed_disC.xaml", UriKind.Relative));
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void speed_dis_time_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                NavigationService.Navigate(new System.Uri("/View/speed_time_disC.xaml", UriKind.Relative));
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void table_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                NavigationService.Navigate(new System.Uri("/View/table.xaml", UriKind.Relative));
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void speed_Br_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                NavigationService.Navigate(new System.Uri("/View/speed_Br.xaml", UriKind.Relative));
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void oilC_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                NavigationService.Navigate(new System.Uri("/View/oilCon.xaml", UriKind.Relative));
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
    }
}
