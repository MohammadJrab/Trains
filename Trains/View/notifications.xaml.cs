using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
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
    /// Interaction logic for notifications.xaml
    /// </summary>
    public partial class notifications : Page
    {
        public notifications()
        {
            InitializeComponent();
        }
        public DataTable dt = new DataTable();

        private void refreshT()
        {
            try
            {
                dt.Rows.Clear();
                dt.Clear();
                dt.Columns.Clear();
                using (var connection = new MySqlConnection(connectionState.ConnectionString))
                {
                    connection.Open();
                    var command = new MySqlCommand("SELECT sn,problem,nextnotifications  FROM notifications  ORDER BY Datetime ASC", connection);
                    //var reader = command.ExecuteReader();
                    var reader = command.ExecuteReader();


                    MySqlDataAdapter adapter = new MySqlDataAdapter(command);
          
                    dt.Columns.Add("nextnotifications");
                    dt.Columns.Add("problem");
                    dt.Columns.Add("sn");
                    while (reader.Read())
                    {
                        DataRow newRow = dt.NewRow();
                        string problem = reader.GetString("problem");
                        string type;
                        if (problem == "m2" || problem == "m3" || problem == "m4" || problem == "m5" || problem == "m6") { type = " km"; }
                        else { type = " H"; }
                        newRow["nextnotifications"] = reader.GetString("nextnotifications") + type;
                        newRow["problem"] = reader.GetString("problem");
                        newRow["sn"] = reader.GetString("sn");
                        dt.Rows.Add(newRow);


                    }
                    //adapter.Fill(dt);
                    dataGrid.ItemsSource = dt.DefaultView;


                }

            }
            catch (Exception ex) { System.Windows.MessageBox.Show($"{ex.Message}", "تحذير", MessageBoxButton.OK, MessageBoxImage.Error); }

        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            refreshT();
        }

        private void backBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                NavigationService.Navigate(new System.Uri("/View/HomeAdmin.xaml", UriKind.Relative));
            }
            catch (Exception ex) { System.Windows.MessageBox.Show(ex.Message); }
        }
    }
}
