using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySqlConnector;


namespace BKU
{
    class Conexao
    {
        private static string con = "datasource=192.168.1.68;port=3306;username=Usi;password=;database=wsanta;";

        private static MySqlConnection dbcon = null;

        public static MySqlConnection abrir()
        {

            dbcon = new MySqlConnection(con);

            try
            {
                dbcon.Open();

            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }
            return dbcon;
        }
        public static void fechar()
        {
            if (dbcon != null)
            {
                dbcon.Close();
            }
        }
    }
}