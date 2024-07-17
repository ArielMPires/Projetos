using MySqlConnector;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BK
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string id = textBox1.Text;
            string pass = textBox2.Text;
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

                    Form2 form = new Form2(id);
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
