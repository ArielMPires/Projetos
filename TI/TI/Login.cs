using MySqlConnector;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace TI
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();

            try
            {
                double atual = 2.0;
                label4.Text = Convert.ToString(atual);

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
        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void Login_Load(object sender, EventArgs e)
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

                Principal form = new Principal(id);
                this.Hide();
                form.ShowDialog();
                this.Close();

            }
            else if (!res)
            {
                MessageBox.Show("Senha ou Usuario Estão errados!");
            }
        }

        private void pass1_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void usu1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
