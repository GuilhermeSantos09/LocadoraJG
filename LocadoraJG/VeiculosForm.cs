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
    public partial class VeiculosForm : Form
    {
        DataTable veiculos;
        public VeiculosForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dataGridView1.Visible = true;
            veiculos = new Banco().BuscarCarro(Carro.PK + " NOT IN (select " + Emprestimo.FKCARRO + " from " + Banco.EMPRESTIMO + " where " + Emprestimo.DATA_FINAL + " >= convert('" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + "',date))");
            dataGridView1.DataSource = veiculos;
            dataGridView1.Refresh();
        }
    }
}
