using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySqlConnector;


namespace WCS
{
    class Conexao
    {
        private static string con = "datasource=192.168.1.68;port=3306;username=System;password=@Sys#1256A;database=wsanta;Convert Zero Datetime=True";

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
                MessageBox.Show(ex.Message);
                con = "datasource=189.1.165.135;port=3306;username=Usi;password=;database=wsanta;Convert Zero Datetime=True";
                dbcon.Open();
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