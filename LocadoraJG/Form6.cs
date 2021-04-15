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
    public partial class Form6 : Form
    {
        Cliente cliente;
        Emprestimo emprestimo;
        Multa multa;
        bool editar;
        public Form6(Cliente cliente, Emprestimo emprestimo)
        {
            InitializeComponent();
            this.cliente = cliente;
            this.emprestimo = emprestimo;
            textBox4.Text = cliente.GetPK().ToString();
            textBox10.Text = emprestimo.GetPK().ToString();
            editar = false;
        }

        public Form6(Multa multa)//editar
        {
            InitializeComponent();
            editar = true;
            this.multa = multa;
            button1.Text = "Atualizar";
            emprestimo = new Banco().PegarEmprestimo(multa.GetFk());
            textBox4.Text = emprestimo.GetFKCliente().ToString();
            textBox10.Text = emprestimo.GetPK().ToString();
            textBox5.Text = multa.descricao;
            textBox2.Text = multa.data_emissao;
            textBox1.Text = multa.data_expiracao;
            textBox3.Text = multa.valor.ToString();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (!editar)
            {
                Banco banco = new Banco();
                multa = new Multa(emprestimo, int.Parse(textBox3.Text));
                multa.descricao = textBox5.Text;
                multa.data_emissao = textBox2.Text;
                multa.data_expiracao = textBox1.Text;
                int pk = banco.RegistrarMulta(multa);

                if (pk > 0)
                {
                    MessageBox.Show("Multa registrada, pk:" + pk.ToString());
                    this.Hide();
                }
                else MessageBox.Show("ERRO, registro não efetuado.");
            }
            else
            {
                multa.descricao = textBox5.Text;
                multa.data_emissao = textBox2.Text;
                multa.data_expiracao = textBox1.Text;
                multa.valor = int.Parse(textBox3.Text);
                if (new Banco().AtualizarMulta(multa))
                {
                    MessageBox.Show("Multa atualizada com sucesso.");
                    this.Hide();
                }
                else MessageBox.Show("ERRO, registro não efetuado.");
            }
        }
    }
}
