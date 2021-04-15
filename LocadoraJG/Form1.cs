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
        DataTable carros;
        Carro carroSelecionado;
        public Form1()
        {
            InitializeComponent();
            carroSelecionado = null;
            Banco banco = new Banco();
             carros = banco.BuscarCarro(null);
            dataGridView1.DataSource = carros;
            dataGridView1.Refresh();
        }

        private void button1_Click(object sender, EventArgs e)//novocarro
        {
            new Form2().Show();
        }

        private void button6_Click(object sender, EventArgs e)//editar carro
        {
            if (carroSelecionado!=null) {
                Form2 editarCarro = new Form2();//reutilizo da tela de registro
                Banco banco = new Banco();
                Carro carro = banco.PegarCarro(carroSelecionado.GetPK());
                editarCarro.mudarParaEditar(carro);
                editarCarro.Show(); 
            }
        }

        private void button5_Click(object sender, EventArgs e)//deletar carro
        {
            if (carroSelecionado != null)
            {
                Banco banco = new Banco();
                DialogResult diagResult = MessageBox.Show("Você tem certeza que deseja deletar o carro de pk: "+carroSelecionado.GetPK().ToString()+" ?", "Deletar Carro", MessageBoxButtons.YesNo);
                if (diagResult == DialogResult.Yes)
                {
                    if (banco.DeletarCarro(carroSelecionado.GetPK()))
                        MessageBox.Show("Registro deletado");
                    else
                        MessageBox.Show("ERRO, registro não deletado");
                }
            }
        }

        private void button3_Click_1(object sender, EventArgs e)//refresh
        {
            carros = null;
            carroSelecionado = null;
            Banco banco = new Banco();
            carros = banco.BuscarCarro(null);
            dataGridView1.DataSource = carros;
            dataGridView1.Refresh();
        }

        private void dataGridView1_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)//selecionar carro
        {
            try
            {
                Banco banco = new Banco();
                carroSelecionado =banco.PegarCarro((int)dataGridView1.Rows[e.RowIndex].Cells[0].Value);
                txtId.Text = carroSelecionado.GetPK().ToString();
                txtAno.Text = carroSelecionado.ano.ToString();
                txtMarca.Text = carroSelecionado.marca;
                txtModelo.Text = carroSelecionado.modelo;
                txtPlaca.Text = carroSelecionado.placa;
                textBox10.Text = "R$" + carroSelecionado.valor.ToString();
                //selecionar emprestimo relacionados nao finalizados
                DataTable dtt= banco.BuscarEmprestimo(Emprestimo.FKCARRO+"="+ carroSelecionado.GetPK().ToString()+" and "+Emprestimo.DATA_FINAL+">convert('"+ DateTime.Now.Year.ToString()+"-"+ DateTime.Now.Month.ToString()+"-"+ DateTime.Now.Day.ToString() +"',date)");
                if (dtt.Rows.Count > 0)
                    textBox5.Text = "Indisponivel";
                else
                    textBox5.Text = "Disponivel";

            }
            catch
            {
                carroSelecionado = null;
                txtId.Text = "";
                txtMarca.Text = "";
                txtModelo.Text = "";
                txtPlaca.Text = "";
                textBox10.Text = "";
                textBox5.Text = "";
            }
        }

        private void button2_Click(object sender, EventArgs e)//gerenciar cliente emprestimo
        {
            Form4 clientesemprestimos = new Form4();
            clientesemprestimos.Show();
            this.Hide();
        }
    }
}
