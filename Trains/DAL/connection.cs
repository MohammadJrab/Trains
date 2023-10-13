using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Trains.DAL
{
    internal class connectionState
    {
        // Localhost
        public static String ConnectionString = "Server=localhost;Port=3306;Database=train;Uid=root;Pwd=;Connection Timeout=2";
        //

        // Remotly

        //public static String ConnectionString = "Server=10.23.182.65;Port=3306;Database=train;Uid=root;Pwd=;Connection Timeout=1";
        //
        private void initialize()
        {

        }
        MySqlConnection conn = new MySqlConnection
           (ConnectionString);
        //Server=10.23.182.65;Port=3306;Database=train;Uid=root;Pwd=;

        public bool OpenConnection()
        {
            try
            {
                conn.Open();
                return true;
            }
            catch (MySqlException ex)
            {
                switch (ex.Number)
                {
                    case 0:
                        MessageBox.Show("...لا يمكن الإتصال بالسيرفر");
                        break;
                    case 1045:
                        MessageBox.Show(".. خطأ في اسم المستخدم او كلمة السر");
                        break;

                }
            }
            return false;

        }
        public void close_conn()
        {

            this.conn.Close();
        }

        public MySqlConnection get_Connection()
        {

            return this.conn;
        }
    }
}
