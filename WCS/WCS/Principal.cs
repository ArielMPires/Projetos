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
using Rectangle = iTextSharp.text.Rectangle;

namespace WCS
{
    public partial class Principal : Form
    {

        private DataSet da;
        private MySqlDataAdapter myd;
        public Principal()
        {
            InitializeComponent();

            menu.Panel2Collapsed = true;

            nome.Text = Info.Nome;
            id.Text = Info.Id;
            nome1.Text = Info.Nome;
            id1.Text = Info.Id;
            voltar.Visible = false;
            button1.Visible = false;
            button2.Visible = false;
            button3.Visible = false;
            button4.Visible = false;
            button5.Visible = false;
            textBox1.Visible = false;
            textBox2.Visible = false;
            textBox3.Visible = false;
            padrao.Visible = false;
            pesquisa.Visible = false;
            listView1.Visible = false;
            dataGridView1.Visible = false;
            label5.Visible = false;
            label6.Visible = false;
            label7.Visible = false;
            label8.Visible = false;
        }

        private void est1_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(Info.Pere) >= 1) {
                est1.BackColor = Color.Brown;
                fin1.BackColor = Color.FromArgb(64, 64, 64);
                ti1.BackColor = Color.FromArgb(64, 64, 64);
                conf1.BackColor = Color.FromArgb(64, 64, 64);

                menu.Panel2Collapsed = false;
                menu.Panel1Collapsed = true;
                voltar.Visible = true;

                button1.Visible = true;
                button1.Text = "Pedidos";
                button2.Visible = true;
                button2.Text = "Planilha";
                button3.Visible = false;
                button4.Visible = false;
                button5.Visible = false;
                textBox1.Visible = false;
                textBox2.Visible = false;
                textBox3.Visible = false;
                padrao.Visible = false;
                pesquisa.Visible = false;
                listView1.Visible = false;
                dataGridView1.Visible = false;
                label5.Visible = false;
                label6.Visible = false;
                label7.Visible = false;
                label8.Visible = false;
            }
            else
            {
                MessageBox.Show("Você Não tem Permissão para acessar esta Parte!");
            }
        }

        private void fin1_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(Info.Perf) >= 1)
            {
                est1.BackColor = Color.FromArgb(64, 64, 64);
                fin1.BackColor = Color.Brown;
                ti1.BackColor = Color.FromArgb(64, 64, 64);
                conf1.BackColor = Color.FromArgb(64, 64, 64);

                menu.Panel2Collapsed = false;
                menu.Panel1Collapsed = true;
                voltar.Visible = true;
                button1.Visible = true;
                button1.Text = "Adicionar Nota-Fiscal";
                button2.Visible = true;
                button2.Text = "Confirmar Retira";
                button3.Visible = false;
                button4.Visible = false;
                button5.Visible = false;
                textBox1.Visible = false;
                textBox2.Visible = false;
                textBox3.Visible = false;
                padrao.Visible = false;
                pesquisa.Visible = false;
                listView1.Visible = false;
                dataGridView1.Visible = false;
                label5.Visible = false;
                label6.Visible = false;
                label7.Visible = false;
                label8.Visible = false;
            }
            else
            {
                MessageBox.Show("Você Não tem Permissão para acessar esta Parte!");
            }
        }

        private void ti1_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(Info.Pert) >= 1)
            {
                est1.BackColor = Color.FromArgb(64, 64, 64);
                fin1.BackColor = Color.FromArgb(64, 64, 64);
                ti1.BackColor = Color.FromArgb(64, 64, 64);
                conf1.BackColor = Color.FromArgb(64, 64, 64);

                menu.Panel2Collapsed = false;
                menu.Panel1Collapsed = true;
                voltar.Visible = true;
                button1.Visible = false;
                button2.Visible = false;
                button3.Visible = false;
                button4.Visible = false;
                button5.Visible = false;
                textBox1.Visible = false;
                textBox2.Visible = false;
                textBox3.Visible = false;
                padrao.Visible = false;
                pesquisa.Visible = false;
                listView1.Visible = false;
                dataGridView1.Visible = false;
                label5.Visible = false;
                label6.Visible = false;
                label7.Visible = false;
                label8.Visible = false;
            }
            else
            {
                MessageBox.Show("Você Não tem Permissão para acessar esta Parte!");
            }
        }

        private void conf1_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(Info.Perc) >= 1)
            {
                est1.BackColor = Color.FromArgb(64, 64, 64);
                fin1.BackColor = Color.FromArgb(64, 64, 64);
                ti1.BackColor = Color.FromArgb(64, 64, 64);
                conf1.BackColor = Color.FromArgb(64, 64, 64);

                menu.Panel2Collapsed = false;
                menu.Panel1Collapsed = true;
                voltar.Visible = true;
                button1.Visible = true;
                if (Convert.ToInt32(Info.Perc) > 1) {
                    button1.Visible = true;
                    button1.Text = "Novo Usuário";
                }
                else
                {
                    button1.Visible = false;
                }
                button2.Visible = true;
                button2.Text = "Modificar Usuário";
                button3.Visible = false;
                button4.Visible = false;
                button5.Visible = false;
                textBox1.Visible = false;
                textBox2.Visible = false;
                textBox3.Visible = false;
                padrao.Visible = false;
                pesquisa.Visible = false;
                listView1.Visible = false;
                dataGridView1.Visible = false;
                label5.Visible = false;
                label6.Visible = false;
                label7.Visible = false;
                label8.Visible = false;
            }
            else
            {
                MessageBox.Show("Você Não tem Permissão para acessar esta Parte!");
            }

        }

        private void voltar_Click(object sender, EventArgs e)
        {
            menu.Panel2Collapsed = true;
            menu.Panel1Collapsed = false;
            voltar.Visible = false;

            est1.BackColor = Color.FromArgb(64, 64, 64);
            fin1.BackColor = Color.FromArgb(64, 64, 64);
            ti1.BackColor = Color.FromArgb(64, 64, 64);
            conf1.BackColor = Color.FromArgb(64, 64, 64);

            button1.BackColor = Color.Brown;
            button2.BackColor = Color.Brown;
            button3.BackColor = Color.Brown;
            button4.BackColor = Color.Brown;
            button5.BackColor = Color.Brown;

            button1.Visible = false;
            button2.Visible = false;
            button3.Visible = false;
            button4.Visible = false;
            button5.Visible = false;
            textBox1.Visible = false;
            textBox2.Visible = false;
            textBox3.Visible = false;
            padrao.Visible = false;
            pesquisa.Visible = false;
            dataGridView1.Visible = false;
            listView1.Visible = false;
            label5.Visible = false;
            label6.Visible = false;
            label7.Visible = false;
            label8.Visible = false;
            listView1.Clear();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            switch (button1.Text)
            {
                case "Pedidos":
                    button1.Visible = true;
                    button1.Text = "Adicionar Pedidos";
                    button2.Visible = true;
                    button2.Text = "Pedidos P/ Separar";
                    button3.Visible = true;
                    button3.Text = "Pedidos P/ Conferir";
                    button4.Visible = true;
                    button4.Text = "Situações dos pedidos";
                    break;
                case "Adicionar Pedidos":
                    if (Convert.ToInt32(Info.Pere) >= 2)
                    {
                        button1.BackColor = Color.FromArgb(64, 64, 64);
                        button2.BackColor = Color.Brown;
                        button3.BackColor = Color.Brown;
                        button4.BackColor = Color.Brown;
                        button5.BackColor = Color.Brown;

                        textBox1.Visible = true;
                        textBox2.Visible = true;
                        textBox3.Visible = true;
                        textBox4.Visible = false;
                        padrao.Visible = true;
                        padrao.Text = "Adicionar Pedido";
                        pesquisa.Visible = false;

                        dataGridView1.Visible = false;
                        listView1.Visible = true;
                        label5.Visible = true;
                        label5.Text = "Nº do Pedido";
                        label6.Visible = true;
                        label6.Text = "Razão social:";
                        label7.Visible = true;
                        label7.Text = "Vendedor";
                        label8.Visible = false;


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



                            for (int i = 0; i <= 7; i++)
                            {
                                if (data.IsDBNull(i))
                                {
                                    VeDb.dbnull(i);

                                }
                                else
                                {
                                    switch (i)
                                    {
                                        case 0:
                                            VeDb.c0 = data.GetString(0);
                                            break;
                                        case 1:
                                            VeDb.c1 = data.GetString(1);
                                            break;
                                        case 2:
                                            VeDb.c2 = data.GetString(2);
                                            break;
                                        case 3:
                                            VeDb.c3 = data.GetString(3);
                                            break;
                                        case 4:
                                            VeDb.c4 = data.GetString(4);
                                            break;
                                        case 5:
                                            VeDb.c5 = data.GetString(5);
                                            break;
                                        case 6:
                                            VeDb.c6 = Convert.ToString(data.GetDateTime(6));
                                            break;
                                        case 7:
                                            VeDb.c7 = data.GetString(7);
                                            break;
                                        case 8:
                                            VeDb.c8 = string.Empty;
                                            break;
                                        case 9:
                                            VeDb.c9 = string.Empty;
                                            break;
                                        case 10:
                                            VeDb.c10 = string.Empty;
                                            break;
                                    }
                                }

                            }


                            string[] row = {
                                VeDb.c0,
                               VeDb.c1,
                                VeDb.c2,
                                VeDb.c3,
                                VeDb.c4,
                                VeDb.c5,
                                VeDb.c6,
                                VeDb.c7
                };

                            var listitem = new ListViewItem(row);

                            listView1.Items.Add(listitem);

                        }
                        Conexao.fechar();
                    }
                    else
                    {
                        MessageBox.Show("Você Não tem Permissão para acessar esta Parte!");
                    }
                    break;
                case "Novo Usuário":
                    if (Convert.ToInt32(Info.Perc) >= 2)
                    {
                        button1.BackColor = Color.FromArgb(64, 64, 64);
                        button2.BackColor = Color.Brown;
                        button3.BackColor = Color.Brown;
                        button4.BackColor = Color.Brown;
                        button5.BackColor = Color.Brown;

                        dataGridView1.Visible = false;
                        listView1.Visible = false;
                        textBox1.Visible = true;
                        textBox2.Visible = true;
                        textBox3.Visible = true;
                        textBox4.Visible = false;
                        pesquisa.Visible = false;
                        label5.Visible = true;
                        label6.Visible = true;
                        label7.Visible = true;
                        label8.Visible = false;
                        padrao.Visible = true;
                    }
                    break;
            }
        }

        private void padrao_Click(object sender, EventArgs e)
        {
            switch (padrao.Text)
            {
                case "Adicionar Pedido":
                    string Pedido = textBox1.Text;
                    string nome = textBox2.Text;

                    string query = "INSERT INTO pedidos(pedido,razao,vendedor,data,situacao) VALUES('" + textBox1.Text + "', '" + textBox2.Text + "', '" + textBox3.Text + "' ,NOW(),'Aguardando Separação')";

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
                    break;
                case "Pesquisar":
                    if (Convert.ToInt32(Info.Pere) >= 2)
                    {
                        listView1.Clear();

                        listView1.View = View.Details;
                        listView1.AllowColumnReorder = true;
                        listView1.FullRowSelect = true;
                        listView1.GridLines = true;

                        listView1.Columns.Add("Nº Pedido:", 80, HorizontalAlignment.Left);
                        listView1.Columns.Add("Razão Social", 130, HorizontalAlignment.Left);
                        listView1.Columns.Add("Vendedor:", 80, HorizontalAlignment.Left);
                        listView1.Columns.Add("Volume:", 80, HorizontalAlignment.Left);
                        listView1.Columns.Add("Data:", 70, HorizontalAlignment.Left);
                        listView1.Columns.Add("Situação:", 70, HorizontalAlignment.Left);


                        string query2 = "SELECT pedido,razao,vendedor,vol,data,situacao,nf FROM pedidos where pedido = '" + pesquisa.Text + "' or razao = '" + pesquisa.Text + "'";

                        MySqlConnection dbcon2 = Conexao.abrir();

                        MySqlCommand commdb2 = new MySqlCommand(query2, dbcon2);

                        MySqlDataReader data = commdb2.ExecuteReader();

                        listView1.Items.Clear();

                        while (data.Read())
                        {



                            for (int i = 0; i <= 6; i++)
                            {
                                if (data.IsDBNull(i))
                                {
                                    VeDb.dbnull(i);

                                }
                                else
                                {
                                    switch (i)
                                    {
                                        case 0:
                                            VeDb.c0 = data.GetString(0);
                                            break;
                                        case 1:
                                            VeDb.c1 = data.GetString(1);
                                            break;
                                        case 2:
                                            VeDb.c2 = data.GetString(2);
                                            break;
                                        case 3:
                                            VeDb.c3 = data.GetString(3);
                                            break;
                                        case 4:
                                            VeDb.c4 = Convert.ToString(data.GetDateTime(4));
                                            break;
                                        case 5:
                                            VeDb.c5 = data.GetString(5);
                                            break;
                                        case 6:
                                            VeDb.c6 = data.GetString(6);
                                            break;
                                        case 7:
                                            VeDb.c7 = string.Empty;
                                            break;
                                        case 8:
                                            VeDb.c8 = string.Empty;
                                            break;
                                        case 9:
                                            VeDb.c9 = string.Empty;
                                            break;
                                        case 10:
                                            VeDb.c10 = string.Empty;
                                            break;
                                    }
                                }

                            }


                            string[] row = {
                                VeDb.c0,
                               VeDb.c1,
                                VeDb.c2,
                                VeDb.c3,
                                VeDb.c4,
                                VeDb.c5,
                                VeDb.c6
                };

                            var listitem = new ListViewItem(row);

                            listView1.Items.Add(listitem);

                        }
                        Conexao.fechar();
                    }
                    else
                    {
                        MessageBox.Show("Você Não tem Permissão para acessar esta Parte!");
                    }
                    break;
                case "Atualizar":
                    string query3 = "SELECT * FROM login";

                    MySqlConnection dbcon3 = Conexao.abrir();

                    MySqlCommand cmd = new MySqlCommand(query3, dbcon3);

                    myd = new MySqlDataAdapter(cmd);
                    myd.SelectCommand = cmd;

                    MySqlCommandBuilder cmm = new MySqlCommandBuilder(myd);
                    myd.UpdateCommand = cmm.GetUpdateCommand();

                    bindingSource1.EndEdit();

                    myd.Update(da);




                    break;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            switch (button2.Text)
            {
                case "Pedidos P/ Separar":
                    if (Convert.ToInt32(Info.Pere) >= 1)
                    {
                        button1.BackColor = Color.Brown;
                        button2.BackColor = Color.FromArgb(64, 64, 64);
                        button3.BackColor = Color.Brown;
                        button4.BackColor = Color.Brown;
                        button5.BackColor = Color.Brown;

                        dataGridView1.Visible = false;
                        listView1.Visible = true;
                        textBox1.Visible = false;
                        textBox2.Visible = false;
                        textBox3.Visible = false;
                        textBox4.Visible = false;
                        pesquisa.Visible = false;
                        label5.Visible = false;
                        label6.Visible = false;
                        label7.Visible = false;
                        label8.Visible = false;
                        padrao.Visible = false;

                        listView1.Clear();

                        listView1.View = View.Details;
                        listView1.AllowColumnReorder = true;
                        listView1.FullRowSelect = true;
                        listView1.GridLines = true;

                        listView1.Columns.Clear();

                        listView1.Columns.Add("Nº Pedido:", 80, HorizontalAlignment.Left);
                        listView1.Columns.Add("Razão Social", 130, HorizontalAlignment.Left);
                        listView1.Columns.Add("Vendedor:", 80, HorizontalAlignment.Left);
                        listView1.Columns.Add("Separado Por:", 100, HorizontalAlignment.Left);
                        listView1.Columns.Add("Conferido Por:", 100, HorizontalAlignment.Left);
                        listView1.Columns.Add("Volume:", 80, HorizontalAlignment.Left);
                        listView1.Columns.Add("Data:", 70, HorizontalAlignment.Left);
                        listView1.Columns.Add("Observações:", 130, HorizontalAlignment.Left);


                        string query = "SELECT * FROM pedidos where Separado = '' or separado is null";

                        MySqlConnection dbcon = Conexao.abrir();

                        MySqlCommand commdb = new MySqlCommand(query, dbcon);

                        MySqlDataReader data = commdb.ExecuteReader();

                        listView1.Items.Clear();

                        while (data.Read())
                        {



                            for (int i = 0; i <= 7; i++)
                            {
                                if (data.IsDBNull(i))
                                {
                                    VeDb.dbnull(i);

                                }
                                else
                                {
                                    switch (i)
                                    {
                                        case 0:
                                            VeDb.c0 = data.GetString(0);
                                            break;
                                        case 1:
                                            VeDb.c1 = data.GetString(1);
                                            break;
                                        case 2:
                                            VeDb.c2 = data.GetString(2);
                                            break;
                                        case 3:
                                            VeDb.c3 = data.GetString(3);
                                            break;
                                        case 4:
                                            VeDb.c4 = data.GetString(4);
                                            break;
                                        case 5:
                                            VeDb.c5 = data.GetString(5);
                                            break;
                                        case 6:
                                            VeDb.c6 = Convert.ToString(data.GetDateTime(6));
                                            break;
                                        case 7:
                                            VeDb.c7 = data.GetString(7);
                                            break;
                                        case 8:
                                            VeDb.c8 = string.Empty;
                                            break;
                                        case 9:
                                            VeDb.c9 = string.Empty;
                                            break;
                                        case 10:
                                            VeDb.c10 = string.Empty;
                                            break;
                                    }
                                }

                            }


                            string[] row = {
                                VeDb.c0,
                               VeDb.c1,
                                VeDb.c2,
                                VeDb.c3,
                                VeDb.c4,
                                VeDb.c5,
                                VeDb.c6,
                                VeDb.c7
                };

                            var listitem = new ListViewItem(row);

                            listView1.Items.Add(listitem);

                        }
                        Conexao.fechar();
                    }
                    else
                    {
                        MessageBox.Show("Você Não tem Permissão para acessar esta Parte!");
                    }
                    break;
                case "Modificar Usuário":
                    if (Convert.ToInt32(Info.Perc) >= 1)
                    {
                        button1.BackColor = Color.Brown;
                        button2.BackColor = Color.FromArgb(64, 64, 64);
                        button3.BackColor = Color.Brown;
                        button4.BackColor = Color.Brown;
                        button5.BackColor = Color.Brown;

                        dataGridView1.Visible = true;
                        listView1.Visible = false;
                        textBox1.Visible = false;
                        textBox2.Visible = false;
                        textBox3.Visible = false;
                        textBox4.Visible = false;
                        pesquisa.Visible = false;
                        label5.Visible = false;
                        label6.Visible = false;
                        label7.Visible = false;
                        label8.Visible = false;
                        padrao.Visible = true;
                        padrao.Text = "Atualizar";

                        string query3 = "SELECT * FROM login";

                        MySqlConnection dbcon3 = Conexao.abrir();

                        MySqlCommand cmd = new MySqlCommand(query3, dbcon3);

                        myd = new MySqlDataAdapter(cmd);
                        myd.SelectCommand = cmd;

                         da = new DataSet("Pedidos");

                        myd.Fill(da);

                        bindingSource1.DataSource = da.Tables[0];
                        Conexao.fechar();


                        dataGridView1.AutoGenerateColumns = true;

                        dataGridView1.DataSource = bindingSource1;



                        /*MySqlDataReader dr = cmd.ExecuteReader();
                        cmd.CommandType = CommandType.Text;

                        int nColunas = dr.FieldCount;

                        for (int i = 0; i < nColunas; i++)

                        {
                            dataGridView1.Columns.Add(dr.GetName(i).ToString(), dr.GetName(i).ToString());

                        }

                        string[] linhaDados = new string[nColunas];

                        while (dr.Read())

                        {
                            for (int a = 0; a < nColunas; a++)

                            {

                                if (dr.GetFieldType(a).ToString() == "System.Int32")

                                {

                                    linhaDados[a] = dr.GetInt32(a).ToString();

                                }

                                if (dr.GetFieldType(a).ToString() == "System.String")

                                {

                                    linhaDados[a] = dr.GetString(a).ToString();

                                }

                                if (dr.GetFieldType(a).ToString() == "System.DateTime")

                                {

                                    linhaDados[a] = dr.GetDateTime(a).ToString();

                                }

                            }

                            dataGridView1.Rows.Add(linhaDados);
                        }*/
                    }
                        break;
                    
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            switch (button3.Text)
            {
                case "Pedidos P/ Conferir":
                    if (Convert.ToInt32(Info.Pere) >= 2)
                    {
                        button1.BackColor = Color.Brown;
                        button2.BackColor = Color.Brown;
                        button3.BackColor = Color.FromArgb(64, 64, 64);
                        button4.BackColor = Color.Brown;
                        button5.BackColor = Color.Brown;

                        listView1.Visible = true;
                        textBox1.Visible = false;
                        textBox2.Visible = false;
                        textBox3.Visible = false;
                        textBox4.Visible = false;
                        pesquisa.Visible = false;
                        label5.Visible = false;
                        label6.Visible = false;
                        label7.Visible = false;
                        label8.Visible = false;
                        padrao.Visible = false;

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


                        string query = "SELECT * FROM pedidos where conferido = '' or conferido is null";

                        MySqlConnection dbcon = Conexao.abrir();

                        MySqlCommand commdb = new MySqlCommand(query, dbcon);

                        MySqlDataReader data = commdb.ExecuteReader();

                        listView1.Items.Clear();

                        while (data.Read())
                        {



                            for (int i = 0; i <= 7; i++)
                            {
                                if (data.IsDBNull(i))
                                {
                                    VeDb.dbnull(i);

                                }
                                else
                                {
                                    switch (i)
                                    {
                                        case 0:
                                            VeDb.c0 = data.GetString(0);
                                            break;
                                        case 1:
                                            VeDb.c1 = data.GetString(1);
                                            break;
                                        case 2:
                                            VeDb.c2 = data.GetString(2);
                                            break;
                                        case 3:
                                            VeDb.c3 = data.GetString(3);
                                            break;
                                        case 4:
                                            VeDb.c4 = data.GetString(4);
                                            break;
                                        case 5:
                                            VeDb.c5 = data.GetString(5);
                                            break;
                                        case 6:
                                            VeDb.c6 = Convert.ToString(data.GetDateTime(6));
                                            break;
                                        case 7:
                                            VeDb.c7 = data.GetString(7);
                                            break;
                                        case 8:
                                            VeDb.c8 = string.Empty;
                                            break;
                                        case 9:
                                            VeDb.c9 = string.Empty;
                                            break;
                                        case 10:
                                            VeDb.c10 = string.Empty;
                                            break;
                                    }
                                }

                            }


                            string[] row = {
                                VeDb.c0,
                               VeDb.c1,
                                VeDb.c2,
                                VeDb.c3,
                                VeDb.c4,
                                VeDb.c5,
                                VeDb.c6,
                                VeDb.c7
                };

                            var listitem = new ListViewItem(row);

                            listView1.Items.Add(listitem);

                        }
                        Conexao.fechar();
                    }
                    else
                    {
                        MessageBox.Show("Você Não tem Permissão para acessar esta Parte!");
                    }
                    break;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            switch (button4.Text)
            {
                case "Situações dos pedidos":
                    if (Convert.ToInt32(Info.Pere) >= 2)
                    {
                        button1.BackColor = Color.Brown;
                        button2.BackColor = Color.Brown;
                        button3.BackColor = Color.Brown;
                        button4.BackColor = Color.FromArgb(64, 64, 64);
                        button5.BackColor = Color.Brown;

                        listView1.Visible = true;
                        textBox1.Visible = false;
                        textBox2.Visible = false;
                        textBox3.Visible = false;
                        textBox4.Visible = false;
                        pesquisa.Visible = true;
                        label5.Visible = false;
                        label6.Visible = false;
                        label7.Visible = false;
                        label8.Visible = true;
                        label8.Text = "Digite abaixo Nº do pedido ou Razão Social";
                        padrao.Visible = true;
                        padrao.Text = "Pesquisar";

                        listView1.Clear();

                        listView1.View = View.Details;
                        listView1.AllowColumnReorder = true;
                        listView1.FullRowSelect = true;
                        listView1.GridLines = true;

                        listView1.Columns.Add("Nº Pedido:", 80, HorizontalAlignment.Left);
                        listView1.Columns.Add("Razão Social", 130, HorizontalAlignment.Left);
                        listView1.Columns.Add("Vendedor:", 80, HorizontalAlignment.Left);
                        listView1.Columns.Add("Volume:", 80, HorizontalAlignment.Left);
                        listView1.Columns.Add("Data:", 70, HorizontalAlignment.Left);
                        listView1.Columns.Add("Situação:", 70, HorizontalAlignment.Left);


                        string query = "SELECT pedido,razao,vendedor,vol,data,situacao,nf FROM pedidos";

                        MySqlConnection dbcon = Conexao.abrir();

                        MySqlCommand commdb = new MySqlCommand(query, dbcon);

                        MySqlDataReader data = commdb.ExecuteReader();

                        listView1.Items.Clear();

                        while (data.Read())
                        {



                            for (int i = 0; i <= 6; i++)
                            {
                                if (data.IsDBNull(i))
                                {
                                    VeDb.dbnull(i);

                                }
                                else
                                {
                                    switch (i)
                                    {
                                        case 0:
                                            VeDb.c0 = data.GetString(0);
                                            break;
                                        case 1:
                                            VeDb.c1 = data.GetString(1);
                                            break;
                                        case 2:
                                            VeDb.c2 = data.GetString(2);
                                            break;
                                        case 3:
                                            VeDb.c3 = data.GetString(3);
                                            break;
                                        case 4:
                                            VeDb.c4 = Convert.ToString(data.GetDateTime(4));
                                            break;
                                        case 5:
                                            VeDb.c5 = data.GetString(5);
                                            break;
                                        case 6:
                                            VeDb.c6 = data.GetString(6);
                                            break;
                                        case 7:
                                            VeDb.c7 = string.Empty;
                                            break;
                                        case 8:
                                            VeDb.c8 = string.Empty;
                                            break;
                                        case 9:
                                            VeDb.c9 = string.Empty;
                                            break;
                                        case 10:
                                            VeDb.c10 = string.Empty;
                                            break;
                                    }
                                }

                            }


                            string[] row = {
                                VeDb.c0,
                               VeDb.c1,
                                VeDb.c2,
                                VeDb.c3,
                                VeDb.c4,
                                VeDb.c5,
                                VeDb.c6
                };

                            var listitem = new ListViewItem(row);

                            listView1.Items.Add(listitem);

                        }
                        Conexao.fechar();
                    }
                    else
                    {
                        MessageBox.Show("Você Não tem Permissão para acessar esta Parte!");
                    }
                    break;
            }
        }


    }
}
