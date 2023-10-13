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
using System.IO;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using Microsoft.Win32;
using DocumentFormat.OpenXml.Office2013.Word;
using Newtonsoft.Json.Linq;
using System.Windows.Forms;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;
using SaveFileDialog = System.Windows.Forms.SaveFileDialog;
using Trains.DAL;
using System.ComponentModel;
using DocumentFormat.OpenXml.Wordprocessing;
using MyToolkit.Messaging;
using Toolkit.Scripting.DataScript.Operators;
using MessageBox = System.Windows.MessageBox;
using OxyPlot.Series;
using System.Data;
using System.Runtime.InteropServices.ComTypes;
using System.Web.UI.WebControls;
using Toolkit.Scripting.xScript.Model;
using DocumentFormat.OpenXml.Drawing;
using Application = System.Windows.Application;
using static ClosedXML.Excel.XLPredefinedFormat;
using DateTime = System.DateTime;

namespace Trains.View
{
    /// <summary>
    /// Interaction logic for ImportDataP.xaml
    /// </summary>
    public partial class ImportDataP : Page
    {
        public ImportDataP()
        {
            InitializeComponent();
        }
        private void backBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                NavigationService.Navigate(new System.Uri("/View/HomeAdmin.xaml", UriKind.Relative));
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        public class MyObject
        {

            public string m1 { get; set; }
      

            public string L { get; set; }

            public string F { get; set; }

            public string br1 { get; set; }
            public string br2 { get; set; }
            public string seconds { get; set; }

            public string time { get; set; }



        }

        private async void AddDataBtn_Click(object sender, RoutedEventArgs e)

        {
            await DoTaskAsync(TrainsList.SelectedItem.ToString());

          

        }
        int countEXE = 3;

        //private void Worker_DoWork(object sender, DoWorkEventArgs e)
        //{
        //}
        public async Task DoTaskAsync(string sn)
        {

            await Task.Run(async () =>
            {


            //try
            //{
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*";
                if (openFileDialog.ShowDialog() == true)
                {
                    this.Dispatcher.Invoke(() =>
                            {
                                AddDataBtn.IsEnabled = false;
                                progressBar.Visibility = Visibility.Visible;
                            });
                    //try
                    //{

                    //BackgroundWorker worker = sender as BackgroundWorker;
                    string fileName = openFileDialog.FileName as string;

                    string content = File.ReadAllText(fileName);
                    if (content.EndsWith(","))
                    {
                        // Remove the comma
                        content = content.TrimEnd(',');
                        File.WriteAllText(fileName, content);
                    }

                    // Check if the content already has "[" at the beginning and "]" at the end
                    if (content.StartsWith("[") && content.EndsWith("]"))
                    {
                        //Console.WriteLine("File already has brackets. No changes made.");
                    }
                    else
                    {
                        // Add brackets to the content
                        string updatedContent = "[" + content + "]";

                        // Write the updated content back to the file
                        File.WriteAllText(fileName, updatedContent);

                        //Console.WriteLine("Brackets added to the file content.");
                    }

                    string jsonData = File.ReadAllText(fileName);
                    List<MyObject> data = JsonConvert.DeserializeObject<List<MyObject>>(jsonData);

                    //List<MyObject> objects = JsonConvert.DeserializeObject<List<MyObject>>(jsonData);
                    //int countp = 0;
                    //MessageBox.Show(objects[0].m1.ToString());

                    using (MySqlConnection connection = new MySqlConnection(connectionState.ConnectionString))
                    {
                        connection.Open();
                        MySqlCommand insertCmd = connection.CreateCommand();
                        //if (data != null)
                        //{

                        string dt1="";
                        bool hasDone = false ;
                        foreach (MyObject dataT in data)
                        {
                              
                            double OldM1 = 0;
                            double OldM2 = 0;
                            double OldM3 = 0;
                            double OldM4 = 0;
                            double OldM5 = 0;
                            double OldM6 = 0;
                            int oldT1 = 0;
                            int oldT2 = 0;
                            int oldT3 = 0;
                            int oldT4 = 0;
                            int oldT5 = 0;
                            int oldT6 = 0;
                            double Rej=0;
                            int seconds=0;
                            double FlowS = 0;
                            FlowS = Convert.ToDouble(dataT.F);
                            //MessageBox.Show(TrainsList.SelectedItem.ToString());
                            MySqlCommand cmd5 = new MySqlCommand("SELECT COUNT(*) FROM screens WHERE sn = @sn ", connection);

                            cmd5.Parameters.AddWithValue("@sn", sn);
                            int count2 = int.Parse(cmd5.ExecuteScalar().ToString());

                            MySqlCommand cmd = new MySqlCommand("SELECT COUNT(*) FROM stable WHERE sn = @sn and time = @time ", connection);
                            cmd.Parameters.AddWithValue("@sn", sn);
                            cmd.Parameters.AddWithValue("@time", dataT.time);


                            if (count2 != 0)
                            {
                                int count = int.Parse(cmd.ExecuteScalar().ToString());
                                if (dt1.ToString() !="")
                                {
                                    TimeSpan timeSpan = Convert.ToDateTime(dataT.time) - Convert.ToDateTime(dt1);

                                     seconds = (int)timeSpan.TotalSeconds;
                                }

                             
                                if (count == 0)
                                {

                                    insertCmd.CommandText = "INSERT INTO stable (sn, m1,m2,m3,m4,m5,m6,T1,T2,T3,T4,T5,T6,F,flowSec,L,br1,br2,speed,time,seconds) VALUES (@sn,@m1,@m2,@m3,@m4,@m5,@m6,@T1,@T2,@T3,@T4,@T5,@T6,@F,@flowSec,@L,@br1,@br2,@speed,@time,@seconds)";
                                    List<string> list = new List<string>();
                                    list = refreshT(sn);
                                    if (list.Count > 0)
                                    {
                                        OldM1 = double.Parse(list[0]);
                                        OldM2 = double.Parse(list[1]);
                                        OldM3 = double.Parse(list[2]);
                                        OldM4 = double.Parse(list[3]);
                                        OldM5 = double.Parse(list[4]);
                                        OldM6 = double.Parse(list[5]);
                                        oldT1 = int.Parse(list[6]);
                                        oldT2 = int.Parse(list[7]);
                                        oldT3 = int.Parse(list[8]);
                                        oldT4 = int.Parse(list[9]);
                                        oldT5 = int.Parse(list[9]);
                                        oldT6 = int.Parse(list[10]);
                                    }

                                    double NewDis = double.Parse(dataT.m1) / 1000;  //Dis From esp
                                    int speed = (int)(double.Parse(dataT.m1) / int.Parse(dataT.seconds) * 3.6);
                                    double m1 = Math.Round(OldM1 + NewDis, 2);
                                    double m2 = Math.Round(OldM2 + NewDis, 2);
                                    double m3 = Math.Round(OldM3 + NewDis, 2);
                                    double m4 = Math.Round(OldM4 + NewDis, 2);
                                    double m5 = Math.Round(OldM5 + NewDis, 2);
                                    double m6 = Math.Round(OldM6 + NewDis, 2);
                                    int NewT1 = oldT1 + int.Parse(dataT.seconds);
                                    int NewT2 = oldT2 + int.Parse(dataT.seconds);
                                    int NewT3 = oldT3 + int.Parse(dataT.seconds);
                                    int NewT4 = oldT4 + int.Parse(dataT.seconds);
                                    int NewT5 = oldT5 + int.Parse(dataT.seconds);
                                    int NewT6 = oldT6 + int.Parse(dataT.seconds);
                                    double mainFlow = GetMainFlow(sn);
                                    double F = Math.Round((mainFlow * seconds) - double.Parse(dataT.F), 1);
                                    if (F < 0) { Rej = 0; }
                                    else { Rej = F; }
                                    double FlowSec = Math.Round(double.Parse(dataT.F), 1);
                                    if (FlowS / seconds < 350)
                                    {
                                        Rej = 0;

                                    }
                                    else { Rej = F; }
                                    if (seconds == 0 && FlowS == 0) { Rej = 0; }
                                    insertCmd.Parameters.Clear();
                                    insertCmd.Parameters.AddWithValue("@sn", sn);
                                    insertCmd.Parameters.AddWithValue("@m1", m1);
                                    insertCmd.Parameters.AddWithValue("@m2", m2);
                                    insertCmd.Parameters.AddWithValue("@m3", m3);
                                    insertCmd.Parameters.AddWithValue("@m4", m4);
                                    insertCmd.Parameters.AddWithValue("@m5", m5);
                                    insertCmd.Parameters.AddWithValue("@m6", m6);
                                    insertCmd.Parameters.AddWithValue("@T1", NewT1);
                                    insertCmd.Parameters.AddWithValue("@T2", NewT2);
                                    insertCmd.Parameters.AddWithValue("@T3", NewT3);
                                    insertCmd.Parameters.AddWithValue("@T4", NewT4);
                                    insertCmd.Parameters.AddWithValue("@T5", NewT5);
                                    insertCmd.Parameters.AddWithValue("@T6", NewT6);
                                    insertCmd.Parameters.AddWithValue("@F", Rej);
                                    insertCmd.Parameters.AddWithValue("@flowSec", FlowS);
                                    insertCmd.Parameters.AddWithValue("@L", dataT.L);
                                    insertCmd.Parameters.AddWithValue("@br1", dataT.br1);
                                    insertCmd.Parameters.AddWithValue("@br2", dataT.br2);
                                    insertCmd.Parameters.AddWithValue("@speed", speed);
                                    insertCmd.Parameters.AddWithValue("@time", dataT.time);
                                    insertCmd.Parameters.AddWithValue("@seconds", seconds);
                                    insertCmd.ExecuteNonQuery();
                                    countEXE = count;

                                }

                                //else if (count == 1)
                                //{

                                //    List<string> list = new List<string>();
                                //    list = refreshT(TrainsList.SelectedItem.ToString());
                                //    if (list != null)
                                //    {
                                //        OldM1 = double.Parse(list[0]);
                                //        OldM2 = double.Parse(list[1]);
                                //        OldM3 = double.Parse(list[2]);
                                //        OldM4 = double.Parse(list[3]);
                                //        OldM5 = double.Parse(list[4]);
                                //        OldM6 = double.Parse(list[5]);
                                //        oldT1 = int.Parse(list[6]);
                                //        oldT2 = int.Parse(list[7]);
                                //        oldT3 = int.Parse(list[8]);
                                //        oldT4 = int.Parse(list[9]);
                                //        oldT5 = int.Parse(list[9]);
                                //        oldT6 = int.Parse(list[10]);
                                //    }

                                //    double NewDis = double.Parse(dataT.m1);//Dis From esp
                                //    int speed = (int)(double.Parse(dataT.m1) / int.Parse(dataT.seconds) * 3.6);
                                //    double m1 = OldM1 + NewDis;
                                //    double m2 = OldM2 + NewDis;
                                //    double m3 = OldM3 + NewDis;
                                //    double m4 = OldM4 + NewDis;
                                //    double m5 = OldM5 + NewDis;
                                //    double m6 = OldM6 + NewDis;
                                //    int NewT1 = oldT1 + int.Parse(dataT.seconds);
                                //    int NewT2 = oldT2 + int.Parse(dataT.seconds);
                                //    int NewT3 = oldT3 + int.Parse(dataT.seconds);
                                //    int NewT4 = oldT4 + int.Parse(dataT.seconds);
                                //    int NewT5 = oldT5 + int.Parse(dataT.seconds);
                                //    int NewT6 = oldT6 + int.Parse(dataT.seconds);
                                //    double mainFlow = GetMainFlow(TrainsList.SelectedItem.ToString());
                                //    double F = (mainFlow * int.Parse(dataT.seconds)) - double.Parse(dataT.F);


                                //    MySqlCommand updateCmd = new MySqlCommand("UPDATE `stable` SET `sn`=@sn,`m1`=@m1,`m2`=@m2,`m3`=@m3,`m4`=@m4,`m5`=@m5,`m6`=@m6,`T1`=@T1,`T2`=@T2,`T3`=@T3,`T4`=@T4,`T5`=@T5,`T6`=@T6,`F`=@F,`flowSec`=@flowSec,`L`=@L,`br1`=@br1,`br2`=@br2,`speed`=@speed,`time`=@time, `seconds`=@seconds WHERE `sn`='" + TrainsList.SelectedItem.ToString() + "' ", connection);
                                //    updateCmd.Parameters.Clear();
                                //    updateCmd.Parameters.AddWithValue("@sn", TrainsList.SelectedItem.ToString());
                                //    updateCmd.Parameters.AddWithValue("@m1", m1);
                                //    updateCmd.Parameters.AddWithValue("@m2", m2);
                                //    updateCmd.Parameters.AddWithValue("@m3", m3);
                                //    updateCmd.Parameters.AddWithValue("@m4", m4);
                                //    updateCmd.Parameters.AddWithValue("@m5", m5);
                                //    updateCmd.Parameters.AddWithValue("@m6", m6);
                                //    updateCmd.Parameters.AddWithValue("@T1", NewT1);
                                //    updateCmd.Parameters.AddWithValue("@T2", NewT2);
                                //    updateCmd.Parameters.AddWithValue("@T3", NewT3);
                                //    updateCmd.Parameters.AddWithValue("@T4", NewT4);
                                //    updateCmd.Parameters.AddWithValue("@T5", NewT5);
                                //    updateCmd.Parameters.AddWithValue("@T6", NewT6);

                                //    updateCmd.Parameters.AddWithValue("@F", F);
                                //    updateCmd.Parameters.AddWithValue("@flowSec", dataT.F);
                                //    updateCmd.Parameters.AddWithValue("@L", dataT.L);
                                //    updateCmd.Parameters.AddWithValue("@br1", dataT.br1);
                                //    updateCmd.Parameters.AddWithValue("@br2", dataT.br2);
                                //    updateCmd.Parameters.AddWithValue("@speed", speed);
                                //    updateCmd.Parameters.AddWithValue("@time", dataT.time);
                                //    updateCmd.Parameters.AddWithValue("@seconds", dataT.seconds);
                                //    updateCmd.ExecuteNonQuery();
                                //    countEXE = count;

                                //}
                                else
                                {
                                    countEXE = count;

                                }
                                dt1 = dataT.time;
                                
                            }

                            else { countEXE = 5; }





                            //int progress = (int)((double)data.IndexOf(dataT) / data.Count * 100);
                            //worker.ReportProgress(progress);
                        }
                            //}
                            //else
                            //{
                            //MessageBox.Show("No Data");
                            //    countEXE = 6;

                            //}
                        
                    }
            
                        GetNotifications(sn);
                    this.Dispatcher.Invoke(() =>
                    {
                        AddDataBtn.IsEnabled = true;
                    progressBar.Visibility = Visibility.Collapsed;
                    if (countEXE == 0)
                    {
                        lblStatus.Text = "تم إضافة البيانات بنجاح";
                            string fileNamed = openFileDialog.FileName as string;

                         
                                // Remove the comma
                                content = "";
                                File.WriteAllText(fileName, content);
                            


                        }
                        else if (countEXE == 1)
                    {
                        lblStatus.Text = "تم تحديث البيانات بنجاح";
                            string fileNamed = openFileDialog.FileName as string;


                            // Remove the comma
                            content = "";
                            File.WriteAllText(fileName, content);
                        }
                    else if (countEXE == 5)
                    {
                        lblStatus.Text = "خطأ لم يتم العثور على القاطرة";
                    }
                    else
                    {

                        lblStatus.Text = "خطأ لم يتم إدخال البيانات تأكد من الاتصال و اعد المحاولة";


                    }
                    });

                    //}
                    //catch (Exception ex)
                    //{
                    //    MessageBox.Show(ex.Message);
                    //}
                    //BackgroundWorker worker = new BackgroundWorker();
                    //worker.WorkerReportsProgress = true;
                    //worker.DoWork += Worker_DoWork;
                    //worker.ProgressChanged += Worker_ProgressChanged;
                    //worker.RunWorkerCompleted += Worker_RunWorkerCompleted;
                    //worker.RunWorkerAsync(openFileDialog.FileName);
                    //AddDataBtn.IsEnabled = true;
                }
                //}
                //catch (Exception ex)
                //{
                //    this.Dispatcher.Invoke(() =>
                //          {
                //              lblStatus.Text = "خطأ لم يتم إدخال البيانات تأكد من الاتصال و اعد المحاولة";
                //          });

                //}







            });


            



          

        }


        //public List<string> Getdata(string sn) 

        //{
        //List<string> data = new List<string>();




        //    return data;

        //}


        public List<string> refreshT(string sn)
        {
            List<string> data = new List<string>();

            //try
            //{
                using (var connection = new MySqlConnection(connectionState.ConnectionString))
                {
                    connection.Open();
                    var command = new MySqlCommand("SELECT * FROM stable where sn='"+sn + "'  ORDER BY time desc LIMIT 1", connection);
                    var reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                       data.Add(reader.GetString(1));//m1
                       data.Add(reader.GetString(2));//m2
                        data.Add(reader.GetString(3));//m3
                        data.Add(reader.GetString(4));//m4
                        data.Add(reader.GetString(5));//m5
                        data.Add(reader.GetString(6));//m6
                        data.Add(reader.GetString(7));//T1
                       data.Add(reader.GetString(8));//T2
                        data.Add(reader.GetString(9));//T3
                        data.Add(reader.GetString(10));//T4
                        data.Add(reader.GetString(11));//T5
                        data.Add(reader.GetString(12));//T6
                        data.Add(reader.GetString(12));//T6


                    }
          


                }
                return data;

            //}
            //catch (Exception ex) { System.Windows.MessageBox.Show($"{ex.Message}", "تحذير", MessageBoxButton.OK, MessageBoxImage.Error); return data ; }

        }

        public double GetMainFlow(string sn)
        {

            //try
            //{
            using (var connection = new MySqlConnection(connectionState.ConnectionString))
            {
                connection.Open();
                var command = new MySqlCommand("SELECT flow FROM ftable where sn='" + sn + "' ", connection);
                var reader = command.ExecuteReader();
                double MainFlow = 0;
                while (reader.Read())
                {
                    MainFlow = reader.GetDouble(0);



                }

                return MainFlow;

            }

            //}
            //catch (Exception ex) { System.Windows.MessageBox.Show($"{ex.Message}", "تحذير", MessageBoxButton.OK, MessageBoxImage.Error); return 370.0; }

        }


        private void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar.Value = e.ProgressPercentage;
        }

        //private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        //{
        //    AddDataBtn.IsEnabled = true;
        //    progressBar.Visibility = Visibility.Collapsed;

        //    //if (e.Error != null)
        //    //{
        //    //    // Display any errors that occurred
        //    //    lblStatus.Content = "خطأ في إدخال البيانات";
        //    //}
        //    //else
        //    //{
        //    //    // Display the success message
        //    //    lblStatus.Content = "تم إدخال البيانات بنجاح";
        //    //}
        //    if (countEXE == 0)
        //    {
        //        lblStatus.Text = "تم إضافة البيانات بنجاح";
        //    }
        //    else if (countEXE == 1)
        //    {
        //        lblStatus.Text = "تم تحديث البيانات بنجاح";
        //    }
        //    else if (countEXE == 5)
        //    {
        //        lblStatus.Text = "خطأ لم يتم العثور على القاطرة";
        //    }
        //    else
        //    {

        //        lblStatus.Text = "خطأ لم يتم إدخال البيانات تأكد من الاتصال و اعد المحاولة";


        //    }
        //}

        public List<string> GetTrain()
        {
            List<string> trains = new List<string>();

            try
            {
                using (var connection = new MySqlConnection(connectionState.ConnectionString))
                {
                    connection.Open();
                    var command = new MySqlCommand("SELECT sn FROM screens", connection);
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {

                        trains.Add(reader.GetString("sn"));

                    }
                    return trains;
                }
            }
            catch (MySqlException ex)
            {
                return trains;
            }


        }

        public List<string> GetStable(string sn)
        {
            List<string> sList = new List<string>();
            try
            {
                using (var connection = new MySqlConnection(connectionState.ConnectionString))
                {
                    connection.Open();
                    var command = new MySqlCommand("SELECT * FROM stable where sn='" + sn + "' order by time desc limit 1 ", connection);
                    var reader = command.ExecuteReader();

                    while (reader.Read())
                    {

                        sList.Add(reader.GetString("m2"));
                        sList.Add(reader.GetString("m3"));
                        sList.Add(reader.GetString("m4"));
                        sList.Add(reader.GetString("m5"));
                        sList.Add(reader.GetString("m6"));
                        sList.Add(Convert.ToString(reader.GetInt32("T2") / 3600));
                        sList.Add(Convert.ToString(reader.GetInt32("T3") / 3600));
                        sList.Add(Convert.ToString(reader.GetInt32("T4") / 3600));
                        sList.Add(Convert.ToString(reader.GetInt32("T5") / 3600));
                        sList.Add(Convert.ToString(reader.GetInt32("T6") / 3600));


                    }
                    return sList;
                }
            }
            catch (MySqlException ex) { return sList; }

        }

        public List<string> GetFtable(string sn)
        {
            List<string> fList = new List<string>();
            try
            {
                using (var connection2 = new MySqlConnection(connectionState.ConnectionString))
                {
                    connection2.Open();
                    var command2 = new MySqlCommand("SELECT * FROM ftable where sn='" + sn + "'", connection2);
                    var reader2 = command2.ExecuteReader();
                    while (reader2.Read())
                    {

                        fList.Add(reader2.GetString("mM2"));
                        fList.Add(reader2.GetString("mM3"));
                        fList.Add(reader2.GetString("mM4"));
                        fList.Add(reader2.GetString("mM5"));
                        fList.Add(reader2.GetString("mM6"));
                        fList.Add(reader2.GetString("mT2"));
                        fList.Add(reader2.GetString("mT3"));
                        fList.Add(reader2.GetString("mT4"));
                        fList.Add(reader2.GetString("mT5"));
                        fList.Add(reader2.GetString("mT6"));


                    }
                    return fList;
                }
            }
            catch (MySqlException exx) { return fList; }





        }
        public void AddNotification(string sn, string problem, string value)
        {


            try
            {
                using (var connection = new MySqlConnection(connectionState.ConnectionString))
                {
                    connection.Open();


                    MySqlCommand cmd5 = new MySqlCommand("SELECT COUNT(*) FROM notifications WHERE sn = @sn and problem=@problem ", connection);

                    cmd5.Parameters.AddWithValue("@sn", sn);
                    cmd5.Parameters.AddWithValue("@problem", problem);
                    int count2 = int.Parse(cmd5.ExecuteScalar().ToString());




                    if (count2 == 0)
                    {



                        var command = new MySqlCommand("insert into train.notifications(sn,problem,nextnotifications,Datetime) values('" + sn + "','" + problem + "','" + value + "','" + DateTime.Now.ToString() + "');", connection);

                        command.ExecuteNonQuery();
                    }



                    else if (count2 == 1)
                    {




                        MySqlCommand updateCmd = new MySqlCommand("UPDATE `notifications` SET `nextnotifications`='" + value + "',`Datetime`='" + DateTime.Now.ToString() +"' WHERE `sn`='" + sn + "' and `problem`='"+problem+"' ", connection);

                        updateCmd.ExecuteNonQuery();

                    }



                }
            }
            catch (Exception ex) { }



        }

        public void GetNotifications(string sn)
        {

            //List<string> trains = GetTrain();

            //if (trains.Count > 0)
            //{
            //    for (int i = 0; i <= trains.Count; i++)
            //    {

                    List<string> stable = GetStable(sn);
                    List<string> ftable = GetFtable(sn);

                    if (stable.Count > 0 && ftable.Count > 0)
                    {

                        if (double.Parse(stable[0]) > double.Parse(ftable[0])) { double NewM2 = double.Parse(stable[0]) + double.Parse(ftable[0]); AddNotification(sn, "m2", NewM2.ToString()); }

                        if (double.Parse(stable[1]) > double.Parse(ftable[1])) { double NewM3 = double.Parse(stable[1]) + double.Parse(ftable[1]); AddNotification(sn, "m3", NewM3.ToString()); }


                        if (double.Parse(stable[2]) > double.Parse(ftable[2])) { double NewM4 = double.Parse(stable[2]) + double.Parse(ftable[2]); AddNotification(sn, "m4", NewM4.ToString()); }


                        if (double.Parse(stable[3]) > double.Parse(ftable[3])) { double NewM5 = double.Parse(stable[3]) + double.Parse(ftable[3]); AddNotification(sn, "m5", NewM5.ToString()); }


                        if (double.Parse(stable[4]) > double.Parse(ftable[4])) { double NewM6 = double.Parse(stable[4]) + double.Parse(ftable[4]); AddNotification(sn, "m6", NewM6.ToString()); }

                        if (double.Parse(stable[5]) > double.Parse(ftable[5])) { double NewT2 = double.Parse(stable[5]) + double.Parse(ftable[5]); AddNotification(sn, "T2", NewT2.ToString()); }

                        if (double.Parse(stable[6]) > double.Parse(ftable[6])) { double NewT3 = double.Parse(stable[6]) + double.Parse(ftable[6]); AddNotification(sn, "T3", NewT3.ToString()); }


                        if (double.Parse(stable[7]) > double.Parse(ftable[7])) { double NewT4 = double.Parse(stable[7]) + double.Parse(ftable[7]); AddNotification(sn, "T4", NewT4.ToString()); }


                        if (double.Parse(stable[8]) > double.Parse(ftable[8])) { double NewT5 = double.Parse(stable[8]) + double.Parse(ftable[8]); AddNotification(sn, "T5", NewT5.ToString()); }


                        if (double.Parse(stable[9]) > double.Parse(ftable[9])) { double NewT6 = double.Parse(stable[9]) + double.Parse(ftable[9]); AddNotification(sn, "T6", NewT6.ToString()); }



                    }


            //    }
            //}
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var connection = new MySqlConnection(connectionState.ConnectionString))
                {
                    connection.Open();
                    var command = new MySqlCommand("SELECT sn FROM screens", connection);
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                      
                        TrainsList.Items.Add(reader.GetString("sn"));

                    }
                }
            }
            catch (MySqlException ex) { MessageBox.Show("خطأ في الاتصال بقاعدة البيانات", "تحذير", MessageBoxButton.OK, MessageBoxImage.Error); }
        
    }
    }
}
public class jsonmodel
{
    public string m1 { get; set; }
    public string m2 { get; set; }
    public string m3 { get; set; }
    public string m4 { get; set; }
    public string m5 { get; set; }
    public string m6 { get; set; }

    public string T1 { get; set; }
    public string T2 { get; set; }
    public string T3 { get; set; }
    public string T4 { get; set; }
    public string T5 { get; set; }
    public string T6 { get; set; }

    public string F { get; set; }

    public string speed { get; set; }




}
