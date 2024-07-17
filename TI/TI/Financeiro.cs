using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WCS
{
    public partial class Financeiro : Form
    {
        public Financeiro(string id,string nome)
        {
            InitializeComponent();
            label1.Text = label1.Text + id;
            label2.Text = label2.Text + nome;
        }
    }
}
