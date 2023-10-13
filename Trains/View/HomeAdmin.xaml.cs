using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
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
    /// Interaction logic for HomeAdmin.xaml
    /// </summary>
    public partial class HomeAdmin : Page
    {
        private Timer timerApp;

        public HomeAdmin()
        {
            InitializeComponent();
            if (users.perUser == "مدير")
            {
                AddTrain.IsEnabled = true;
                Settings.IsEnabled=true;
                AddAccount.IsEnabled = true;
                EditAccount.IsEnabled = true;
                EditAccount.IsEnabled = true;

            }
            timerApp = new Timer(3000); // 1000 milliseconds = 1 second
            timerApp.Elapsed += new ElapsedEventHandler(OnTimerElapsed);
            timerApp.AutoReset = true;
            timerApp.Enabled = true;

        }

        private void OnTimerElapsed(object sender, ElapsedEventArgs e)
        {
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += Worker_DoWork;
            worker.RunWorkerAsync();
        }
        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {




            Task task = DoTaskAsync();
            task.Start();

        }
        public async Task DoTaskAsync()
        {

            try
            {

                using (var connection = new MySqlConnection(connectionState.ConnectionString))
                {
                    connection.Open();
                    var command = new MySqlCommand("SELECT Count(problem) FROM notifications", connection);
                    //var reader = command.ExecuteReader();
                    int count = int.Parse(command.ExecuteScalar().ToString());
                    
                    if (count > 0)
                    {
                        this.Dispatcher.Invoke(() =>
                        {
                            CountNotifications.Text = count.ToString();

                            Notifications.Foreground = new SolidColorBrush(Color.FromRgb(250, 112, 112));

                        });
                        await Task.Delay(1000);
                        this.Dispatcher.Invoke(() =>
                        {
                            Notifications.Foreground = new SolidColorBrush(Color.FromRgb(232, 232, 232));

                        });

                    }




                }

            }
            catch (Exception ex) { System.Windows.MessageBox.Show($"{ex.Message}", "تحذير", MessageBoxButton.OK, MessageBoxImage.Error); }


        }

            private void SelectTrain_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                NavigationService.Navigate(new System.Uri("/View/TrainsList.xaml", UriKind.Relative));
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void Settings_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            NavigationService.Navigate(new Uri("/View/TrainsAd.xaml", UriKind.Relative));
        }

        private void AddAccount_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                NavigationService.Navigate(new System.Uri("/View/usersManager.xaml", UriKind.Relative));
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }

        }

        private void EditAccount_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                NavigationService.Navigate(new System.Uri("/View/editAdminAC.xaml", UriKind.Relative));
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void AddTrain_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                NavigationService.Navigate(new System.Uri("/View/addNewTrain.xaml", UriKind.Relative));
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void InmportTrain_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                NavigationService.Navigate(new System.Uri("/View/ImportDataP.xaml", UriKind.Relative));
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            
        }

        private void PackIcon_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                NavigationService.Navigate(new System.Uri("/View/notifications.xaml", UriKind.Relative));
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
    }
}
