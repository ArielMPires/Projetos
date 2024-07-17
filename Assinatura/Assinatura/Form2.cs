using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using iTextSharp;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.IO;
using Rectangle = iTextSharp.text.Rectangle;
using Image = iTextSharp.text.Image;
using Font = iTextSharp.text.Font;

namespace Assinatura
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form form1 = new Form1();
            this.Hide();
            form1.ShowDialog();
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //define as propriedades do controle 
            //OpenFileDialog
            this.opf2.Multiselect = true;
            this.opf2.Title = "Selecionar PDF";
            opf2.InitialDirectory = @"C:\";
            //filtra para exibir somente arquivos de imagens
            opf2.Filter = "pdf files (*.pdf)|*.pdf|All files (*.*)|*.*";
            opf2.CheckFileExists = true;
            opf2.CheckPathExists = true;
            opf2.FilterIndex = 2;
            opf2.RestoreDirectory = true;
            opf2.ReadOnlyChecked = true;
            opf2.ShowReadOnly = true;

            DialogResult dr = this.opf2.ShowDialog();

            if (dr == System.Windows.Forms.DialogResult.OK)
            {
                foreach (String arquivo in opf2.FileNames)
                {
                    textBox1.Text += arquivo;
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

        private void button1_Click(object sender, EventArgs e)
        {
            sfd2.Title = "Salvar PDF";
            sfd2.Filter = "pdf files (*.pdf)|*.pdf|All files (*.*)|*.*";
            sfd2.FilterIndex = 0;
            sfd2.FileName = "";
            sfd2.DefaultExt = ".pdf";
            sfd2.InitialDirectory = @"c:\";
            sfd2.RestoreDirectory = true;

            DialogResult result = sfd2.ShowDialog();
            if (result == DialogResult.OK)
            {
                foreach (string newfile in sfd2.FileNames)
                {

                    string C = "C97#/57*";
                    string J = "J05/0#3*";
                    string P = "P85#/3*";
                    string JE = "JE54/#11";
                    string FE = "FE74/*16";
                    String chave = textBox2.Text;

                    if (chave == P)
                    {
                        DateTime thisDay = DateTime.Today;
                        string oldfile = @textBox1.Text;

                        PdfReader reader = new PdfReader(oldfile);
                        Rectangle size = reader.GetPageSizeWithRotation(1);
                        Document document = new Document(size);
                        FileStream fs = new FileStream(newfile, FileMode.Create, FileAccess.Write);
                        PdfWriter writer = PdfWriter.GetInstance(document, fs);
                        document.Open();

                        PdfContentByte cb = writer.DirectContentUnder;

                        BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                        cb.SetColorFill(BaseColor.BLACK);
                        cb.SetFontAndSize(bf, 16);
                        cb.BeginText();

                        string text2 = thisDay.ToString("d");
                        Paragraph paragrafo3 = new Paragraph(text2, new Font(bf, 16f, 0));
                        document.Add(paragrafo3);
                        cb.EndText();
                        if (apr2.Checked)
                        {
                            cb.BeginText();

                            Paragraph paragrafo = new Paragraph("Aprovado", new Font(bf, 16f, 0));
                            document.Add(paragrafo);

                            cb.EndText();
                        }
                        else if (rep2.Checked)
                        {
                            cb.BeginText();

                            Paragraph paragrafo = new Paragraph("Reprovado", new Font(bf, 16f, 0));
                            document.Add(paragrafo);
                            cb.EndText();
                        }
                        else
                        {
                            MessageBox.Show("Você Não Selecionou nenhuma Opção");
                            return;
                        }
                        cb.BeginText();

                        string text = textBox3.Text;
                        Paragraph paragrafo2 = new Paragraph(text, new Font(bf, 16f, 0));
                        document.Add(paragrafo2);

                        cb.EndText();

                        Image imagem1 = Image.GetInstance(@"Resources\AssPedro.png");
                        imagem1.ScalePercent(16f);

                        imagem1.SetAbsolutePosition(document.PageSize.Width - 310f - 315f,
                              document.PageSize.Height - 110f - 100f);
                        document.Add(imagem1);

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
                        writer.Close();
                        fs.Close();
                        reader.Close();
                        MessageBox.Show("Assinatura Concluida Com Sucesso");
                        textBox1.Clear();
                        textBox2.Clear();
                        textBox3.Clear();
                        apr2.CheckState = CheckState.Unchecked;
                        rep2.CheckState = CheckState.Unchecked;
                    }
                    else if (chave == C)
                    {
                        DateTime thisDay = DateTime.Today;
                        string oldfile = @textBox1.Text;

                        PdfReader reader = new PdfReader(oldfile);
                        Rectangle size = reader.GetPageSizeWithRotation(1);
                        Document document = new Document(size);

                        FileStream fs = new FileStream(newfile, FileMode.Create, FileAccess.Write);
                        PdfWriter writer = PdfWriter.GetInstance(document, fs);
                        document.Open();

                        PdfContentByte cb = writer.DirectContentUnder;

                        BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                        cb.SetColorFill(BaseColor.BLACK);
                        cb.SetFontAndSize(bf, 16);
                        cb.BeginText();

                        string text2 = thisDay.ToString("d");
                        Paragraph paragrafo3 = new Paragraph(text2, new Font(bf, 16f, 0));
                        document.Add(paragrafo3);
                        cb.EndText();
                        if (apr2.Checked)
                        {
                            cb.BeginText();

                            Paragraph paragrafo = new Paragraph("Aprovado", new Font(bf, 16f, 0));
                            document.Add(paragrafo);

                            cb.EndText();
                        }
                        else if (rep2.Checked)
                        {
                            cb.BeginText();

                            Paragraph paragrafo = new Paragraph("Reprovado", new Font(bf, 16f, 0));
                            document.Add(paragrafo);
                            cb.EndText();
                        }
                        else
                        {
                            MessageBox.Show("Você Não Selecionou nenhuma Opção");
                            return;
                        }
                        cb.BeginText();

                        string text = textBox3.Text;
                        Paragraph paragrafo2 = new Paragraph(text, new Font(bf, 16f, 0));
                        document.Add(paragrafo2);

                        cb.EndText();

                        Image imagem1 = Image.GetInstance(@"Resources\AssCris.png");
                        imagem1.ScalePercent(16f);

                        imagem1.SetAbsolutePosition(document.PageSize.Width - 310f - 310f,
                              document.PageSize.Height - 110f - 100f);
                        document.Add(imagem1);


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
                        MessageBox.Show("Assinatura Concluida Com Sucesso");
                        textBox1.Clear();
                        textBox2.Clear();
                        textBox3.Clear();
                        apr2.CheckState = CheckState.Unchecked;
                        rep2.CheckState = CheckState.Unchecked;
                    }
                    else if (chave == J)
                    {
                        DateTime thisDay = DateTime.Today;
                        string oldfile = @textBox1.Text;

                        PdfReader reader = new PdfReader(oldfile);
                        Rectangle size = reader.GetPageSizeWithRotation(1);
                        Document document = new Document(size);

                        FileStream fs = new FileStream(newfile, FileMode.Create, FileAccess.Write);
                        PdfWriter writer = PdfWriter.GetInstance(document, fs);
                        document.Open();

                        PdfContentByte cb = writer.DirectContentUnder;

                        BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                        cb.SetColorFill(BaseColor.BLACK);
                        cb.SetFontAndSize(bf, 16);
                        cb.BeginText();

                        string text2 = thisDay.ToString("d");
                        Paragraph paragrafo3 = new Paragraph(text2, new Font(bf, 16f, 0));
                        document.Add(paragrafo3);
                        cb.EndText();
                        if (apr2.Checked)
                        {
                            cb.BeginText();

                            Paragraph paragrafo = new Paragraph("Aprovado", new Font(bf, 16f, 0));
                            document.Add(paragrafo);

                            cb.EndText();
                        }
                        else if (rep2.Checked)
                        {
                            cb.BeginText();

                            Paragraph paragrafo = new Paragraph("Reprovado", new Font(bf, 16f, 0));
                            document.Add(paragrafo);
                            cb.EndText();
                        }
                        else
                        {
                            MessageBox.Show("Você Não Selecionou nenhuma Opção");
                            return;
                        }
                        cb.BeginText();

                        string text = textBox3.Text;
                        Paragraph paragrafo2 = new Paragraph(text, new Font(bf, 16f, 0));
                        document.Add(paragrafo2);

                        cb.EndText();

                        Image imagem1 = Image.GetInstance(@"Resources\AssJoce.png");
                        imagem1.ScalePercent(16f);

                        imagem1.SetAbsolutePosition(document.PageSize.Width - 310f - 315f,
                              document.PageSize.Height - 110f - 100f);
                        document.Add(imagem1);


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
                        MessageBox.Show("Assinatura Concluida Com Sucesso");
                        textBox1.Clear();
                        textBox2.Clear();
                        textBox3.Clear();
                        apr2.CheckState = CheckState.Unchecked;
                        rep2.CheckState = CheckState.Unchecked;
                    }
                    else if (chave == JE)
                    {
                        DateTime thisDay = DateTime.Today;
                        string oldfile = @textBox1.Text;

                        PdfReader reader = new PdfReader(oldfile);
                        Rectangle size = reader.GetPageSizeWithRotation(1);
                        Document document = new Document(size);

                        FileStream fs = new FileStream(newfile, FileMode.Create, FileAccess.Write);
                        PdfWriter writer = PdfWriter.GetInstance(document, fs);
                        document.Open();

                        PdfContentByte cb = writer.DirectContentUnder;

                        BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                        cb.SetColorFill(BaseColor.BLACK);
                        cb.SetFontAndSize(bf, 16);
                        cb.BeginText();

                        string text2 = thisDay.ToString("d");
                        Paragraph paragrafo3 = new Paragraph(text2, new Font(bf, 16f, 0));
                        document.Add(paragrafo3);
                        cb.EndText();
                        if (apr2.Checked)
                        {
                            cb.BeginText();

                            Paragraph paragrafo = new Paragraph("Aprovado", new Font(bf, 16f, 0));
                            document.Add(paragrafo);

                            cb.EndText();
                        }
                        else if (rep2.Checked)
                        {
                            cb.BeginText();

                            Paragraph paragrafo = new Paragraph("Reprovado", new Font(bf, 16f, 0));
                            document.Add(paragrafo);
                            cb.EndText();
                        }
                        else
                        {
                            MessageBox.Show("Você Não Selecionou nenhuma Opção");
                            return;
                        }
                        cb.BeginText();

                        string text = textBox3.Text;
                        Paragraph paragrafo2 = new Paragraph(text, new Font(bf, 16f, 0));
                        document.Add(paragrafo2);

                        cb.EndText();

                        Image imagem1 = Image.GetInstance(@"Resources\AssJe.png");
                        imagem1.ScalePercent(16f);

                        imagem1.SetAbsolutePosition(document.PageSize.Width - 280f - 270f,
                              document.PageSize.Height - 50f - 90f);
                        document.Add(imagem1);


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
                        MessageBox.Show("Assinatura Concluida Com Sucesso");
                        textBox1.Clear();
                        textBox2.Clear();
                        textBox3.Clear();
                        apr2.CheckState = CheckState.Unchecked;
                        rep2.CheckState = CheckState.Unchecked;
                    }
                    else if (chave == FE)
                    {
                        DateTime thisDay = DateTime.Today;
                        string oldfile = @textBox1.Text;

                        PdfReader reader = new PdfReader(oldfile);
                        Rectangle size = reader.GetPageSizeWithRotation(1);
                        Document document = new Document(size);

                        FileStream fs = new FileStream(newfile, FileMode.Create, FileAccess.Write);
                        PdfWriter writer = PdfWriter.GetInstance(document, fs);
                        document.Open();

                        PdfContentByte cb = writer.DirectContentUnder;

                        BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                        cb.SetColorFill(BaseColor.BLACK);
                        cb.SetFontAndSize(bf, 16);
                        cb.BeginText();

                        string text2 = thisDay.ToString("d");
                        Paragraph paragrafo3 = new Paragraph(text2, new Font(bf, 16f, 0));
                        document.Add(paragrafo3);
                        cb.EndText();
                        if (apr2.Checked)
                        {
                            cb.BeginText();

                            Paragraph paragrafo = new Paragraph("Aprovado", new Font(bf, 16f, 0));
                            document.Add(paragrafo);

                            cb.EndText();
                        }
                        else if (rep2.Checked)
                        {
                            cb.BeginText();

                            Paragraph paragrafo = new Paragraph("Reprovado", new Font(bf, 16f, 0));
                            document.Add(paragrafo);
                            cb.EndText();
                        }
                        else
                        {
                            MessageBox.Show("Você Não Selecionou nenhuma Opção");
                            return;
                        }
                        cb.BeginText();

                        string text = textBox3.Text;
                        Paragraph paragrafo2 = new Paragraph(text, new Font(bf, 16f, 0));
                        document.Add(paragrafo2);

                        cb.EndText();

                        Image imagem1 = Image.GetInstance(@"Resources\AssFe.png");
                        imagem1.ScalePercent(16f);

                        imagem1.SetAbsolutePosition(document.PageSize.Width - 240f - 240f,
                              document.PageSize.Height - 50f - 90f);
                        document.Add(imagem1);


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
                        MessageBox.Show("Assinatura Concluida Com Sucesso");
                        textBox1.Clear();
                        textBox2.Clear();
                        textBox3.Clear();
                        apr2.CheckState = CheckState.Unchecked;
                        rep2.CheckState = CheckState.Unchecked;
                    }
                    else
                    {
                        MessageBox.Show("Você não tem Autorização para Assinar esse documento");
                    }
                }
            }
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (apr2.Checked)
            {
                rep2.CheckState = CheckState.Unchecked;
            }
        }

        private void apr2_CheckedChanged(object sender, EventArgs e)
        {
            if (rep2.Checked)
            {
                apr2.CheckState = CheckState.Unchecked;
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
