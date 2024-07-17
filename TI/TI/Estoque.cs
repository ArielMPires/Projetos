using iTextSharp.text;
using iTextSharp.text.pdf;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TI;
using Rectangle = iTextSharp.text.Rectangle;

namespace WCS
{
    public partial class Estoque : Form
    {
        public Estoque(string id,string nome, string per)
        {
            InitializeComponent();
            label4.Text =  id;
            label5.Text = nome;
            label3.Text = per;
            label3.Visible = false;
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            //pedidos
            Pedidos.BackColor = Color.White;
            Protocolo.BackColor = Color.Transparent;
            Planilha.BackColor = Color.Transparent;
            Vale.BackColor = Color.Transparent;

            label6.Text = "Nº Pedido";
            label7.Text = "Nome";
            label8.Text = "Vendedor";
            label6.Visible = true;
            label7.Visible = true;
            label8.Visible = true;

            textBox1.Visible = true;
            textBox2.Visible = true;
            textBox3.Visible = true;

            Sep1.Text = "Separado";
            Conf1.Text = "Conferido";
            Sep1.Visible = true;
            Conf1.Visible = true;

            tabPage1.Text = "Pedidos P/ Separar";
            tabPage2.Text = "Pedidos P/ Conferir";
            tabControl1.Visible = true;

            button1.Text = "Assinar";
            button2.Text = "Adicionar Pedido";
            button1.Visible = true;
            button2.Visible = true;

            pictureBox2.Visible = true;
            pictureBox3.Visible = true;


            listView1.View = View.Details;
            listView1.AllowColumnReorder = true;
            listView1.FullRowSelect = true;
            listView1.GridLines = true;

            listView1.Columns.Add("Nº Pedido:", 80, HorizontalAlignment.Left);
            listView1.Columns.Add("Razão Social", 130, HorizontalAlignment.Left);
            listView1.Columns.Add("Vendedor:", 80, HorizontalAlignment.Left);
            listView1.Columns.Add("Separado Por:", 100, HorizontalAlignment.Left);
            listView1.Columns.Add("Conferido Por:", 100, HorizontalAlignment.Left);
            listView1.Columns.Add("Volume:", 80, HorizontalAlignment.Left);
            listView1.Columns.Add("Data:", 70, HorizontalAlignment.Left);
            listView1.Columns.Add("Observações:", 130, HorizontalAlignment.Left);

            listView2.View = View.Details;
            listView2.AllowColumnReorder = true;
            listView2.FullRowSelect = true;
            listView2.GridLines = true;

            listView2.Columns.Add("Nº Pedido:", 80, HorizontalAlignment.Left);
            listView2.Columns.Add("Razão Social", 130, HorizontalAlignment.Left);
            listView2.Columns.Add("Vendedor:", 80, HorizontalAlignment.Left);
            listView2.Columns.Add("Separado Por:", 100, HorizontalAlignment.Left);
            listView2.Columns.Add("Conferido Por:", 100, HorizontalAlignment.Left);
            listView2.Columns.Add("Volume:", 80, HorizontalAlignment.Left);
            listView2.Columns.Add("Data:", 70, HorizontalAlignment.Left);
            listView2.Columns.Add("Observações:", 130, HorizontalAlignment.Left);


            string query = "SELECT * FROM pedidos where Separado = ''";

            MySqlConnection dbcon = Conexao.abrir();

            MySqlCommand commdb = new MySqlCommand(query, dbcon);

            MySqlDataReader data = commdb.ExecuteReader();

            listView1.Items.Clear();


            while (data.Read())
            {


                string[] row = {
                    data.GetString(0),
                    data.GetString(1),
                    data.GetString(2),
                    data.GetString(3),
                    data.GetString(4),
                    data.GetString(5),
                    Convert.ToString(data.GetDateTime(6)),
                    data.GetString(7)
                };

                var listitem = new ListViewItem(row);

                listView1.Items.Add(listitem);

            }
            Conexao.fechar();

            string query2 = "SELECT * FROM pedidos where conferido = ''";

            MySqlConnection dbcon2 = Conexao.abrir();

            MySqlCommand commdb2 = new MySqlCommand(query2, dbcon2);

            MySqlDataReader reader2;
            reader2 = commdb2.ExecuteReader();

            listView2.Items.Clear();

            while (reader2.Read())
            {
                
                string[] row = {
                    reader2.GetString(0),
                    reader2.GetString(1),
                    reader2.GetString(2),
                    reader2.GetString(3),
                    reader2.GetString(4),
                    reader2.GetString(5),
                    Convert.ToString(reader2.GetDateTime(6)),
                    reader2.GetString(7)
                };

                var listitem = new ListViewItem(row);

                listView2.Items.Add(listitem);


            }
            Conexao.fechar();

        }

        private void button5_Click(object sender, EventArgs e)
        {
            
            this.Close();
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            //protocolos
            Pedidos.BackColor = Color.Transparent;
            Protocolo.BackColor = Color.White;
            Planilha.BackColor = Color.Transparent;
            Vale.BackColor = Color.Transparent;

            label6.Text = "NF'S/Cliente";
            label7.Text = "Material P/";
            label8.Text = "Quantidade";
            label9.Text = "Solicitante";
            label10.Text = "Observações";
            label6.Visible = true;
            label7.Visible = true;
            label8.Visible = true;
            label9.Visible = true;
            label10.Visible = true;

            textBox1.Visible = true;
            textBox2.Visible = true;
            textBox3.Visible = true;
            textBox5.Visible = true;
            textBox6.Visible = true;

            Sep1.Text = "São Paulo";
            Conf1.Text = "Mogi Das Cruzes";
            Sep1.Visible = true;
            Conf1.Visible = true;

            tabPage1.Text = "Protocolos";
            tabPage2.Text = "Itens Do ultimo protocolo";
            tabControl1.Visible = true;

            button1.Text = "Adicionar item";
            button2.Text = "Editar Protocolo";
            button1.Visible = true;
            button2.Visible = true;
            button3.Visible = true;

            pictureBox2.Visible = true;
            pictureBox3.Visible = true;


        }

        private void Planilha_Click(object sender, EventArgs e)
        {
            Pedidos.BackColor = Color.Transparent;
            Protocolo.BackColor = Color.Transparent;
            Planilha.BackColor = Color.White;
            Vale.BackColor = Color.Transparent;
            listView2.Items.Clear();
            listView1.Items.Clear();

            label6.Text = "";
            label7.Text = "";
            label8.Text = "";
            label6.Visible = false;
            label7.Visible = false;
            label8.Visible = false;

            textBox1.Visible = false;
            textBox2.Visible = false;
            textBox3.Visible = true;

            Sep1.Text = "Nº Pedido";
            Conf1.Text = "Nome";
            Sep1.Visible = true;
            Conf1.Visible = true;

            tabPage1.Text = "Pedidos";
            tabPage2.Text = "";
            tabControl1.Visible = true;

            button1.Text = "";
            button2.Text = "Pesquisar";
            button1.Visible = false;
            button2.Visible = true;
            pictureBox2.Visible = false;
            pictureBox3.Visible = false;

            listView1.Clear();

            listView1.View = View.Details;
            listView1.AllowColumnReorder = true;
            listView1.FullRowSelect = true;
            listView1.GridLines = true;

            listView1.Columns.Add("Nº Pedido:", 80, HorizontalAlignment.Left);
            listView1.Columns.Add("Razão Social", 130, HorizontalAlignment.Left);
            listView1.Columns.Add("Vendedor:", 80, HorizontalAlignment.Left);
            listView1.Columns.Add("Separado Por:", 100, HorizontalAlignment.Left);
            listView1.Columns.Add("Conferido Por:", 100, HorizontalAlignment.Left);
            listView1.Columns.Add("Volume:", 80, HorizontalAlignment.Left);
            listView1.Columns.Add("Data:", 70, HorizontalAlignment.Left);
            listView1.Columns.Add("Observações:", 130, HorizontalAlignment.Left);


            string query = "SELECT * FROM pedidos";

            MySqlConnection dbcon = Conexao.abrir();

            MySqlCommand commdb = new MySqlCommand(query, dbcon);

            MySqlDataReader data = commdb.ExecuteReader();

            listView1.Items.Clear();


            while (data.Read())
            {

                
                string[] row = {
                    
                    data.GetString(0),
                    data.GetString(1),
                    data.GetString(2),
                    data.GetString(3),
                    data.GetString(4),
                    data.GetString(5),
                    Convert.ToString(data.GetDateTime(6)),
                    data.GetString(7)
                };

                var listitem = new ListViewItem(row);

                listView1.Items.Add(listitem);

            }
            Conexao.fechar();

        }

        private void Vale_Click(object sender, EventArgs e)
        {
            Pedidos.BackColor = Color.Transparent;
            Protocolo.BackColor = Color.Transparent;
            Planilha.BackColor = Color.Transparent;
            Vale.BackColor = Color.White;

            label6.Text = "";
            label7.Text = "";
            label8.Text = "";
            label6.Visible = false;
            label7.Visible = false;
            label8.Visible = false;

            textBox1.Visible = false;
            textBox2.Visible = false;
            textBox3.Visible = false;

            Sep1.Text = "";
            Conf1.Text = "";
            Sep1.Visible = false;
            Conf1.Visible = false;

            tabPage1.Text = "";
            tabPage2.Text = "";
            tabControl1.Visible = false;

            button1.Text = "";
            button2.Text = "";
            button1.Visible = false;
            button2.Visible = false;

            pictureBox2.Visible = false;
            pictureBox3.Visible = false;

            MessageBox.Show("Função não está Ativada!");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string t = "Adicionar Pedido";
            string p = "Pesquisar";
            if (button2.Text == t)
            {

                if (Convert.ToInt32(label3.Text) >= 2)
                {
                    string Pedido = textBox1.Text;
                    string nome = textBox2.Text;

                    string query = "INSERT INTO pedidos(pedido,razao,vendedor,data) VALUES('" + textBox1.Text + "', '" + textBox2.Text + "', '" + textBox3.Text + "' ,NOW())";

                    MySqlConnection dbcon = Conexao.abrir();

                    MySqlCommand commdb = new MySqlCommand(query, dbcon);

                    commdb.ExecuteNonQuery();

                    Conexao.fechar();


                    //define as propriedades do controle 
                    //OpenFileDialog
                    this.ofd1.Multiselect = true;
                    this.ofd1.Title = "Selecionar PDF";
                    ofd1.InitialDirectory = @"C:\";
                    //filtra para exibir somente arquivos de imagens
                    ofd1.Filter = "pdf files (*.pdf)|*.pdf|All files (*.*)|*.*";
                    ofd1.CheckFileExists = true;
                    ofd1.CheckPathExists = true;
                    ofd1.FilterIndex = 2;
                    ofd1.RestoreDirectory = true;
                    ofd1.ReadOnlyChecked = true;
                    ofd1.ShowReadOnly = true;

                    DialogResult dr = this.ofd1.ShowDialog();

                    if (dr == System.Windows.Forms.DialogResult.OK)
                    {
                        foreach (String arquivo in ofd1.FileNames)
                        {
                            textBox4.Text += arquivo;
                            try
                            {
                                string oldfile = @textBox4.Text;
                                string newfile = @"Pedidos\" + Pedido + ".pdf";
                                PdfReader reader = new PdfReader(oldfile);
                                Rectangle size = reader.GetPageSizeWithRotation(1);
                                Document document = new Document(size);

                                FileStream fs = new FileStream(newfile, FileMode.Create, FileAccess.Write);
                                PdfWriter writer = PdfWriter.GetInstance(document, fs);
                                document.Open();

                                PdfContentByte cb = writer.DirectContent;

                                BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                                cb.SetColorFill(BaseColor.BLACK);
                                cb.SetFontAndSize(bf, 16);
                                for (int i = 1; i <= reader.NumberOfPages; i++)
                                {
                                    if (i == 1)
                                    {
                                        PdfImportedPage page = writer.GetImportedPage(reader, i);
                                        cb.AddTemplate(page, 0, 0);

                                    }
                                    else if (i >= 2)
                                    {

                                        document.NewPage();
                                        PdfImportedPage page = writer.GetImportedPage(reader, i);
                                        cb.AddTemplate(page, 0, 0);
                                    }

                                }

                                document.Close();
                                fs.Close();
                                writer.Close();
                                reader.Close();


                                MessageBox.Show("Pedido Adicionado Com Sucesso");
                                textBox3.Clear();
                                textBox1.Clear();
                                textBox2.Clear();
                                textBox4.Clear();


                                ConexaoFTP.EnviarArquivoFTP(newfile, Pedido);

                            }

                            catch (SecurityException ex)
                            {
                                MessageBox.Show("Erro de segurança Contate o administrador.\n\n" +
                                                            "Mensagem : " + ex.Message + "\n\n" +
                                                            "Detalhes (enviar ao suporte):\n\n" + ex.StackTrace);
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Você Não tem Permissão para acessar esta Parte!");
                }
            }else if (button2.Text == p)
            {
                string query = "SELECT * FROM pedidos";

                MySqlConnection dbcon = Conexao.abrir();

                MySqlCommand commdb = new MySqlCommand(query, dbcon);

                MySqlDataReader data = commdb.ExecuteReader();

                listView1.Items.Clear();


                while (data.Read())
                {


                    string[] row = {
                    data.GetString(0),
                    data.GetString(1),
                    data.GetString(2),
                    data.GetString(3),
                    data.GetString(4),
                    data.GetString(5),
                    Convert.ToString(data.GetDateTime(6)),
                    data.GetString(7)
                };

                    var listitem = new ListViewItem(row);

                    listView1.Items.Add(listitem);

                }
                Conexao.fechar();


                if (Sep1.Checked) {
                    string query2 = "SELECT * FROM pedidos where pedido ='"+textBox3.Text+"'";

                    MySqlConnection dbcon2 = Conexao.abrir();

                    MySqlCommand commdb2 = new MySqlCommand(query2, dbcon2);

                    MySqlDataReader reader2;
                    reader2 = commdb2.ExecuteReader();

                    listView1.Items.Clear();

                    while (reader2.Read())
                    {

                        string[] row = {
                    reader2.GetString(0),
                    reader2.GetString(1),
                    reader2.GetString(2),
                    reader2.GetString(3),
                    reader2.GetString(4),
                    reader2.GetString(5),
                    Convert.ToString(reader2.GetDateTime(6)),
                    reader2.GetString(7)
                };

                        var listitem = new ListViewItem(row);

                        listView1.Items.Add(listitem);


                    }
                    Conexao.fechar();
                }
                else if (Conf1.Checked)
                {
                    string query2 = "SELECT * FROM pedidos where razao ='" + textBox3.Text + "'";

                    MySqlConnection dbcon2 = Conexao.abrir();

                    MySqlCommand commdb2 = new MySqlCommand(query2, dbcon2);

                    MySqlDataReader reader2;
                    reader2 = commdb2.ExecuteReader();

                    listView1.Items.Clear();

                    while (reader2.Read())
                    {

                        string[] row = {
                    reader2.GetString(0),
                    reader2.GetString(1),
                    reader2.GetString(2),
                    reader2.GetString(3),
                    reader2.GetString(4),
                    reader2.GetString(5),
                    Convert.ToString(reader2.GetDateTime(6)),
                    reader2.GetString(7)
                };

                        var listitem = new ListViewItem(row);

                        listView1.Items.Add(listitem);


                    }
                    Conexao.fechar();
                }
            }
        }

        private void Sep1_CheckedChanged(object sender, EventArgs e)
        {
            if (Conf1.Checked)
            {
                Sep1.CheckState = CheckState.Unchecked;
            }
        }

        private void Conf1_CheckedChanged(object sender, EventArgs e)
        {
            if (Sep1.Checked)
            {
                Conf1.CheckState = CheckState.Unchecked;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string id = label4.Text;
            string nome = label5.Text;
            string per = label3.Text;

            if (Convert.ToInt32(per) >= 1)
            {
                if (Sep1.Checked)
                {
                    foreach (ListViewItem item in listView1.Items)
                    {
                        if (item.Checked)
                        {
                            string pedido = item.Text;

                            string query = "UPDATE pedidos SET separado = '" + nome + "',data = NOW() WHERE pedido = '" + pedido + "'";
                            MySqlConnection dbcon = Conexao.abrir();

                            MySqlCommand commdb = new MySqlCommand(query, dbcon);

                            commdb.ExecuteNonQuery();

                            Conexao.fechar();

                            MessageBox.Show("Assinado!");

                            Sep1.Checked = false;
                            Conf1.Checked = false;

                        }
                        else
                        {
                        }
                    }

                }
                else if (Conf1.Checked)
                {
                    foreach (ListViewItem item in listView1.Items)
                    {
                        if (item.Checked)
                        {
                            string pedido = item.Text;

                            string query = "UPDATE pedidos SET conferido = '" + id + "',data = NOW() WHERE pedido = '" + pedido + "'";
                            MySqlConnection dbcon = Conexao.abrir();

                            MySqlCommand commdb = new MySqlCommand(query, dbcon);

                            commdb.ExecuteNonQuery();

                            Conexao.fechar();
                            MessageBox.Show("Assinado!");

                                                        Sep1.Checked = false;
                            Conf1.Checked = false;

                        }
                        else
                        {
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Você não selecionou nenhuma das Opções!");
                }
            }
            else
            {
                MessageBox.Show("Você Não tem Permissão para acessar esta Parte!");
            }
        }

        private void pictureBox2_Click_1(object sender, EventArgs e)
        {

            //separar
            string query = "SELECT * FROM pedidos where Separado = ''";

            MySqlConnection dbcon = Conexao.abrir();

            MySqlCommand commdb = new MySqlCommand(query, dbcon);

            MySqlDataReader data = commdb.ExecuteReader();

            listView1.Items.Clear();


            while (data.Read())
            {


                string[] row = {
                    data.GetString(0),
                    data.GetString(1),
                    data.GetString(2),
                    data.GetString(3),
                    data.GetString(4),
                    data.GetString(5),
                    Convert.ToString(data.GetDateTime(6)),
                    data.GetString(7)
                };

                var listitem = new ListViewItem(row);

                listView1.Items.Add(listitem);

            }
            Conexao.fechar();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            string query2 = "SELECT * FROM pedidos where conferido = ''";

            MySqlConnection dbcon2 = Conexao.abrir();

            MySqlCommand commdb2 = new MySqlCommand(query2, dbcon2);

            MySqlDataReader reader2;
            reader2 = commdb2.ExecuteReader();

            listView2.Items.Clear();

            while (reader2.Read())
            {
                string[] row = {
                    reader2.GetString(0),
                    reader2.GetString(1),
                    reader2.GetString(2),
                    reader2.GetString(3),
                    reader2.GetString(4),
                    reader2.GetString(5),
                    Convert.ToString(reader2.GetDateTime(6)),
                    reader2.GetString(7)
                };

                var listitem = new ListViewItem(row);

                listView2.Items.Add(listitem);


            }
            Conexao.fechar();
        }

        private void Estoque_Load(object sender, EventArgs e)
        {

        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listView1.SelectedItems)
            {

                var uri = "http://192.168.1.68/waldesa/pedidos/" + item.Text + ".pdf";
                var psi = new System.Diagnostics.ProcessStartInfo();
                psi.UseShellExecute = true;
                psi.FileName = uri;
                System.Diagnostics.Process.Start(psi);
            }
        }
    }
}
