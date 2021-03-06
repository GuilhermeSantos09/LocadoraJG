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
    public partial class Form1 : Form
    {
        Carro carroSelecionado;
        public Form1()
        {
            InitializeComponent();
            carroSelecionado = null;
            Banco banco = new Banco();
            dataGridView1.DataSource = banco.BuscarCarro(null);
            dataGridView1.Refresh();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            new Form2().Show();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
           
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (carroSelecionado!=null) {
                Form2 editarCarro = new Form2();
                Banco banco = new Banco();
                Carro carro = banco.PegarCarro(carroSelecionado.GetPK());
                editarCarro.mudarParaEditar(carro);
                editarCarro.Show(); 
            }
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (carroSelecionado != null)
            {
                Banco banco = new Banco();
                DialogResult diagResult = MessageBox.Show("Deletar Carro","Você tem certeza que deseja deletar o carro de pk: "+pk.ToString()+" ?",MessageBoxButtons.YesNo);
                if (diagResult == DialogResult.Yes)
                {
                    if (banco.DeletarCarro(carroSelecionado.GetPK()))
                        MessageBox.Show("Registro deletado");
                    else
                        MessageBox.Show("ERRO, registro não deletado");
                }
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int pk = int.Parse(dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());
            Banco banco = new Banco();
            carroSelecionado = banco.PegarCarro(pk);
            txtId.Text = carroSelecionado.GetPK().ToString();
            txtMarca.Text = carroSelecionado.marca;
            txtModelo.Text = carroSelecionado.modelo;
            txtPlaca.Text = carroSelecionado.marca;
            textBox10.Text = "R$"+carroSelecionado.valor.ToString();

        }

        private void textBox10_TextChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            carroSelecionado = null;
            Banco banco = new Banco();
            dataGridView1.DataSource = banco.BuscarCarro(null);
            dataGridView1.Refresh();
        }
    }
}
