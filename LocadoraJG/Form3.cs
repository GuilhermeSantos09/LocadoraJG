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
    public partial class Form3 : Form
    {
        Banco banco;
        bool editar;
        Cliente cliente;
        public Form3()
        {
            InitializeComponent();
            editar = false;
            banco = new Banco();
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!editar)//NovoRegistro
            {
                try { cliente = new Cliente(textBox6.Text, textBox9.Text, textBox8.Text, textBox7.Text); }
                catch (Exception) { 
                    MessageBox.Show("Erro na conversão, tente mudar o telefone ou cpf"); 
                    return; 
                }
                int pk = banco.RegistrarCliente(cliente);
                if (pk > 0)
                {
                    MessageBox.Show("Cliente registrado, pk:" + pk.ToString());
                    this.Hide();
                }
                else MessageBox.Show("ERRO, registro não efetuado.");
                }
            else//Editar registro ja existente
            {
                cliente.nome = textBox6.Text;
                cliente.endereco = textBox9.Text;
                
                    cliente.cpf = textBox7.Text;
                    cliente.tel = textBox8.Text;
               
                Banco banco = new Banco();
                if (banco.AtualizarCliente(cliente))
                {
                    MessageBox.Show("Cliente atualizado com sucesso.");
                    this.Hide();
                }
                else MessageBox.Show("ERRO, registro não efetuado.");
            }

        }
        public void mudarParaEditar(Cliente cliente)
        {
            this.cliente = cliente;
            label1.Text = "Editar registro de cliente";
            textBox9.Text = cliente.endereco;
            textBox6.Text = cliente.nome;
            textBox8.Text = cliente.tel.ToString();
            textBox7.Text = cliente.cpf.ToString();
            editar = true;
            button1.Text = "Atualizar";
        }
    }
}
