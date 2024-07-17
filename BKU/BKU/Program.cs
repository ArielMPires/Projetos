using MySqlConnector;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;


namespace BKU
{
    class Program
    {
        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        const int SW_HIDE = 0;
        const int SW_SHOW = 5;

        static void Main(string[] args)
        {
            var handle = GetConsoleWindow();

            // Hide
            ShowWindow(handle, SW_HIDE);
            string id = "5";
            loop_BK(id);
        }

        private static void loop_BK(string id)
        {
            int i = 1;
            while (i < 2)
            {
                try
                {
                    string query2 = "SELECT dh,ordem FROM BK WHERE situacao = 'Agendado' AND ID = "+id+";";
                    MySqlConnection dbcon2 = Conexao.abrir();

                    MySqlCommand commdb2 = new MySqlCommand(query2, dbcon2);

                    MySqlDataReader reader2;
                    reader2 = commdb2.ExecuteReader();
                    reader2.Read();
                    DateTime date = reader2.GetDateTime(0);
                    int od = reader2.GetInt32(1);



                    int result = DateTime.Compare(DateTime.Now, date);

                    if (result == 0 || result == 1)
                    {
                        CFTP ftp = new CFTP("BKV", "@BkvS#5327", "192.168.1.68", "Rafael");
                        ftp.UploadDirectory();

                        CFTP FT = new CFTP("BKV", "@BkvS#5327", "192.168.1.68", @"C:\Users\Rafael\Documents\Arquivos do Outlook");
                        FT.UploadDirectory();

                        string query = "UPDATE BK SET situacao = 'Backup Feito' WHERE Ordem = '" + od + "' ";
                        MySqlConnection dbcon = Conexao.abrir();

                        MySqlCommand commdb = new MySqlCommand(query, dbcon);

                        commdb.ExecuteNonQuery();

                        Conexao.fechar();
                    }
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    continue;
                }
                finally
                {
                    Conexao.fechar();
                }
            }
        }
    }
}
