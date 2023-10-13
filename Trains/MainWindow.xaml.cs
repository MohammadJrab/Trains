using DocumentFormat.OpenXml.Bibliography;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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
using Trains.View;

namespace Trains
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //private Timer timerApp;

        public MainWindow()
        {
            InitializeComponent();
            //timerApp = new Timer(3000); // 1000 milliseconds = 1 second
            //timerApp.Elapsed += new ElapsedEventHandler(OnTimerElapsed);
            //timerApp.AutoReset = true;
            //timerApp.Enabled = true;
        }

        private void OnTimerElapsed(object sender, ElapsedEventArgs e)
        {
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += Worker_DoWork;
            worker.RunWorkerAsync();
        }



        public List<notificationsClass> GetNotifications()
        {
            List<notificationsClass> noti = new List<notificationsClass>();

            try
            {
                using (var connection = new MySqlConnection(connectionState.ConnectionString))
                {
                    connection.Open();
                    var command = new MySqlCommand("SELECT sn,problem,nextnotifications FROM notifications", connection);
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {

                        noti.Add(new notificationsClass
                        {
                            sn = reader.GetString("sn")
                            ,problem=reader.GetString("problem")
                            ,nextnotifications=reader.GetString("nextnotifications")

                        }) ;

                    }
                    return noti;
                }
            }
            catch (MySqlException ex)
            {
                return noti;
            }


        }



        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {

            List<notificationsClass> noti = GetNotifications();

            if (noti.Count > 0)
            {

                for (int i = 0; i <=noti.Count; i++)
                {
                    string sn = noti[i].sn;
                    string problem = noti[i].problem;
                    string nextnotifications= noti[i].nextnotifications;
                    string type;
                    if (problem == "m2" || problem == "m3" || problem == "m4" || problem == "m5" || problem == "m6")
                    {
                        type = "Km";
                    }
                    else
                    {type = "H";
                    }
                    string alarm = " "+ type + " للقاطرة " + sn + "  وأصبح التنبيه القادم في  " + nextnotifications + "  " + problem + " تنبيه : تم تجاوز القيمة العظمى ل";
                    this.Dispatcher.Invoke(() =>
                    {
                        MessageAlarm.Content = alarm;
                        turnAlarm.IsActive = true;
                    });


                }




            }


        }
        //public async Task DoTaskAsync()
        //{
        //    try
        //    {
        //        await Task.Run(async () =>
        //        {


        //            this.Dispatcher.Invoke(() =>
        //            {
        //                //MessageTextStatusSeccess.Content = "جاري محاولة تشغيل مخرج " + exit + " ";
        //                //SnackbarStatusSeccess.IsActive = true;
        //            });


        //        });


        //    }



        //    catch (Exception ex)
        //    {


        //    }

        //}



        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            try
            {
                base.OnMouseLeftButtonDown(e);
                DragMove();
            }
            catch (Exception) { }
        }

        private void Exit_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void Min_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
        protected override void OnPreviewMouseUp(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseUp(e);
            if (Mouse.Captured is Calendar || Mouse.Captured is System.Windows.Controls.Primitives.CalendarItem)
            {
                Mouse.Capture(null);
            }
        }

        private void MessageAlarm_ActionClick(object sender, RoutedEventArgs e)
        {
            turnAlarm.IsActive = false;

        }
    }
}
public class notificationsClass {


    public string sn { get; set; }
    public string problem { get; set; }
    public string nextnotifications { get; set; }


}
