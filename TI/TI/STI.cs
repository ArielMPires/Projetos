
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using iTextSharp.text;
using iTextSharp.text.pdf;
using MySqlConnector;
using TI;
using Rectangle = iTextSharp.text.Rectangle;

namespace WCS
{
    public partial class STI : Form
    {
        public STI(string id, string nome, string per)
        {
            InitializeComponent();
            Info info = new Info();
            label9.Text = info.id;
            label8.Text = info.nome;

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            //O.S
            almo.BackColor = Color.Transparent;
            os.BackColor = Color.White;
            tarefa.BackColor = Color.Transparent;
            modi.BackColor = Color.Transparent;
            senha.BackColor = Color.Transparent;


            label6.Text = "Solicitante:";
            label5.Text = "Qual Problema?:";
            label6.Visible = true;
            label5.Visible = true;
            label10.Text = "Motivo:";
            label10.Visible = true;

            checkBox1.Text = "1º Item:";
            checkBox2.Text = "2º Item:";
            checkBox3.Text = "3º Item:";
            checkBox1.Enabled = true;
            checkBox1.Checked = false;
            checkBox2.Enabled = true;
            checkBox2.Checked = false;
            checkBox3.Enabled = true;
            checkBox3.Checked = false;
            checkBox3.Visible = true;



            textBox1.Visible = true;
            textBox2.Visible = true;
            textBox3.Visible = true;
            textBox14.Visible = false;
            textBox7.Visible = false;
            textBox8.Visible = false;
            textBox9.Visible = false;
            textBox10.Visible = false;


            tabPage1.Text = "O.S Pendentes";
            tabPage2.Text = "O.S Finalizadas";
            tabPage3.Text = "Finalizar O.S";
            tabPage4.Text = "";
            tabControl1.Visible = true;

            button1.Text = "Adicionar O.S";
            button1.Visible = true;
            button2.Text = "Finalizar O.S";
            button2.Visible = true;
            button3.Text = "Pegar O.S";
            button3.Visible = true;
            button4.Text = "Cancelar O.S";
            button4.Visible = true;

            listView4.Visible = false;


            listView1.View = View.Details;
            listView1.AllowColumnReorder = true;
            listView1.FullRowSelect = true;
            listView1.GridLines = true;
            listView1.Columns.Clear();

            listView1.Columns.Add("Nº O.S:", 80, HorizontalAlignment.Left);
            listView1.Columns.Add("Qual Problema", 130, HorizontalAlignment.Left);
            listView1.Columns.Add("Solicitante:", 80, HorizontalAlignment.Left);
            listView1.Columns.Add("Data Solicitada:", 100, HorizontalAlignment.Left);
            listView1.Columns.Add("Resolvendo Por:", 100, HorizontalAlignment.Left);

            string query = "SELECT Ordem,fazer,solicitante,dtsol,pego FROM chamados WHERE situacao = 'Esperando Atendente'";

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
                    Convert.ToString(reader.GetDateTime(3).ToString("dd/MM/yyyy")),
                    reader.GetString(4)

                };

                var listitem = new ListViewItem(row);

                listView1.Items.Add(listitem);

            }
            Conexao.fechar();

            listView2.View = View.Details;
            listView2.AllowColumnReorder = true;
            listView2.FullRowSelect = true;
            listView2.GridLines = true;

            listView2.Columns.Clear();

            listView2.Columns.Add("Nº O.S:", 80, HorizontalAlignment.Left);
            listView2.Columns.Add("Qual Problema", 130, HorizontalAlignment.Left);
            listView2.Columns.Add("Solicitante:", 80, HorizontalAlignment.Left);
            listView2.Columns.Add("Data Solicitada:", 100, HorizontalAlignment.Left);
            listView2.Columns.Add("Resolvendo Por:", 100, HorizontalAlignment.Left);
            listView2.Columns.Add("Data Finalizada:", 80, HorizontalAlignment.Left);

            string query2 = "SELECT ordem,fazer,solicitante,dtsol,pego,dtfin FROM chamados WHERE situacao = 'Finalizado'";

            MySqlConnection dbcon2 = Conexao.abrir();

            MySqlCommand com2 = new MySqlCommand(query2, dbcon2);

            MySqlDataReader reader2 = com2.ExecuteReader();

            listView2.Items.Clear();


            while (reader2.Read())
            {


                string[] row = {
                    Convert.ToString(reader2.GetInt32(0)),
                    reader2.GetString(1),
                    reader2.GetString(2),
                    Convert.ToString(reader2.GetDateTime(3).ToString("dd/MM/yyyy")),
                    reader2.GetString(4),
                    Convert.ToString(reader2.GetDateTime(5).ToString("dd/MM/yyyy"))

                };

                var listitem = new ListViewItem(row);

                listView2.Items.Add(listitem);

            }
            Conexao.fechar();

            listView3.CheckBoxes = true;

            listView3.View = View.Details;
            listView3.AllowColumnReorder = true;
            listView3.FullRowSelect = true;
            listView3.GridLines = true;

            listView3.Columns.Clear();

            listView3.Columns.Add("Nº O.S:", 80, HorizontalAlignment.Left);
            listView3.Columns.Add("Qual Problema", 130, HorizontalAlignment.Left);
            listView3.Columns.Add("Solicitante:", 80, HorizontalAlignment.Left);
            listView3.Columns.Add("Data Solicitada:", 100, HorizontalAlignment.Left);
            listView3.Columns.Add("Resolvendo Por:", 100, HorizontalAlignment.Left);

            string query3 = "SELECT Ordem,fazer,solicitante,dtsol,pego FROM chamados WHERE situacao = 'Resolvendo'";

            MySqlConnection dbcon3 = Conexao.abrir();

            MySqlCommand com3 = new MySqlCommand(query3, dbcon3);

            MySqlDataReader reader3 = com3.ExecuteReader();

            listView3.Items.Clear();


            while (reader3.Read())
            {


                string[] row = {
                    Convert.ToString(reader3.GetInt32(0)),
                    reader3.GetString(1),
                    reader3.GetString(2),
                    Convert.ToString(reader3.GetDateTime(3).ToString("dd/MM/yyyy")),
                    reader3.GetString(4),

                };

                var listitem = new ListViewItem(row);

                listView3.Items.Add(listitem);

            }
            Conexao.fechar();

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
 
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string i = "Pegar O.S";
            string o = "Gerar PDF";

            if (button3.Text == i)
            {
                foreach (ListViewItem item in listView1.Items)
                {
                    if (item.Checked)
                    {
                        string os = item.Text;

                        string query = "UPDATE chamados SET pego = '"+label8.Text+"', situacao = 'Resolvendo' WHERE ordem = '" + os + "'";
                        MySqlConnection dbcon = Conexao.abrir();

                        MySqlCommand commdb = new MySqlCommand(query, dbcon);

                        commdb.ExecuteNonQuery();

                        Conexao.fechar();

                        item.Checked = false;
                        MessageBox.Show("O.S Pega!");

                    }
                    else if(item.Checked = false)
                    {
                        MessageBox.Show("Você não selecionou nenhum O.S");
                    }
                }
            }else if(button3.Text == o)
            {
                sfd1.Title = "Salvar PDF";
                sfd1.Filter = "pdf files (*.pdf)|*.pdf|All files (*.*)|*.*";
                sfd1.FilterIndex = 0;
                sfd1.FileName = "";
                sfd1.DefaultExt = ".pdf";
                sfd1.InitialDirectory = "";
                sfd1.RestoreDirectory = true;

                DialogResult result = sfd1.ShowDialog();
                if (result == DialogResult.OK)
                {
                    foreach (string newfile in sfd1.FileNames)
                    {

                        foreach (ListViewItem item in listView1.Items)
                        {
                            if (item.Checked)
                            {
                                string pedido = "Relatorio - " + item.Text;

                                string oldfile = @"Relatorio\" + pedido + ".pdf";


                                if (File.Exists(oldfile))
                                {
                                    continue;
                                }
                                else
                                {
                                    ConexaoPFtp.BaixarArquivoFTP(pedido);
                                }

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
                                for (int y = 1; y <= reader.NumberOfPages; y++)
                                {
                                    if (y == 1)
                                    {
                                        PdfImportedPage page = writer.GetImportedPage(reader, y);
                                        cb.AddTemplate(page, 0, 0);

                                    }
                                    else if (y >= 2)
                                    {

                                        document.NewPage();
                                        PdfImportedPage page = writer.GetImportedPage(reader, y);
                                        cb.AddTemplate(page, 0, 0);
                                    }

                                }

                                document.Close();
                                fs.Close();
                                writer.Close();
                                reader.Close();
                                MessageBox.Show("Relatório baixado com sucesso!");

                            }
                            else
                            {
                            }
                        }
                    }
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string i = "Cancelar O.S";

            string o = "Excluir peça";

            string c = "Cancelado - "+ textBox1.Text;

            if (!string.IsNullOrWhiteSpace(textBox1.Text))
            {
                if (button4.Text == i)
                {
                    foreach (ListViewItem item in listView1.Items)
                    {
                        if (item.Checked)
                        {
                            string os = item.Text;

                            string query = "UPDATE chamados SET fazer = '" + c + "', dtfin = CURDATE() WHERE ordem = '" + os + "'";
                            MySqlConnection dbcon = Conexao.abrir();

                            MySqlCommand commdb = new MySqlCommand(query, dbcon);

                            commdb.ExecuteNonQuery();

                            Conexao.fechar();

                        }
                        else if(item.Checked = false)
                        {
                            MessageBox.Show("Você não selecionou nenhum O.S!");
                        }
                    }
                }else if (button4.Text == o)
                {

                    foreach (ListViewItem item in listView1.Items)
                    {
                        if (item.Checked)
                        {
                            string pc = item.Text;

                            if (!string.IsNullOrWhiteSpace(textBox1.Text) && !string.IsNullOrWhiteSpace(textBox14.Text))
                            {
                                string query2 = "UPDATE mat SET quantidade = quantidade-" + textBox14.Text + " WHERE cod = '" + pc + "'";
                                MySqlConnection dbcon2 = Conexao.abrir();

                                MySqlCommand commdb2 = new MySqlCommand(query2, dbcon2);

                                commdb2.ExecuteNonQuery();

                                Conexao.fechar();

                                string query6 = "SELECT nome FROM mat where cod = '" + pc + "';";

                                MySqlConnection dbcon6 = Conexao.abrir();

                                MySqlCommand commdb6 = new MySqlCommand(query6, dbcon6);

                                MySqlDataReader reader6;
                                reader6 = commdb6.ExecuteReader();

                                reader6.Read();

                                string n = reader6.GetString(0);

                                Conexao.fechar();

                                string query5 = "INSERT INTO historico(movimento,cmat,nmat,qts,iduser,user,nfo,data,idemp,emp) VALUES('" + textBox1.Text + "','" + pc + "','" + n + "','" + textBox14.Text + "','" + label9.Text + "','" + label8.Text + "','" + os + "',CURDATE(),'12','Waldesa')";


                                MySqlConnection dbcon5 = Conexao.abrir();

                                MySqlCommand com5 = new MySqlCommand(query5, dbcon5);

                                com5.ExecuteNonQuery();

                                Conexao.fechar();

                                item.Checked = false;
                                textBox1.Text = "";
                                textBox14.Text = "";

                            }
                        }
                    }
                }
            }else
            {
                MessageBox.Show("Você não Adicionou um motivo para a exclusão dessa O.S!");
            }
        }

        private void almo_Click(object sender, EventArgs e)
        {
            almo.BackColor = Color.White;
            os.BackColor = Color.Transparent;
            tarefa.BackColor = Color.Transparent;
            modi.BackColor = Color.Transparent;
            senha.BackColor = Color.Transparent;


            label6.Text = "Codigo:";
            label5.Text = "";
            label6.Visible = true;
            label5.Visible = false;
            label10.Text = "Motivo:";
            label10.Visible = true;
            label11.Text = "Nº da NF:";
            label11.Visible = true;
            label12.Text = "Empresa:";
            label12.Visible = true;
            label13.Text = "Peça:";
            label13.Visible = true;
            label14.Text = "Quantidade:";
            label14.Visible = true;

            textBox1.Visible = true;
            textBox2.Visible = false;
            textBox3.Visible = true;
            textBox14.Visible = true;


            tabPage1.Text = "Exclusao";
            tabPage2.Text = "Materiais";
            tabPage3.Text = "Adicionar material";
            tabPage4.Text = "Adicionar peças";
            tabControl1.Visible = true;

            button1.Text = "Pesquisar";
            button1.Visible = true;
            button2.Text = "Adicionar Material";
            button2.Visible = true;
            button3.Text = "Pegar O.S";
            button3.Visible = false;
            button4.Text = "Excluir peça";
            button4.Visible = true;
            button6.Text = "Adicionar Peça";
            button6.Visible = true;

            checkBox1.Checked = true;
            checkBox1.Enabled = false;
            checkBox1.Text = "Descrição";
            checkBox1.Visible = true;
            checkBox2.Checked = true;
            checkBox2.Enabled = false;
            checkBox2.Text = "Modelo:";
            checkBox2.Visible = true;
            checkBox3.Visible = true;
            checkBox3.Text = "Preço:";
            checkBox3.Checked = true;
            checkBox3.Enabled = false;
            textBox6.Visible = true;

            textBox11.Visible = true;
            textBox5.Visible = true;
            textBox12.Visible = true;
            textBox6.Visible = true;
            textBox13.Visible = true;
            textBox7.Visible = true;
            textBox8.Visible = true;
            textBox9.Visible = true;
            textBox10.Visible = true;
            listView3.Visible = true;
            listView4.Visible = true;
            

            listView1.View = View.Details;
            listView1.AllowColumnReorder = true;
            listView1.FullRowSelect = true;
            listView1.GridLines = true;

            listView1.Columns.Clear();

            listView1.Columns.Add("Codigo:", 80, HorizontalAlignment.Left);
            listView1.Columns.Add("Descrição", 130, HorizontalAlignment.Left);
            listView1.Columns.Add("Quantidade:", 80, HorizontalAlignment.Left);
            listView1.Columns.Add("Modelo:", 100, HorizontalAlignment.Left);

            string query = "SELECT * FROM mat";

            MySqlConnection dbcon = Conexao.abrir();

            MySqlCommand com = new MySqlCommand(query, dbcon);

            MySqlDataReader reader = com.ExecuteReader();

            listView1.Items.Clear();


            while (reader.Read())
            {


                string[] row = {
                    Convert.ToString(reader.GetInt32(0)),
                    reader.GetString(1),
                   Convert.ToString(reader.GetInt32(2)),
                    reader.GetString(3),

                };

                var listitem = new ListViewItem(row);

                listView1.Items.Add(listitem);

            }
            Conexao.fechar();

            listView2.View = View.Details;
            listView2.AllowColumnReorder = true;
            listView2.FullRowSelect = true;
            listView2.GridLines = true;

            listView2.Columns.Clear();

            listView2.Columns.Add("Codigo:", 80, HorizontalAlignment.Left);
            listView2.Columns.Add("Descrição", 130, HorizontalAlignment.Left);
            listView2.Columns.Add("Quantidade:", 80, HorizontalAlignment.Left);
            listView2.Columns.Add("Modelo:", 100, HorizontalAlignment.Left);

            string query2 = "SELECT * FROM mat";

            MySqlConnection dbcon2 = Conexao.abrir();

            MySqlCommand com2 = new MySqlCommand(query2, dbcon2);

            MySqlDataReader reader2 = com2.ExecuteReader();

            listView2.Items.Clear();


            while (reader2.Read())
            {


                string[] row = {
                    Convert.ToString(reader2.GetInt32(0)),
                    reader2.GetString(1),
                   Convert.ToString(reader2.GetInt32(2)),
                    reader2.GetString(3),

                };

                var listitem = new ListViewItem(row);

                listView2.Items.Add(listitem);

            }
            Conexao.fechar();

            listView3.CheckBoxes = false;

            listView3.View = View.Details;
            listView3.AllowColumnReorder = true;
            listView3.FullRowSelect = true;
            listView3.GridLines = true;

            listView3.Columns.Clear();

            listView3.Columns.Add("Codigo:", 80, HorizontalAlignment.Left);
            listView3.Columns.Add("Descrição", 130, HorizontalAlignment.Left);
            listView3.Columns.Add("Quantidade:", 80, HorizontalAlignment.Left);
            listView3.Columns.Add("Modelo:", 100, HorizontalAlignment.Left);

            string query6 = "SELECT * FROM mat";

            MySqlConnection dbcon6 = Conexao.abrir();

            MySqlCommand com6 = new MySqlCommand(query6, dbcon6);

            MySqlDataReader reader6 = com6.ExecuteReader();

            listView3.Items.Clear();


            while (reader6.Read())
            {


                string[] row = {
                    Convert.ToString(reader6.GetInt32(0)),
                    reader6.GetString(1),
                   Convert.ToString(reader6.GetInt32(2)),
                    reader6.GetString(3),

                };

                var listitem = new ListViewItem(row);

                listView3.Items.Add(listitem);

            }
            Conexao.fechar();


            listView4.View = View.Details;
            listView4.AllowColumnReorder = true;
            listView4.FullRowSelect = true;
            listView4.GridLines = true;

            listView4.Columns.Clear();

            listView4.Columns.Add("Codigo:", 80, HorizontalAlignment.Left);
            listView4.Columns.Add("Nome Fantasia", 130, HorizontalAlignment.Left);
            listView4.Columns.Add("CNPJ/CPF:", 80, HorizontalAlignment.Left);
            listView4.Columns.Add("Nome da Conta:", 100, HorizontalAlignment.Left);

            string query5 = "SELECT cod,nfan,cnf,ncon FROM empresas";

            MySqlConnection dbcon5 = Conexao.abrir();

            MySqlCommand com5 = new MySqlCommand(query5, dbcon5);

            MySqlDataReader reader5 = com5.ExecuteReader();

            listView4.Items.Clear();


            while (reader5.Read())
            {


                string[] row = {
                    Convert.ToString(reader5.GetInt32(0)),
                    reader5.GetString(1),
                    reader5.GetString(2),
                    reader5.GetString(3),

                };

                var listitem = new ListViewItem(row);

                listView4.Items.Add(listitem);

            }
            Conexao.fechar();
        }

        private void tarefa_Click(object sender, EventArgs e)
        {
            almo.BackColor = Color.Transparent;
            os.BackColor = Color.Transparent;
            tarefa.BackColor = Color.White;
            modi.BackColor = Color.Transparent;
            senha.BackColor = Color.Transparent;
        }

        private void senha_Click(object sender, EventArgs e)
        {
            almo.BackColor = Color.Transparent;
            os.BackColor = Color.Transparent;
            tarefa.BackColor = Color.Transparent;
            modi.BackColor = Color.Transparent;
            senha.BackColor = Color.White;

            label6.Text = "";
            label5.Text = "";
            label6.Visible = false;
            label5.Visible = false;
            label10.Text = "";
            label10.Visible = false;
            label11.Text = "";
            label11.Visible = false;
            label12.Text = "";
            label12.Visible = false;
            label13.Text = "";
            label13.Visible = false;
            label14.Text = "";
            label14.Visible = false;

            textBox1.Visible = false;
            textBox2.Visible = false;
            textBox3.Visible = false;
            textBox14.Visible = false;
            textBox4.Visible = false;
            textBox11.Visible = false;
            textBox5.Visible = false;
            textBox12.Visible = false;
            textBox6.Visible = false;
            textBox13.Visible = false;
            textBox7.Visible = false;
            textBox8.Visible = false;
            textBox9.Visible = false;
            textBox10.Visible = false;
            listView3.Visible = false;
            listView4.Visible = false;



            tabPage1.Text = "Relatório Mensal";
            tabPage2.Text = "Historico de Entradas e Saidas";
            tabPage3.Text = "";
            tabPage4.Text = "";
            tabControl1.Visible = true;
            

            button1.Text = "";
            button1.Visible = false;
            button2.Text = "";
            button2.Visible = false;
            button3.Text = "Gerar PDF";
            button3.Visible = true;
            button4.Text = "";
            button4.Visible = false;
            button6.Text = "";
            button6.Visible = false;

            checkBox1.Checked = true;
            checkBox1.Enabled = false;
            checkBox1.Text = "";
            checkBox1.Visible = false;
            checkBox2.Checked = true;
            checkBox2.Enabled = false;
            checkBox2.Text = "";
            checkBox2.Visible = false;
            checkBox3.Text = "";
            checkBox3.Visible = false;
            checkBox3.Checked = true;
            checkBox3.Enabled = false;
            textBox6.Visible = false;

            listView1.View = View.Details;
            listView1.AllowColumnReorder = true;
            listView1.FullRowSelect = true;
            listView1.GridLines = true;

            listView1.Columns.Clear();

            listView1.Columns.Add("Codigo:", 150, HorizontalAlignment.Left);
            listView1.Columns.Add("Descrição", 150, HorizontalAlignment.Left);

            string query = "SELECT * FROM relatorio";

            MySqlConnection dbcon = Conexao.abrir();

            MySqlCommand com = new MySqlCommand(query, dbcon);

            MySqlDataReader reader = com.ExecuteReader();

            listView1.Items.Clear();


            while (reader.Read())
            {


                string[] row = {
                    reader.GetString(0),
                   Convert.ToString(reader.GetDateTime(1).ToString("dd/MM/yyyy")),

                };

                var listitem = new ListViewItem(row);

                listView1.Items.Add(listitem);

            }
            Conexao.fechar();

            listView2.View = View.Details;
            listView2.AllowColumnReorder = true;
            listView2.FullRowSelect = true;
            listView2.GridLines = true;

            listView2.Columns.Clear();

            listView2.Columns.Add("Nº:", 80, HorizontalAlignment.Left);
            listView2.Columns.Add("Movimentação:", 130, HorizontalAlignment.Left);
            listView2.Columns.Add("Material:", 80, HorizontalAlignment.Left);
            listView2.Columns.Add("Quantidade:", 100, HorizontalAlignment.Left);
            listView2.Columns.Add("Usuario:", 80, HorizontalAlignment.Left);
            listView2.Columns.Add("NF/OS:", 80, HorizontalAlignment.Left);
            listView2.Columns.Add("Empresa:", 80, HorizontalAlignment.Left);
            listView2.Columns.Add("Data:", 80, HorizontalAlignment.Left);

            string query2 = "SELECT nm,movimento,nmat,qts,user,nfo,emp,data FROM historico";

            MySqlConnection dbcon2 = Conexao.abrir();

            MySqlCommand com2 = new MySqlCommand(query2, dbcon2);

            MySqlDataReader reader2 = com2.ExecuteReader();

            listView2.Items.Clear();


            while (reader2.Read())
            {


                string[] row = {
                    Convert.ToString(reader2.GetInt32(0)),
                    reader2.GetString(1),
                    reader2.GetString(2),
                    Convert.ToString(reader2.GetInt32(3)),
                    reader2.GetString(4),
                    Convert.ToString(reader2.GetInt32(5)),
                    reader2.GetString(6),
                    Convert.ToString(reader2.GetDateTime(7).ToString("dd/MM/yyyy")),

                };

                var listitem = new ListViewItem(row);

                listView2.Items.Add(listitem);

            }
            Conexao.fechar();

        }

        private void modi_Click(object sender, EventArgs e)
        {
            almo.BackColor = Color.Transparent;
            os.BackColor = Color.Transparent;
            tarefa.BackColor = Color.Transparent;
            modi.BackColor = Color.White;
            senha.BackColor = Color.Transparent;

            try
            {
                string id = label9.Text;
                string nome = label8.Text;
                string per = label4.Text;


                    Ajustes form1 = new Ajustes(id, nome, per);
                    form1.ShowDialog();
                
            }
            finally
            {

            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                textBox4.Visible = true;
                textBox11.Visible = true;

            }
            else
            {
                textBox4.Visible = false;
                textBox11.Visible = false;

            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                textBox5.Visible = true;
                textBox12.Visible = true;
            }
            else
            {
                textBox5.Visible = false;
                textBox12.Visible = false;
            }
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox3.Checked)
            {
                textBox6.Visible = true;
                textBox13.Visible = true;

            }
            else
            {
                textBox6.Visible = false;
                textBox13.Visible = false;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            string query6 = "SELECT nome FROM mat where cod = '" + textBox10.Text + "';";

            MySqlConnection dbcon6 = Conexao.abrir();

            MySqlCommand commdb6 = new MySqlCommand(query6, dbcon6);

            MySqlDataReader reader6;
            reader6 = commdb6.ExecuteReader();

            reader6.Read();

            string n = reader6.GetString(0);

            Conexao.fechar();

            string query7 = "SELECT nome FROM mat where cod = '" + textBox10.Text + "';";

            MySqlConnection dbcon7 = Conexao.abrir();

            MySqlCommand commdb7 = new MySqlCommand(query7, dbcon7);

            MySqlDataReader reader7;
            reader7 = commdb7.ExecuteReader();

            reader7.Read();

            string z = reader7.GetString(0);

            Conexao.fechar();


            string query = "INSERT INTO historico(movimento,cmat,nmat,qts,iduser,user,nfo,data,idemp,emp) VALUES('Entrada de Material','" + textBox10.Text + "','" + n + "','" + textBox8.Text + "','" + label9.Text + "','" + label8.Text + "','" + textBox7.Text + "',CURDATE(),'" + textBox9.Text + "',"+z+")";
            

            MySqlConnection dbcon = Conexao.abrir();

            MySqlCommand com = new MySqlCommand(query, dbcon);

            com.ExecuteNonQuery();

            Conexao.fechar();

            string query2 = "UPDATE mat SET quantidade = quantidade+'"+textBox8.Text+"' WHERE cod = " + textBox10.Text + "";
            MySqlConnection dbcon2 = Conexao.abrir();

            MySqlCommand commdb2 = new MySqlCommand(query2, dbcon2);

            commdb2.ExecuteNonQuery();

            Conexao.fechar();
            MessageBox.Show("Peças Adicionadas!");

        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            string i = "Finalizar O.S";
            string o = "Adicionar Material";

            if (button2.Text == i)
            {
                foreach (ListViewItem item in listView3.Items)
                {
                    if (item.Checked)
                    {
                        string os = item.Text;


                        string query = "UPDATE chamados SET dtfin = CURDATE(), situacao = 'Finalizado' WHERE ordem = '" + os + "'";
                        MySqlConnection dbcon = Conexao.abrir();

                        MySqlCommand commdb = new MySqlCommand(query, dbcon);

                        commdb.ExecuteNonQuery();

                        Conexao.fechar();

                        if (checkBox1.Checked)
                        {
                            if (!string.IsNullOrWhiteSpace(textBox4.Text) && !string.IsNullOrWhiteSpace(textBox11.Text))
                            {
                                string query2 = "UPDATE mat SET quantidade = quantidade-"+textBox11.Text+" WHERE cod = '" + textBox4.Text + "'";
                                MySqlConnection dbcon2 = Conexao.abrir();

                                MySqlCommand commdb2 = new MySqlCommand(query2, dbcon2);

                                commdb2.ExecuteNonQuery();

                                Conexao.fechar();

                                string query6 = "SELECT nome FROM mat where cod = '"+textBox4.Text+"';";

                                MySqlConnection dbcon6 = Conexao.abrir();

                                MySqlCommand commdb6 = new MySqlCommand(query6, dbcon6);

                                MySqlDataReader reader6;
                                reader6 = commdb6.ExecuteReader();

                                reader6.Read();

                                string n = reader6.GetString(0);

                                Conexao.fechar();


                                string query5 = "INSERT INTO historico(movimento,cmat,nmat,qts,iduser,user,nfo,data,idemp,emp) VALUES('Material foi utilizado na O.S Nº" + os + ",'" + textBox4.Text + "','" + n + "','" + textBox11.Text + "','" + label9.Text + "','" + label8.Text + "','" + os + "',CURDATE(),'12','Waldesa')";
                                

                                MySqlConnection dbcon5 = Conexao.abrir();

                                MySqlCommand com5 = new MySqlCommand(query5, dbcon5);

                                com5.ExecuteNonQuery();

                                Conexao.fechar();
                                checkBox1.Checked = false;
                                textBox4.Text = "";
                            }
                        }

                        if (checkBox2.Checked)
                        {
                            if (!string.IsNullOrWhiteSpace(textBox5.Text) && !string.IsNullOrWhiteSpace(textBox12.Text))
                            {
                                string query2 = "UPDATE mat SET quantidade = quantidade-" + textBox12.Text + " WHERE cod = '" + textBox5.Text + "'";
                                MySqlConnection dbcon2 = Conexao.abrir();

                                MySqlCommand commdb2 = new MySqlCommand(query2, dbcon2);

                                commdb2.ExecuteNonQuery();

                                Conexao.fechar();

                                string query6 = "SELECT nome FROM mat where cod = '" + textBox5.Text + "';";

                                MySqlConnection dbcon6 = Conexao.abrir();

                                MySqlCommand commdb6 = new MySqlCommand(query6, dbcon6);

                                MySqlDataReader reader6;
                                reader6 = commdb6.ExecuteReader();

                                reader6.Read();

                                string n = reader6.GetString(0);

                                Conexao.fechar();


                                string query5 = "INSERT INTO historico(movimento,cmat,nmat,qts,iduser,user,nfo,data,idemp,emp) VALUES('Material foi utilizado na O.S Nº" + os + ",'" + textBox4.Text + "','" + n + "','" + textBox11.Text + "','" + label9.Text + "','" + label8.Text + "','" + os + "',CURDATE(),'12','Waldesa')";



                                MySqlConnection dbcon5 = Conexao.abrir();

                                MySqlCommand com5 = new MySqlCommand(query5, dbcon5);

                                com5.ExecuteNonQuery();

                                Conexao.fechar();

                                checkBox2.Checked = false;
                                textBox5.Text = "";
                            }
                        }

                        if (checkBox3.Checked)
                        {
                            if (!string.IsNullOrWhiteSpace(textBox6.Text) && !string.IsNullOrWhiteSpace(textBox13.Text))
                            {
                                string query2 = "UPDATE mat SET quantidade = quantidade-" + textBox13.Text + " WHERE cod = '" + textBox6.Text + "'";
                                MySqlConnection dbcon2 = Conexao.abrir();

                                MySqlCommand commdb2 = new MySqlCommand(query2, dbcon2);

                                commdb2.ExecuteNonQuery();

                                Conexao.fechar();
                                string query6 = "SELECT nome FROM mat where cod = '" + textBox6.Text + "';";

                                MySqlConnection dbcon6 = Conexao.abrir();

                                MySqlCommand commdb6 = new MySqlCommand(query6, dbcon6);

                                MySqlDataReader reader6;
                                reader6 = commdb6.ExecuteReader();

                                reader6.Read();

                                string n = reader6.GetString(0);

                                Conexao.fechar();


                                string query5 = "INSERT INTO historico(movimento,cmat,nmat,qts,iduser,user,nfo,data,idemp,emp) VALUES('Material foi utilizado na O.S Nº" + os + ",'" + textBox4.Text + "','" + n + "','" + textBox11.Text + "','" + label9.Text + "','" + label8.Text + "','" + os + "',CURDATE(),'12','Waldesa')";

                                MySqlConnection dbcon5 = Conexao.abrir();

                                MySqlCommand com5 = new MySqlCommand(query5, dbcon5);

                                com5.ExecuteNonQuery();

                                Conexao.fechar();


                                checkBox3.Checked = false;
                                textBox6.Text = "";
                            }
                        }
                        item.Checked = false;
                        MessageBox.Show("O.S Finalizada!");
                    }
                    else if(item.Checked = false)
                    {
                        MessageBox.Show("Você não selecionou nenhum O.S");
                    }
                }
            }
            else if (button2.Text == o)
            {
                string query = "INSERT INTO mat(nome,quantidade,modelo,preco) VALUES('" + textBox4.Text + "','0','" + textBox5.Text + "','"+textBox6.Text+"')";

                MySqlConnection dbcon = Conexao.abrir();

                MySqlCommand com = new MySqlCommand(query, dbcon);

                com.ExecuteNonQuery();

                Conexao.fechar();

                MessageBox.Show("Material Adicionado com Sucesso!");

                textBox4.Clear();
                textBox5.Clear();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            string um = "Adicionar O.S";
            string dois = "Pesquisar";

            if (button1.Text == um)
            {
                string query = "INSERT INTO chamados(fazer,dtsol,pego,situacao,idsol) VALUES('" + textBox2.Text + "', CURDATE(),'','Esperando Atendente', '" + textBox3.Text + "')";

                MySqlConnection dbcon = Conexao.abrir();

                MySqlCommand com = new MySqlCommand(query, dbcon);

                com.ExecuteNonQuery();

                if (com.LastInsertedId != 0)
                    com.Parameters.Add(new MySqlParameter("ultimoId", com.LastInsertedId));

                string y = Convert.ToString(com.Parameters["@ultimoId"].Value);

                Conexao.fechar();

                string query6 = "SELECT Nome FROM login where ID = '" + textBox3.Text + "';";

                MySqlConnection dbcon6 = Conexao.abrir();

                MySqlCommand commdb6 = new MySqlCommand(query6, dbcon6);

                MySqlDataReader reader6;
                reader6 = commdb6.ExecuteReader();

                reader6.Read();

                string n = reader6.GetString(0);

                Conexao.fechar();

                string query2 = "UPDATE chamados SET solicitante = '" + n + "' WHERE Ordem = '" + y + "'";
                MySqlConnection dbcon2 = Conexao.abrir();

                MySqlCommand commdb2 = new MySqlCommand(query2, dbcon2);

                commdb2.ExecuteNonQuery();

                Conexao.fechar();

                MessageBox.Show("O.S Criada!");

                textBox3.Clear();
                textBox2.Clear();


            }
            else if (button1.Text == dois)
            {

            }
        }

        private void saveFileDialog1_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
