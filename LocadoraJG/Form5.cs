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
    public partial class Form5 : Form
    {
        DataTable veiculos;
        bool editar;
        Emprestimo emprestimo;
        private int clientePK;

        public Form5(Cliente cliente)
        {
            InitializeComponent();
            clientePK = cliente.GetPK();
            editar = false;
            textBox10.Text = clientePK.ToString();
        }

        public Form5(Emprestimo emprestimo)//editar
        {
            InitializeComponent();
             this.emprestimo = emprestimo;
            editar = true;
            label3.Text = "Atualizar empréstimo" ;
            textBox10.Text = emprestimo.GetFKCliente().ToString();
            textBox11.Enabled = false;
            textBox11.Text = emprestimo.GetFKCarro().ToString();
            button1.Text = "Atualizar";
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!editar)//NovoRegistro
            {
                try { 
                    emprestimo = new Emprestimo(int.Parse(textBox11.Text), clientePK);
                    emprestimo.data_inicial = textBox2.Text;
                    emprestimo.data_final = textBox1.Text;
                    emprestimo.situacao = textBox3.Text;
                
                }
                catch (Exception)
                {
                    MessageBox.Show("Erro na atribuição, corriga os campos de entrada");
                    return;
                }
                int pk = new Banco().RegistrarEmprestimo(emprestimo);
                if (pk > 0)
                {
                    MessageBox.Show("Emprestimo registrado, pk:" + pk.ToString());
                    this.Hide();
                }
                else MessageBox.Show("ERRO, registro não efetuado.");
            }
            else//Editar registro ja existente
            {
                emprestimo.data_inicial = textBox2.Text;
                emprestimo.data_final = textBox1.Text;
                emprestimo.situacao = textBox3.Text;
                Banco banco = new Banco();
                if (banco.AtualizarEmprestimo(emprestimo))
                {
                    MessageBox.Show("Emprestimo atualizado com sucesso.");
                    this.Hide();
                }
                else MessageBox.Show("ERRO, registro não efetuado.");
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            new VeiculosForm().Show();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
