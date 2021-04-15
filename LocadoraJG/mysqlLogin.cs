using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LocadoraJG
{
    public partial class mysqlLogin : Form
    {
        public mysqlLogin()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Banco.user = textBox1.Text;
            Banco.password = textBox2.Text;
            if (!Banco.TestarConexao())
            {
                MessageBox.Show("Conexão inválida");
                return;
            }
            if (!Banco.DBexitste())
            {
                MessageBox.Show("O banco de dados será instanciado");
                Banco.Inicializar();
                MessageBox.Show("O banco de dados provavelmente foi instanciado");
            }

            new Form1().Show();
            this.Hide();
        }
    }
}
