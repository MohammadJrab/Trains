using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Drawing;
using PrintDialog = System.Windows.Controls.PrintDialog;
using MessageBox = System.Windows.Forms.MessageBox;
using System.Windows.Markup;
using SaveFileDialog = Microsoft.Win32.SaveFileDialog;
using System.Data;
using Microsoft.Office.Interop.Excel;
using Page = System.Windows.Controls.Page;
using System.Data.SqlClient;
using DataTable = System.Data.DataTable;
using DataGrid = System.Windows.Controls.DataGrid;
using ClosedXML.Excel;
using iTextSharp.text;
using DocumentFormat.OpenXml.Office.Word;
using Trains.DAL;

namespace Trains.View
{
    /// <summary>
    /// Interaction logic for table.xaml
    /// </summary>
    public partial class table : Page
    {
        public table()
        {
            InitializeComponent();
            //refreshTable();
            //refreshT();
        }

        private void backBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                NavigationService.Navigate(new System.Uri("/View/selectGraphes.xaml", UriKind.Relative));
            }
            catch (Exception ex) { System.Windows.MessageBox.Show(ex.Message); }
        }
      public  DataTable dt = new DataTable();

        private void refreshT()
        {
            //try
            //{
                dt.Rows.Clear();
                dt.Clear();
                dt.Columns.Clear();
                using (var connection = new MySqlConnection(connectionState.ConnectionString))
                {
                    connection.Open();
                    var command = new MySqlCommand("SELECT sn,CONCAT(CAST(L as VARCHAR(50)),'%') as L,br1,br2,CONCAT(CAST(Speed as VARCHAR(50)),' Km/h') as speed,F,time,(F/seconds)*3.6 as oil  FROM stable where sn='" + TrainsList.sn + "' and time BETWEEN '" + startDate.Text + "' and '" + endDate.Text + "' ORDER BY time ASC", connection);
                    //var reader = command.ExecuteReader();
                    var reader = command.ExecuteReader();


                    MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                    dt.Columns.Add("time");
                    dt.Columns.Add("F");
                    dt.Columns.Add("speed");
                    dt.Columns.Add("br2");
                    dt.Columns.Add("br1");
                    dt.Columns.Add("L");
                    dt.Columns.Add("sn");
                    while (reader.Read())
                    {
                        DataRow newRow = dt.NewRow();
                        newRow["time"] = reader.GetString("time");
                    try
                    {
                        newRow["F"] = Math.Round(reader.GetDouble("oil"), 1).ToString() + " L/H";

                    }
                    catch (Exception ex)
                    {
                        newRow["F"] = "0";
                    }

                    newRow["speed"] = reader.GetString("speed");
                        newRow["br2"] = reader.GetString("br2");
                        newRow["br1"] = reader.GetString("br1");
                        newRow["L"] = reader.GetString("L");
                        newRow["sn"] = reader.GetString("sn");
                        dt.Rows.Add(newRow);


                    }
                    //adapter.Fill(dt);
                    dataGrid.ItemsSource = dt.DefaultView;
                    

                }

            //}
            //catch (Exception ex) { System.Windows.MessageBox.Show($"{ex.Message}", "تحذير", MessageBoxButton.OK, MessageBoxImage.Error); }

        }
      

        private void PrintBtn_Click(object sender, RoutedEventArgs e)
        {

            //ExportToExcel(dataGrid);
            ExportToPDF(dataGrid);


        }
        public void ExportToExcel(DataGrid dataGrid)
        {
            try
            {
                Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();
                Workbook wb = excel.Workbooks.Add();
                Worksheet ws = wb.ActiveSheet;

                for (int i = 0; i < dataGrid.Items.Count; i++)
                {
                    for (int j = 0; j < dataGrid.Columns.Count; j++)
                    {
                        ws.Cells[i + 1, j + 1].Value = (dataGrid.Items[i] as DataRowView).Row.ItemArray[j].ToString();
                    }
                }

                excel.Columns.AutoFit();
                wb.SaveAs("test.xlsx");
                excel.Quit();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

            private void ExportToPDF(DataGrid dataGrid)
        {
            try
            {

                var dataTable = new DataTable();

                foreach (var column in dataGrid.Columns)
                {
                    dataTable.Columns.Add(column.Header.ToString());
                }

                foreach (var item in dataGrid.Items)
                {
                    var row = dataTable.NewRow();
                    var index = 0;
                    foreach (var column in dataGrid.Columns)
                    {
                        var cell = column.GetCellContent(item);
                        if (cell != null)
                        {
                            var textBlock = cell as TextBlock;
                            if (textBlock != null)
                            {
                                row[index] = textBlock.Text;
                            }
                        }
                        index++;
                    }
                    dataTable.Rows.Add(row);
                }


                var workbook = new XLWorkbook();
                var worksheet = workbook.Worksheets.Add($"جدول القاطرة {TrainsList.sn}n  ");

                for (int i = 0; i < dataTable.Columns.Count; i++)
                {
                    worksheet.Cell(1, i + 1).Value = dataTable.Columns[i].ColumnName;
                }

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    var row = dt.Rows[i];
                    for (int j = 0; j < dataTable.Columns.Count; j++)
                    {
                        worksheet.Cell(i + 2, j + 1).Value = row[j];
                    }
                }

                var saveFileDialog = new SaveFileDialog
                {
                    Filter = "Excel Files (*.xlsx)|*.xlsx",
                    DefaultExt = ".xlsx"
                };

                if (saveFileDialog.ShowDialog() == true)
                {
                    workbook.SaveAs(saveFileDialog.FileName);
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }

        }



        private void enterBtn_Click(object sender, RoutedEventArgs e)
        {
            refreshT();
        }
    } }