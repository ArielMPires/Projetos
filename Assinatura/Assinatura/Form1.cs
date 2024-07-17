using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Rectangle = iTextSharp.text.Rectangle;
using System.IO;
using System.Drawing.Imaging;
using Image = iTextSharp.text.Image;

namespace Assinatura
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

            sfd1.Title = "Salvar PDF";
            sfd1.Filter = "pdf files (*.pdf)|*.pdf|All files (*.*)|*.*";
            sfd1.FilterIndex = 0;
            sfd1.FileName = "";
            sfd1.DefaultExt = ".pdf";
            sfd1.InitialDirectory = @"c:\";
            sfd1.RestoreDirectory = true;

            DialogResult result = sfd1.ShowDialog();
            if (result == DialogResult.OK)
            {
                foreach (string newfile in sfd1.FileNames)
                {



                    String chave = textBox2.Text;
                    string c = "C97#/57*";
                    string j = "J05/0#3*";
                    string p = "P85#/3*";

                    if (chave == c)
                    {


                        DateTime thisDay = DateTime.Today;
                        string oldfile = @textBox3.Text;

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
                        cb.BeginText();

                        string text2 = thisDay.ToString("d");


                        cb.ShowTextAligned(1, text2, 150, 117, 0);
                        cb.EndText();
                        if (Apr.Checked)
                        {
                            cb.BeginText();

                            string text3 = "X";


                            cb.ShowTextAligned(1, text3, 246, 147, 0);
                            cb.EndText();
                        }
                        else if (Rep.Checked)
                        {
                            cb.BeginText();

                            string text3 = "X";


                            cb.ShowTextAligned(1, text3, 387, 147, 0);
                            cb.EndText();
                        }
                        else
                        {
                            MessageBox.Show("Você Não Selecionou nenhuma Opção");
                            return;
                        }
                        cb.BeginText();

                        string text = textBox1.Text;


                        cb.ShowTextAligned(1, text, 210, 90, 0);
                        cb.EndText();


                        Image imagem1 = Image.GetInstance(@"Resources\AssCris.png");
                        imagem1.ScalePercent(16f);

                        imagem1.SetAbsolutePosition(document.PageSize.Width - 300f - 100f,
                              document.PageSize.Height - 420f - 420f);
                        document.Add(imagem1);

                        PdfImportedPage page = writer.GetImportedPage(reader, 1);
                        cb.AddTemplate(page, 0, 0);

                        document.Close();
                        fs.Close();
                        writer.Close();
                        reader.Close();
                        MessageBox.Show("Assinatura Concluida Com Sucesso");
                        textBox1.Clear();
                        textBox2.Clear();
                        textBox3.Clear();
                        Apr.CheckState = CheckState.Unchecked;
                        Rep.CheckState = CheckState.Unchecked;


                    }
                    else if (chave == p)
                    {
                        DateTime thisDay = DateTime.Today;
                        string oldfile = @textBox3.Text;

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
                        cb.BeginText();

                        string text2 = thisDay.ToString("d");


                        cb.ShowTextAligned(1, text2, 150, 117, 0);
                        cb.EndText();
                        if (Apr.Checked)
                        {
                            cb.BeginText();

                            string text3 = "X";


                            cb.ShowTextAligned(1, text3, 246, 147, 0);
                            cb.EndText();
                        }
                        else if (Rep.Checked)
                        {
                            cb.BeginText();

                            string text3 = "X";


                            cb.ShowTextAligned(1, text3, 387, 147, 0);
                            cb.EndText();
                        }
                        else
                        {
                            MessageBox.Show("Você Não Selecionou nenhuma Opção");
                            return;
                        }

                        cb.BeginText();

                        string text = textBox1.Text;


                        cb.ShowTextAligned(1, text, 210, 90, 0);
                        cb.EndText();


                        Image imagem1 = Image.GetInstance(@"Resources\AssPedro.png");
                        imagem1.ScalePercent(16f);

                        imagem1.SetAbsolutePosition(document.PageSize.Width - 300f - 100f,
                              document.PageSize.Height - 420f - 420f);
                        document.Add(imagem1);

                        PdfImportedPage page = writer.GetImportedPage(reader, 1);
                        cb.AddTemplate(page, 0, 0);

                        document.Close();
                        fs.Close();
                        writer.Close();
                        reader.Close();
                        MessageBox.Show("Assinatura Concluida Com Sucesso");
                        textBox1.Clear();
                        textBox2.Clear();
                        textBox3.Clear();
                        Apr.CheckState = CheckState.Unchecked;
                        Rep.CheckState = CheckState.Unchecked;

                    }
                    else if (chave == j)
                    {
                        DateTime thisDay = DateTime.Today;
                        string oldfile = @textBox3.Text;

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
                        cb.BeginText();

                        string text2 = thisDay.ToString("d");


                        cb.ShowTextAligned(1, text2, 150, 117, 0);
                        cb.EndText();
                        if (Apr.Checked)
                        {
                            cb.BeginText();

                            string text3 = "X";


                            cb.ShowTextAligned(1, text3, 246, 147, 0);
                            cb.EndText();
                        }
                        else if (Rep.Checked)
                        {
                            cb.BeginText();

                            string text3 = "X";


                            cb.ShowTextAligned(1, text3, 387, 147, 0);
                            cb.EndText();
                        }
                        else
                        {
                            MessageBox.Show("Você Não Selecionou nenhuma Opção");
                            return;
                        }

                        cb.BeginText();

                        string text = textBox1.Text;


                        cb.ShowTextAligned(1, text, 210, 90, 0);
                        cb.EndText();


                        Image imagem1 = Image.GetInstance(@"Resources\AssJoce.png");
                        imagem1.ScalePercent(12f);

                        imagem1.SetAbsolutePosition(document.PageSize.Width - 300f - 100f,
                              document.PageSize.Height - 420f - 420f);
                        document.Add(imagem1);

                        PdfImportedPage page = writer.GetImportedPage(reader, 1);
                        cb.AddTemplate(page, 0, 0);

                        document.Close();
                        fs.Close();
                        writer.Close();
                        reader.Close();
                        MessageBox.Show("Assinatura Concluida Com Sucesso");
                        textBox1.Clear();
                        textBox2.Clear();
                        textBox3.Clear();
                        Apr.CheckState = CheckState.Unchecked;
                        Rep.CheckState = CheckState.Unchecked;

                    }
                    else
                    {
                        MessageBox.Show("Você não tem Autorização para Assinar esse documento");
                    }
                }
            }
        }

        private void folderBrowserDialog1_HelpRequest(object sender, EventArgs e)
        {

        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
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
                    textBox3.Text += arquivo;
                    try
                    {
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

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void Apr_CheckedChanged(object sender, EventArgs e)
        {
            if (Rep.Checked)
            {
                Apr.CheckState = CheckState.Unchecked;
            }
        }

        private void Rep_CheckedChanged(object sender, EventArgs e)
        {
            if (Apr.Checked)
            {
                Rep.CheckState = CheckState.Unchecked;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form2 form = new Form2();
            this.Hide();
            form.ShowDialog();
            this.Close();


        }
    }
    }

