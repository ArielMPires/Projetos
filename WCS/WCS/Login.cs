using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySqlConnector;

namespace WCS
{
    public partial class Login : Form
    {

        public Login()
        {
            InitializeComponent();

            try
            {
                double atual = 2.1;

                string query = "SELECT versao FROM att;";

                MySqlConnection dbcon = Conexao.abrir();

                MySqlCommand commdb = new MySqlCommand(query, dbcon);

                MySqlDataReader reader;
                reader = commdb.ExecuteReader();
                reader.Read();

                double att = reader.GetDouble(0);

                if (atual == att)
                {

                }
                else if (atual < att)
                {
                    System.Diagnostics.Process.Start(@"VersãoAtt.exe");
                    this.Close();
                    Environment.Exit(0);
                }
            }
            finally
            {
                Conexao.fechar();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Login_Load_1(object sender, EventArgs e)
        {
            this.AcceptButton = enter;
        }

        private void enter_Click(object sender, EventArgs e)
        {
            string id = usu1.Text;
            string pass = pass1.Text;
            bool res = false;
            string query = "SELECT * FROM login where ID ='" + id + "'and senha = '" + pass + "';";

            MySqlConnection dbcon = Conexao.abrir();

            MySqlCommand commdb = new MySqlCommand(query, dbcon);

            MySqlDataReader reader;
            reader = commdb.ExecuteReader();

            res = reader.HasRows;

            Conexao.fechar();

            bool logado = false;


            if (res)
            {
                string query1 = "SELECT Nome,permissao,Estoque,Financeiro,TI,Conf FROM login where ID ='" + id + "';";

                MySqlConnection dbcon1 = Conexao.abrir();

                MySqlCommand commdb1 = new MySqlCommand(query1, dbcon1);

                MySqlDataReader reader1;
                reader1 = commdb1.ExecuteReader();
                reader1.Read();


                string n = reader1.GetString(0);
                string per = Convert.ToString(reader1.GetInt32(1));
                string pere = Convert.ToString(reader1.GetInt32(2));
                string perf = Convert.ToString(reader1.GetInt32(3));
                string pert = Convert.ToString(reader1.GetInt32(4));
                string perc = Convert.ToString(reader1.GetInt32(5));
                Conexao.fechar();


                Info.Id = id;
                Info.Nome = n;
                Info.Per = per;
                Info.Pere = pere;
                Info.Perf = perf;
                Info.Pert = pert;
                Info.Perc = perc;
                
                
                Principal form = new Principal();
                this.Hide();
                form.ShowDialog();
                this.Close();

            }
            else if (!res)
            {
                MessageBox.Show("Senha ou Usuario Estão errados!");
            }
        }


    }
}
