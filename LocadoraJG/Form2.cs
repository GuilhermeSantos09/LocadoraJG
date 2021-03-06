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
    public partial class Form2 : Form
    {
        private bool editar;
        private Carro carro;
        public Form2()
        {
            InitializeComponent();
            editar = false;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!editar)//NovoRegistro
            {
                carro = new Carro(textBox9.Text, textBox6.Text, textBox7.Text, int.Parse(textBox8.Text), int.Parse(textBox10.Text));
                Banco banco = new Banco();
                int pk = banco.RegistrarCarro(carro);
                if (pk > 0)
                {
                    MessageBox.Show("Carro registrado, pk:" + pk.ToString());
                    this.Hide();
                }
                else MessageBox.Show("ERRO, registro não efetuado.");
            }
            else//Editar registro ja existente
            {
                carro.placa = textBox9.Text;
                carro.modelo = textBox6.Text;
                carro.marca = textBox7.Text;
                carro.ano = int.Parse(textBox8.Text);
                carro.valor = int.Parse(textBox10.Text);
                Banco banco = new Banco();
                if (banco.AtualizarCarro(carro))
                {
                    MessageBox.Show("Carro atualizado com sucesso.");
                    this.Hide();
                }
                else MessageBox.Show("ERRO, registro não efetuado.");
            }
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        public void mudarParaEditar(Carro carro)
        {
            this.carro = carro;
            label1.Text = "Editar registro de carro";
            textBox9.Text =carro.placa;
            textBox6.Text =carro.modelo;
            textBox9.Text =carro.marca;
            carro.ano = int.Parse(textBox8.Text);
            carro.valor = int.Parse(textBox9.Text);
        }
    }
}
