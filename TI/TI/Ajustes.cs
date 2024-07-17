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
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TI;
using Font = iTextSharp.text.Font;

namespace WCS
{
    public partial class Ajustes : Form
    {
        public Ajustes(string id, string nome, string per)
        {
            InitializeComponent();
            label4.Text = id;
            label3.Text = nome;
            label5.Text = per;

            dateTimePicker1.Value = DateTime.Now.AddDays(1);
            dateTimePicker2.Value = DateTime.Now.AddDays(1);
        }

        private void button2_Click(object sender, EventArgs e)
        {

            try
            {

                if (!string.IsNullOrWhiteSpace(textBox1.Text))
                {
                    string query = "INSERT INTO empresas(NFan,cnf,Ncon,ag,banco,conta,nconta,vendedor) VALUES('" + textBox1.Text + "', '" + textBox3.Text + "', '" + textBox2.Text + "', '" + textBox5.Text + "', '" + textBox4.Text + "', '" + comboBox1.Text + "', '" + textBox6.Text + "', '"+textBox7.Text+"')";

                    MySqlConnection dbcon = Conexao.abrir();

                    MySqlCommand com = new MySqlCommand(query, dbcon);

                    com.ExecuteNonQuery();

                    MessageBox.Show("Cadastro Completo!");

                    textBox1.Clear();
                    textBox2.Clear();
                    textBox3.Clear();
                    textBox4.Clear();
                    textBox5.Clear();
                    textBox6.Clear();
                    textBox7.Clear();

                }
                else { MessageBox.Show("Voce não preencheu corretamente o formulario!"); }

            }
            finally
            {
                Conexao.fechar();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {


            string query6 = "SELECT COUNT(*) Ordem FROM chamados WHERE dtsol between '"+ dateTimePicker1.Value.ToString("yyyy-MM-dd") + "' and '"+dateTimePicker2.Value.ToString("yyyy-MM-dd")+"';";

            MySqlConnection dbcon6 = Conexao.abrir();

            MySqlCommand commdb6 = new MySqlCommand(query6, dbcon6);

            MySqlDataReader reader6;
            reader6 = commdb6.ExecuteReader();

            reader6.Read();

            int n = reader6.GetInt32(0);

            Conexao.fechar();

            string newfile = @"Relatorio\Relatorio - " + dateTimePicker1.Value.ToString("MMMM - yyyy") + ".pdf";
            string relatorio = "Relatorio -" + dateTimePicker1.Value.ToString(" MMMM - yyyy");

            Document doc = new Document(iTextSharp.text.PageSize.LETTER, 12, 12, 45, 35);
            PdfWriter wri = PdfWriter.GetInstance(doc, new FileStream(newfile, FileMode.Create));

            doc.Open();

            PdfContentByte cb = wri.DirectContentUnder;

            BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            cb.SetColorFill(BaseColor.BLACK);
            cb.SetFontAndSize(bf, 8);


            //Define a logo no Início da página
            var imagemDoTopo = iTextSharp.text.Image.GetInstance(@"Ref\WCSlogo.png");
            imagemDoTopo.SetAbsolutePosition(50, 700);
            imagemDoTopo.ScalePercent(26f);
            doc.Add(imagemDoTopo);
            doc.Add(new Paragraph(new Chunk("\n\n")));
            doc.Add(new Paragraph(new Chunk("                           Relatório completo de custos e serviços", new Font(bf, 18f, 0))));
            doc.Add(new Paragraph(new Chunk("\n")));
            doc.Add(new Paragraph(new Chunk("Temos um total de "+ n +" de serviços solicitados e resolvidos.", new Font(bf, 9f, 0))));
            //doc.Add(new Paragraph(new Chunk("Segue abaixo a quantidade de solitação por Funcionario:", new Font(bf, 9f, 0))));
            //doc.Add(new Paragraph(new Chunk("\n\n")));

            PdfPTable table;

            float[] widths = new float[] { 25f, 25f };
            float[] widths2 = new float[] { 30f, 50f, 30f, 40f };
            /*
            table.AddCell(new Phrase("FUNCIONARIOS", new Font(bf, 9f, 0)));
            table.AddCell(new Phrase("QUANTIDADE DE CHAMADOS", new Font(bf, 9f, 0)));

            doc.Add(table);

            table.DeleteRow(4);

            string query64 = "SELECT COUNT(*) ID FROM login;";

            MySqlConnection dbcon64 = Conexao.abrir();

            MySqlCommand commdb64 = new MySqlCommand(query64, dbcon64);

            MySqlDataReader reader64;
            reader64 = commdb64.ExecuteReader();

            reader64.Read();

            int y = reader64.GetInt32(0);

            Conexao.fechar();

            for (int x = 1; x < y ;x++)
            {
                string query = "SELECT COUNT(*) Ordem FROM chamados WHERE idsol = '"+x+"' and dtsol between '" + dateTimePicker1.Value.ToString("yyyy-MM-dd") + "' and '" + dateTimePicker2.Value.ToString("yyyy-MM-dd") + "';";

                MySqlConnection dbcon = Conexao.abrir();

                MySqlCommand commdb = new MySqlCommand(query, dbcon);

                MySqlDataReader reader;
                reader = commdb.ExecuteReader();

                reader.Read();

                int z = reader.GetInt32(0);

                reader.Close();

                Conexao.fechar();

                string no = login(x);


                if (z != 0)
                {
                    table = new PdfPTable(2);

                    table.SetWidths(widths);

                    table.AddCell(new Phrase(no, new Font(bf, 9f, 0)));
                    table.AddCell(new Phrase(Convert.ToString(z), new Font(bf, 9f, 0)));
                    doc.Add(table);

                    table.DeleteRow(2);
                }
            }

            table = new PdfPTable(2);

            table.SetWidths(widths);

            table.AddCell(new Phrase("Total", new Font(bf, 9f, 0)));
            table.AddCell(new Phrase(Convert.ToString(n), new Font(bf, 9f, 0)));
            doc.Add(table);

            table.DeleteRow(2);
           */

            string query7 = "SELECT cmat,nmat,qts,data FROM historico WHERE movimento = 'Entrada de Material' and data between '" + dateTimePicker1.Value.ToString("yyyy-MM-dd") + "' and '" + dateTimePicker2.Value.ToString("yyyy-MM-dd") + "';";

            MySqlConnection dbcon7 = Conexao.abrir();

            MySqlCommand commdb7 = new MySqlCommand(query7, dbcon7);

            MySqlDataReader reader7;
            reader7 = commdb7.ExecuteReader();

            




            //doc.Add(new Paragraph(new Chunk("\n")));
            //doc.Add(new Paragraph(new Chunk("\n")));
            doc.Add(new Paragraph(new Chunk("Segue abaixo os custos de compra com material novo para manutenção dos computadores e perifericos da waldesa:", new Font(bf, 9f, 0))));
            doc.Add(new Paragraph(new Chunk("\n")));
            doc.Add(new Paragraph(new Chunk("\n")));

            table = new PdfPTable(4);

            table.SetWidths(widths2);

            table.AddCell(new Phrase("Material", new Font(bf, 9f, 0)));
            table.AddCell(new Phrase("Quantidade", new Font(bf, 9f, 0)));
            table.AddCell(new Phrase("Preço", new Font(bf, 9f, 0)));
            table.AddCell(new Phrase("Data", new Font(bf, 9f, 0)));
            doc.Add(table);

            table.DeleteRow(4);

            while (reader7.Read())
            {
                string query78 = "SELECT preco FROM mat WHERE cod = '"+reader7.GetInt32(0)+"' ;";

                MySqlConnection dbcon78 = Conexao.abrir();

                MySqlCommand commdb78 = new MySqlCommand(query78, dbcon78);

                MySqlDataReader reader78;
                reader78 = commdb78.ExecuteReader();

                reader78.Read();

                table = new PdfPTable(4);

                table.SetWidths(widths2);

                table.AddCell(new Phrase(reader7.GetString(1), new Font(bf, 9f, 0)));
                table.AddCell(new Phrase(Convert.ToString(reader7.GetInt32(2)), new Font(bf, 9f, 0)));
                table.AddCell(new Phrase("R$ "+ Convert.ToString(reader78.GetFloat(0)), new Font(bf, 9f, 0)));
                table.AddCell(new Phrase(Convert.ToString(reader7.GetDateTime(3)), new Font(bf, 9f, 0)));
                doc.Add(table);

                table.DeleteRow(4);


                float t = reader78.GetFloat(0) * reader7.GetInt32(2);

                Conexao.fechar();

                calc(t);

            }
            Conexao.fechar();


            table = new PdfPTable(2);

            table.SetWidths(widths);

            table.AddCell(new Phrase("Total", new Font(bf, 9f, 0)));
            table.AddCell(new Phrase("R$ " + Math.Round(total, 2).ToString(), new Font(bf, 9f, 0)));
            doc.Add(table);

            table.DeleteRow(2);

            /*
            string query76 = "SELECT nmat,qts,nfo,data FROM historico WHERE idemp = '12' and data between '" + dateTimePicker1.Value.ToString("yyyy-MM-dd") + "' and '" + dateTimePicker2.Value.ToString("yyyy-MM-dd") + "';";

            MySqlConnection dbcon76 = Conexao.abrir();

            MySqlCommand commdb76 = new MySqlCommand(query76, dbcon76);

            MySqlDataReader reader76;
            reader76 = commdb76.ExecuteReader();

            doc.Add(new Paragraph(new Chunk("\n")));
            doc.Add(new Paragraph(new Chunk("\n")));
            doc.Add(new Paragraph(new Chunk("Segue abaixo materiais usados:.", new Font(bf, 9f, 0))));
            doc.Add(new Paragraph(new Chunk("\n")));
            doc.Add(new Paragraph(new Chunk("\n")));


            table = new PdfPTable(4);

            table.SetWidths(widths2);

            table.AddCell(new Phrase("Material", new Font(bf, 9f, 0)));
            table.AddCell(new Phrase("Quantidade", new Font(bf, 9f, 0)));
            table.AddCell(new Phrase("Funcionario", new Font(bf, 9f, 0)));
            table.AddCell(new Phrase("Data", new Font(bf, 9f, 0)));
            doc.Add(table);

            table.DeleteRow(4);

            while (reader76.Read())
            {
                string query78 = "SELECT solicitante FROM chamados WHERE Ordem = '" + reader76.GetInt32(2) + "' ;";

                MySqlConnection dbcon78 = Conexao.abrir();

                MySqlCommand commdb78 = new MySqlCommand(query78, dbcon78);

                MySqlDataReader reader78;
                reader78 = commdb78.ExecuteReader();

                reader78.Read();

                table = new PdfPTable(4);

                table.SetWidths(widths2);

                table.AddCell(new Phrase(reader76.GetString(0), new Font(bf, 9f, 0)));
                table.AddCell(new Phrase(Convert.ToString(reader76.GetInt32(1)), new Font(bf, 9f, 0)));
                table.AddCell(new Phrase(reader78.GetString(0), new Font(bf, 9f, 0)));
                table.AddCell(new Phrase(Convert.ToString(reader76.GetDateTime(3)), new Font(bf, 9f, 0)));
                doc.Add(table);

                table.DeleteRow(4);



                Conexao.fechar();

            }
            Conexao.fechar();


            */

            doc.Close();


            ConexaoFTP.EnviarArquivoFTPR(newfile, relatorio);

            try{

                string query = "INSERT INTO relatorio(mes,data) VALUES('" + dateTimePicker1.Value.ToString("MMMM - yyyy") + "',CURDATE())";


                MySqlConnection dbcon = Conexao.abrir();

                MySqlCommand com = new MySqlCommand(query, dbcon);

                com.ExecuteNonQuery();

            }catch (Exception ex)
            {
                MessageBox.Show("Relatorio desse mês foi substituido!");
            }
            Conexao.fechar();

            MessageBox.Show("Relátorio de "+ dateTimePicker1.Value.ToString("MMMM")+" foi gerado!");

            


        }
        private string login(int x)
        {
            string query = "SELECT nome FROM login where ID = '" + x + "';";

            MySqlConnection dbcon = Conexao.abrir();

            MySqlCommand commdb = new MySqlCommand(query, dbcon);

            MySqlDataReader data;
            data = commdb.ExecuteReader();
            string z;
            data.Read();
            if (data.Read())
            {
                z = data.GetString(0);
            }
            else
            {
                z = null;
            }
            Conexao.fechar();

            return z;
        }

        private float total;

        public void calc(float t)
        {
            total = total + t;
        }
    }
}
