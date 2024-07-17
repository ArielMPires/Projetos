using MySqlConnector;
using System.Data;

namespace WCS_Compras
{
    public partial class Form1 : Form
    {
        private DataSet da;
        private MySqlDataAdapter myd;

        public Form1()
        {
            InitializeComponent();
            dateTimePicker1.Value = DateTime.Now.AddDays(0);

            string query3 = "SELECT * FROM pedidos WHERE Data = '"+dateTimePicker1.Value.ToString("yyyy-MM-dd")+"'";

            MySqlConnection dbcon3 = Conexaoc.abrir();

            MySqlCommand cmd = new MySqlCommand(query3, dbcon3);

            myd = new MySqlDataAdapter(cmd);
            myd.SelectCommand = cmd;

            da = new DataSet("Pedidos");

            myd.Fill(da);

            bindingSource1.DataSource = da.Tables[0];
            Conexaoc.fechar();


            dataGridView1.AutoGenerateColumns = true;

            dataGridView1.DataSource = bindingSource1;

            dataGridView1.CellFormatting +=
    new System.Windows.Forms.DataGridViewCellFormattingEventHandler(
    this.dataGridView1_CellFormatting);

        }

        private void button3_Click(object sender, EventArgs e)
        {

            da.Clear();
            string query3 = "SELECT * FROM pedidos WHERE Data = '" + dateTimePicker1.Value.ToString("yyyy-MM-dd") + "'";

            MySqlConnection dbcon3 = Conexaoc.abrir();

            MySqlCommand cmd = new MySqlCommand(query3, dbcon3);

            myd = new MySqlDataAdapter(cmd);
            myd.SelectCommand = cmd;

            da = new DataSet("Pedidos");

            myd.Fill(da);

            bindingSource1.DataSource = da.Tables[0];
            Conexaoc.fechar();


            dataGridView1.AutoGenerateColumns = true;

            dataGridView1.DataSource = bindingSource1;

            dataGridView1.CellFormatting +=
new System.Windows.Forms.DataGridViewCellFormattingEventHandler(
this.dataGridView1_CellFormatting);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            da.Clear();
            string query3 = "SELECT * FROM pedidos WHERE Data = '" + dateTimePicker1.Value.ToString("yyyy-MM-dd") + "'";

            MySqlConnection dbcon3 = Conexaoc.abrir();

            MySqlCommand cmd = new MySqlCommand(query3, dbcon3);

            myd = new MySqlDataAdapter(cmd);
            myd.SelectCommand = cmd;

            da = new DataSet("Pedidos");

            myd.Fill(da);

            bindingSource1.DataSource = da.Tables[0];
            Conexaoc.fechar();


            dataGridView1.AutoGenerateColumns = true;

            dataGridView1.DataSource = bindingSource1;

            dataGridView1.CellFormatting +=
new System.Windows.Forms.DataGridViewCellFormattingEventHandler(
this.dataGridView1_CellFormatting);
        }

        private void button1_Click(object sender, EventArgs e)
        {

            MySqlCommandBuilder cmm = new MySqlCommandBuilder(myd);

            da.GetChanges();

            if (da.GetChanges(DataRowState.Modified) != null)
            {
                myd.UpdateCommand = cmm.GetUpdateCommand();

                bindingSource1.EndEdit();

                myd.Update(da);
            }
            if (da.GetChanges(DataRowState.Added) != null) {

                myd.InsertCommand = cmm.GetInsertCommand();

                bindingSource1.EndEdit();

                myd.Update(da);


            }
            if (da.GetChanges(DataRowState.Deleted) != null)
            {
                groupBox1.Visible = true;
                textBox1.Visible = true;
                textBox2.Visible = true;
                label1.Visible = true;
                label2.Visible = true;
                button4.Visible = true;

            }

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
        }

        private void button4_Click(object sender, EventArgs e)
        {
            groupBox1.Visible = false;
            textBox1.Visible = false;
            textBox2.Visible = false;
            label1.Visible = false;
            label2.Visible = false;
            button4.Visible = false;

            if (!string.IsNullOrEmpty(textBox1.Text))
            {
                if (!string.IsNullOrEmpty(textBox1.Text))
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
                        string query1 = "SELECT Nome,permissao,Estoque,Financeiro,TI,Conf FROM login where ID ='" + id + "';";

                        MySqlConnection dbcon1 = Conexao.abrir();

                        MySqlCommand commdb1 = new MySqlCommand(query1, dbcon1);

                        MySqlDataReader reader1;
                        reader1 = commdb1.ExecuteReader();
                        reader1.Read();

                        string per = Convert.ToString(reader1.GetInt32(1));

                        if (Convert.ToInt32(per) >= 2)
                        {
                            MySqlCommandBuilder cmm = new MySqlCommandBuilder(myd);

                            myd.DeleteCommand = cmm.GetDeleteCommand();

                             bindingSource1.EndEdit();

                             myd.Update(da);

                            textBox1.Clear();
                            textBox2.Clear();
                        }
                        else
                        {
                            MessageBox.Show("Voce não tem autorização!");
                            textBox1.Clear();
                            textBox2.Clear();
                        }
                    }
                    else if (!res)
                    {
                        MessageBox.Show("Senha ou Usuario Estão errados!");
                        textBox1.Clear();
                        textBox2.Clear();
                    }
                }
                else
                {
                    MessageBox.Show("Não foi digitada a senha");

                    groupBox1.Visible = true;
                    textBox1.Visible = true;
                    textBox2.Visible = true;
                    label1.Visible = true;
                    label2.Visible = true;
                    button4.Visible = true;
                    textBox1.Clear();
                    textBox2.Clear();
                }
            }
            else
            {
                MessageBox.Show("Prencha o Id");

                groupBox1.Visible = true;
                textBox1.Visible = true;
                textBox2.Visible = true;
                label1.Visible = true;
                label2.Visible = true;
                button4.Visible = true;
                textBox1.Clear();
                textBox2.Clear();
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {
        }

        private void dataGridView1_CellFormatting(object sender ,DataGridViewCellFormattingEventArgs e)
        {

            if (dataGridView1.Columns[e.ColumnIndex].Name.Equals("Situação"))
            {
                switch(e.Value){
                    case "Entregue":
                        dataGridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightGreen;

                        break;
                    case "Parcial":
                        dataGridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Yellow;
                        break;
                    case "entregue":
                        dataGridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightGreen;

                        break;
                    case "parcial":
                        dataGridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Yellow;
                        break;
                    case "cancelado":
                        dataGridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Red;
                        break;
                    case "Cancelado":
                        dataGridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Red;
                        break;
                    case null:
                        return;
                        break;
                }

                        


            }
        }

        private void button5_Click(object sender, EventArgs e)
        {


            string query3 = "SELECT * FROM pedidos where Ordem_de_Venda = '" + textBox3.Text + "' or Número_do_Pedido = '" + textBox3.Text + "'";

            MySqlConnection dbcon3 = Conexaoc.abrir();

            MySqlCommand cmd = new MySqlCommand(query3, dbcon3);

            myd = new MySqlDataAdapter(cmd);
            myd.SelectCommand = cmd;

            da = new DataSet("Pedidos");

            myd.Fill(da);

            bindingSource1.DataSource = da.Tables[0];
            Conexaoc.fechar();


            dataGridView1.AutoGenerateColumns = true;

            dataGridView1.DataSource = bindingSource1;

            dataGridView1.CellFormatting +=
    new System.Windows.Forms.DataGridViewCellFormattingEventHandler(
    this.dataGridView1_CellFormatting);

        }
    }
}