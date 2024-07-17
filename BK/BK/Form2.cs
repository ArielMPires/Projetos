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
    public partial class Form2 : Form
    {
        public Form2(string id)
        {
            InitializeComponent();
            Conexao.fechar();
                listView1.View = View.Details;
                listView1.AllowColumnReorder = true;
                listView1.FullRowSelect = true;
                listView1.GridLines = true;

                listView1.Columns.Add("Nº da Ordem:", 80, HorizontalAlignment.Left);
                listView1.Columns.Add("ID", 80, HorizontalAlignment.Left);
                listView1.Columns.Add("Nome:", 140, HorizontalAlignment.Left);
                listView1.Columns.Add("Data e Hora:", 150, HorizontalAlignment.Left);
                listView1.Columns.Add("Situação:", 110, HorizontalAlignment.Left);

                string query4 = "SELECT * FROM BK where situacao = 'Agendado'";

                MySqlConnection dbcon4 = Conexao.abrir();

                MySqlCommand commdb4 = new MySqlCommand(query4, dbcon4);

                MySqlDataReader data = commdb4.ExecuteReader();

                listView1.Items.Clear();


                while (data.Read())
                {

                    string[] row = {
                    Convert.ToString(data.GetInt32(0)),
                    Convert.ToString(data.GetInt32(1)),
                    data.GetString(2),
                    Convert.ToString(data.GetDateTime(3)),
                    data.GetString(4)
                };

                    var listitem = new ListViewItem(row);

                    listView1.Items.Add(listitem);

                }
                Conexao.fechar();

                listView2.View = View.Details;
                listView2.AllowColumnReorder = true;
                listView2.FullRowSelect = true;
                listView2.GridLines = true;

                listView2.Columns.Add("Nº da Ordem:", 80, HorizontalAlignment.Left);
                listView2.Columns.Add("ID", 80, HorizontalAlignment.Left);
                listView2.Columns.Add("Nome:", 140, HorizontalAlignment.Left);
                listView2.Columns.Add("Data e Hora:", 150, HorizontalAlignment.Left);
                listView2.Columns.Add("Situação:", 110, HorizontalAlignment.Left);

                string query5 = "SELECT * FROM BK where situacao = 'Backup Feito'";

                MySqlConnection dbcon5 = Conexao.abrir();

                MySqlCommand commdb5 = new MySqlCommand(query5, dbcon5);

                MySqlDataReader data1 = commdb5.ExecuteReader();

                listView2.Items.Clear();


                while (data1.Read())
                {


                    string[] row2 = {
                    Convert.ToString(data1.GetInt32(0)),
                    Convert.ToString(data1.GetInt32(1)),
                    data1.GetString(2),
                    Convert.ToString(data1.GetDateTime(3)),
                    data1.GetString(4)
                };

                    var listitem = new ListViewItem(row2);

                    listView2.Items.Add(listitem);

                }
                Conexao.fechar();


        }

        private void button1_Click(object sender, EventArgs e)
        {
            listView1.Clear();
            listView2.Clear();
            listView1.View = View.Details;
            listView1.AllowColumnReorder = true;
            listView1.FullRowSelect = true;
            listView1.GridLines = true;

            listView1.Columns.Add("Nº da Ordem:", 80, HorizontalAlignment.Left);
            listView1.Columns.Add("ID", 80, HorizontalAlignment.Left);
            listView1.Columns.Add("Nome:", 140, HorizontalAlignment.Left);
            listView1.Columns.Add("Data e Hora:", 150, HorizontalAlignment.Left);
            listView1.Columns.Add("Situação:", 110, HorizontalAlignment.Left);

            string query4 = "SELECT * FROM BK where situacao = 'Agendado'";

            MySqlConnection dbcon4 = Conexao.abrir();

            MySqlCommand commdb4 = new MySqlCommand(query4, dbcon4);

            MySqlDataReader data = commdb4.ExecuteReader();

            listView1.Items.Clear();


            while (data.Read())
            {

                string[] row = {
                    Convert.ToString(data.GetInt32(0)),
                    Convert.ToString(data.GetInt32(1)),
                    data.GetString(2),
                    Convert.ToString(data.GetDateTime(3)),
                    data.GetString(4)
                };

                var listitem = new ListViewItem(row);

                listView1.Items.Add(listitem);

            }
            Conexao.fechar();

            Conexao.fechar();
            listView2.View = View.Details;
            listView2.AllowColumnReorder = true;
            listView2.FullRowSelect = true;
            listView2.GridLines = true;

            listView2.Columns.Add("Nº da Ordem:", 80, HorizontalAlignment.Left);
            listView2.Columns.Add("ID", 80, HorizontalAlignment.Left);
            listView2.Columns.Add("Nome:", 140, HorizontalAlignment.Left);
            listView2.Columns.Add("Data e Hora:", 150, HorizontalAlignment.Left);
            listView2.Columns.Add("Situação:", 110, HorizontalAlignment.Left);

            string query5 = "SELECT * FROM BK where situacao = 'Backup Feito'";

            MySqlConnection dbcon5 = Conexao.abrir();

            MySqlCommand commdb5 = new MySqlCommand(query5, dbcon5);

            MySqlDataReader data1 = commdb5.ExecuteReader();

            listView2.Items.Clear();


            while (data1.Read())
            {


                string[] row = {
                    Convert.ToString(data1.GetInt32(0)),
                    Convert.ToString(data1.GetInt32(1)),
                    data1.GetString(2),
                    Convert.ToString(data1.GetDateTime(3)),
                    data1.GetString(4)
                };

                var listitem = new ListViewItem(row);

                listView2.Items.Add(listitem);

            }
            Conexao.fechar();
        }

        private void Agendar_Click(object sender, EventArgs e)
        {


            string query3 = "SELECT Nome FROM login where ID ='" + textBox1.Text + "';";

            MySqlConnection dbcon3 = Conexao.abrir();

            MySqlCommand commdb3 = new MySqlCommand(query3, dbcon3);

            MySqlDataReader reader3;
            reader3 = commdb3.ExecuteReader();
            reader3.Read();
            string nome = reader3.GetString(0);

            Conexao.fechar();


            string query = "INSERT INTO BK(ID,nome,dh,situacao) VALUES('" + textBox1.Text + "','"+nome+"','"+dateTimePicker1.Value.ToString("yyyy/MM/dd HH:mm:ss")+"','Agendado')";

            MySqlConnection dbcon = Conexao.abrir();

            MySqlCommand commdb = new MySqlCommand(query, dbcon);

            commdb.ExecuteNonQuery();

            Conexao.fechar();
            dateTimePicker1.Value = DateTime.Now.AddDays(1);
            textBox1.Clear();

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }
    }
}
