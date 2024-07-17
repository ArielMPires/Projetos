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
using WCS;

namespace TI
{
    public partial class Principal : Form
    {
        public Principal(string id)
        {
            InitializeComponent();

            try
            {



                label4.Text = id;

                string query = "SELECT Nome FROM login where ID ='" + id + "';";

                MySqlConnection dbcon = Conexao.abrir();

                MySqlCommand commdb = new MySqlCommand(query, dbcon);

                MySqlDataReader reader;
                reader = commdb.ExecuteReader();
                reader.Read();

                MessageBox.Show("Bem Vindo " + reader.GetString(0) + "!");

                label3.Text = reader.GetString(0);
                Conexao.fechar();

                string query2 = "SELECT Permissao FROM login where ID ='" + id + "';";

                MySqlConnection dbcon2 = Conexao.abrir();

                MySqlCommand commdb2 = new MySqlCommand(query2, dbcon2);

                MySqlDataReader reader2;
                reader2 = commdb2.ExecuteReader();
                reader2.Read();
                string per = Convert.ToString(reader2.GetInt32(0));
                label5.Text = per;
                Conexao.fechar();

                Info info = new Info();
                info.id = id;
                info.nome = label3.Text;
                info.per = per;


            }
            finally
            {
                Conexao.fechar();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void estoque_Click(object sender, EventArgs e)
        {
            try
            {
                 string id = label4.Text;
                 string nome = label3.Text;
                string per = label5.Text;


                if (Convert.ToInt32(per) >= 1)
                {
                    
                    Estoque form1 = new Estoque(id,nome, per);
                    form1.ShowDialog();
                }
                else
                {
                    MessageBox.Show("Você Não tem autorização para essa Função do Sistema!");
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally {
                Conexao.fechar();
            }
        }

        private void financeiro_Click(object sender, EventArgs e)
        {
            try
            {
                string id = label4.Text;
                string nome = label3.Text;
                string per = label5.Text;


                if (Convert.ToInt32(per) >= 3)
                {
                    Financeiro form1 = new Financeiro(id, nome);
                    form1.ShowDialog();
                }
            }
            finally {
                Conexao.fechar();
            }
        }

        private void ti_Click(object sender, EventArgs e)
        {
            try
            {
                string id = label4.Text;
                string nome = label3.Text;
                string per = label5.Text;


                if (Convert.ToInt32(per) >= 5)
                {
                    STI form1 = new STI(id, nome, per);
                    form1.ShowDialog();
                }
            }
            finally {
                Conexao.fechar();
            }
        }

        private void conf_Click(object sender, EventArgs e)
        {
            try
            {
                string id = label4.Text;
                string nome = label3.Text;
                string per = label5.Text;


                if (Convert.ToInt32(per) >= 1)
                {
                    Conf form1 = new Conf(id, nome, per);
                    form1.ShowDialog();
                }
            }
            finally {
                Conexao.fechar();
            }
        }
    }
}
