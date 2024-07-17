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
using TI;

namespace WCS
{
    public partial class Conf : Form
    {
        public Conf(string id,string nome, string per)
        {
            InitializeComponent();

            label12.Text =  id;
            label13.Text =  nome;

            string query = "SELECT conf FROM login where ID = '"+id+"';";

            MySqlConnection dbcon = Conexao.abrir();

            MySqlCommand commdb = new MySqlCommand(query, dbcon);

            MySqlDataReader reader;
            reader = commdb.ExecuteReader();
            reader.Read();

            int cf = reader.GetInt32(0);

            Conexao.fechar();


            label3.Text = Convert.ToString(cf);

            label3.Visible = false;
        }

        private void modusu_Click(object sender, EventArgs e)
        {
            modusu.BackColor = Color.White;
            addusu.BackColor = Color.Transparent;
            label14.Text = "mod";

            if(Convert.ToInt32(label3.Text) == 1)
            {
                listView1.Visible = false;
                label5.Visible = true;
                textBox1.Visible = true;
                label4.Visible = true;
                textBox2.Visible = true;
                label6.Visible = true;
                textBox3.Visible = true;
                label7.Visible = false;
                textBox4.Visible = false;
                label8.Visible = false;
                textBox5.Visible = false;
                label9.Visible = false;
                textBox6.Visible = false;
                label10.Visible = false;
                textBox7.Visible = false;
                label11.Visible = false;
                textBox8.Visible = false;
                button1.Visible = true;

                textBox1.Text = label12.Text;
                textBox2.Text = label13.Text;

                string query = "SELECT Senha FROM login where ID = '" + label12.Text + "';";

                MySqlConnection dbcon = Conexao.abrir();

                MySqlCommand commdb = new MySqlCommand(query, dbcon);

                MySqlDataReader reader;
                reader = commdb.ExecuteReader();
                reader.Read();

                string cf = reader.GetString(0);

                Conexao.fechar();
                textBox3.Text = cf;
                


            }
            else if(Convert.ToInt32(label3.Text) == 2)
            {
                listView1.Visible = true;
                label5.Visible = true;
                textBox1.Visible = true;
                label4.Visible = true;
                textBox2.Visible = true;
                label6.Visible = true;
                textBox3.Visible = true;
                label7.Visible = true;
                textBox4.Visible = true;
                label8.Visible = true;
                textBox5.Visible = true;
                label9.Visible = true;
                textBox6.Visible = true;
                label10.Visible = true;
                textBox7.Visible = true;
                label11.Visible = true;
                textBox8.Visible = true;
                button1.Visible = true;

                listView1.View = View.Details;
                listView1.AllowColumnReorder = true;
                listView1.FullRowSelect = true;
                listView1.GridLines = true;

                listView1.Columns.Clear();

                listView1.Columns.Add("ID:", 80, HorizontalAlignment.Left);
                listView1.Columns.Add("Nome", 130, HorizontalAlignment.Left);
                listView1.Columns.Add("Senha:", 80, HorizontalAlignment.Left);
                listView1.Columns.Add("Permissão Geral:", 50, HorizontalAlignment.Left);
                listView1.Columns.Add("Permissão Estoque:", 50, HorizontalAlignment.Left);
                listView1.Columns.Add("Permissão Financeiro:", 50, HorizontalAlignment.Left);
                listView1.Columns.Add("Permissão TI:", 50, HorizontalAlignment.Left);
                listView1.Columns.Add("Permissão Configurações:", 50, HorizontalAlignment.Left);

                string query = "SELECT * FROM login";

                MySqlConnection dbcon = Conexao.abrir();

                MySqlCommand com = new MySqlCommand(query, dbcon);

                MySqlDataReader reader = com.ExecuteReader();

                listView1.Items.Clear();


                while (reader.Read())
                {


                    string[] row = {
                    Convert.ToString(reader.GetInt32(0)),
                    reader.GetString(1),
                    reader.GetString(2),
                   Convert.ToString(reader.GetInt32(3)),
                   Convert.ToString(reader.GetInt32(4)),
                   Convert.ToString(reader.GetInt32(5)),
                   Convert.ToString(reader.GetInt32(6)),
                   Convert.ToString(reader.GetInt32(7)),

                };

                    var listitem = new ListViewItem(row);

                    listView1.Items.Add(listitem);

                }
                Conexao.fechar();
            }
        }

        private void addusu_Click(object sender, EventArgs e)
        {
            modusu.BackColor = Color.Transparent;
            addusu.BackColor = Color.White;
            label14.Text = "add";

            if (Convert.ToInt32(label3.Text) == 2)
            {
                listView1.Visible = false;
                label5.Visible = true;
                textBox1.Visible = true;
                label4.Visible = true;
                textBox2.Visible = true;
                label6.Visible = true;
                textBox3.Visible = true;
                label7.Visible = true;
                textBox4.Visible = true;
                label8.Visible = true;
                textBox5.Visible = true;
                label9.Visible = true;
                textBox6.Visible = true;
                label10.Visible = true;
                textBox7.Visible = true;
                label11.Visible = true;
                textBox8.Visible = true;
                button1.Visible = true;
            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listView1.SelectedItems)
            {

                string query = "SELECT * FROM login where ID = '" + item.Text + "';";

                MySqlConnection dbcon = Conexao.abrir();

                MySqlCommand commdb = new MySqlCommand(query, dbcon);

                MySqlDataReader reader;
                reader = commdb.ExecuteReader();
                reader.Read();

                textBox1.Text = Convert.ToString(reader.GetInt32(0));
                textBox2.Text = reader.GetString(1);
                textBox3.Text = reader.GetString(2);
                textBox4.Text = Convert.ToString(reader.GetInt32(3));
                textBox5.Text = Convert.ToString(reader.GetInt32(4));
                textBox6.Text = Convert.ToString(reader.GetInt32(5));
                textBox7.Text = Convert.ToString(reader.GetInt32(6));
                textBox8.Text = Convert.ToString(reader.GetInt32(7));

                Conexao.fechar();

            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            if (label14.Text == "mod")
            {
                if (Convert.ToInt32(label3.Text) == 1)
                {
                    string query = "UPDATE login SET Nome = '" + textBox2.Text + "',Senha = '" + textBox3.Text + "' WHERE ID = '" + textBox1.Text + "'";
                    MySqlConnection dbcon = Conexao.abrir();

                    MySqlCommand commdb = new MySqlCommand(query, dbcon);

                    commdb.ExecuteNonQuery();

                    Conexao.fechar();
                }
                else if (Convert.ToInt32(label3.Text) == 2)
                {
                    string query = "UPDATE login SET Nome = '" + textBox2.Text + "',Senha = '" + textBox3.Text + "',permissao = '" + textBox6.Text + "',estoque = '"+textBox4.Text+"',financeiro = '"+textBox7.Text+"',ti = '"+textBox5.Text+"',conf = '"+textBox8.Text+"' WHERE ID = '" + textBox1.Text + "'";
                    MySqlConnection dbcon = Conexao.abrir();

                    MySqlCommand commdb = new MySqlCommand(query, dbcon);

                    commdb.ExecuteNonQuery();

                    Conexao.fechar();
                }

                MessageBox.Show("Modificação Feita com Sucesso!");

                textBox1.Clear();
                textBox2.Clear();
                textBox3.Clear();
                textBox4.Clear();
                textBox5.Clear();
                textBox6.Clear();
                textBox7.Clear();
                textBox8.Clear();
            }
            else if(label14.Text == "add")
            {
                string query = "INSERT INTO login(Nome,Senha,permissao,Estoque,Financeiro,TI,Conf) VALUES( Nome = '" + textBox2.Text + "',Senha = '" + textBox3.Text + "',permissao = '" + textBox6.Text + "',estoque = '" + textBox4.Text + "',financeiro = '" + textBox7.Text + "',ti = '" + textBox5.Text + "',conf = '" + textBox8.Text + "');";
                MySqlConnection dbcon = Conexao.abrir();

                MySqlCommand commdb = new MySqlCommand(query, dbcon);

                commdb.ExecuteNonQuery();

                if (commdb.LastInsertedId != 0)
                    commdb.Parameters.Add(new MySqlParameter("ultimoId", commdb.LastInsertedId));

                string y = Convert.ToString(commdb.Parameters["@ultimoId"].Value);

                Conexao.fechar();

            MessageBox.Show("Novo Usuário Adicionado com Sucesso!");
            MessageBox.Show("ID do Usuário "+textBox2.Text+":"+y);

            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
            textBox6.Clear();
            textBox7.Clear();
            textBox8.Clear();
        }
        }
    }
}
