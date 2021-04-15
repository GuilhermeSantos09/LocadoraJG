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
    public partial class Form4 : Form
    {
        DataTable clientes;
        Cliente clienteSelecionado;
        Banco banco;
        List<Emprestimo> emprestimos;
        List<Multa> multas;
        int emprestimoAtual = -1,
            multaAtual = -1;
        public Form4()
        {
            InitializeComponent();
            banco = new Banco();
            clientes = banco.BuscarCliente(null);
            dataGridView1.DataSource = clientes;
            dataGridView1.Refresh();
        }

        private void Form4_Load(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (clienteSelecionado != null)
            {
                Form3 editarCliente= new Form3();//reutilizo da tela de registro
                Banco banco = new Banco();
                Cliente cliente = banco.PegarCliente(clienteSelecionado.GetPK());
                editarCliente.mudarParaEditar(cliente);
                editarCliente.Show();
            }
        }

        private void button6_Click(object sender, EventArgs e)//deletar cliente
        {
            if (clienteSelecionado != null)
            {
                Banco banco = new Banco();
                DialogResult diagResult = MessageBox.Show("Você tem certeza que deseja deletar o cliente de pk: " + clienteSelecionado.GetPK().ToString() + " ?", "Deletar Cliente", MessageBoxButtons.YesNo);
                if (diagResult == DialogResult.Yes)
                {
                    if (banco.DeletarCliente(clienteSelecionado.GetPK()))
                        MessageBox.Show("Registro deletado");
                    else
                        MessageBox.Show("ERRO, registro não deletado");
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)//multar
        {
            if (clienteSelecionado != null && emprestimos!=null && emprestimoAtual>=0)
            {
                DialogResult result = MessageBox.Show("Confirme o ID de cliente e Emprestimo\nID de Cliente: " + clienteSelecionado.GetPK().ToString() + "\nID do Emprestimo: " + emprestimos.ElementAt(emprestimoAtual).GetPK().ToString() + "\nDeseja prosseguir?", "Multar", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes) new Form6(clienteSelecionado,emprestimos.ElementAt(emprestimoAtual)).Show();
            }
            else
            {
                MessageBox.Show("Selecione um cliente e seu emprestimo antes de prosseguir", "Erro Multar", MessageBoxButtons.OK);
            }
        }

        private void button1_Click(object sender, EventArgs e)//gerenciar carros
        {
            Form1 carros = new Form1();
            carros.Show();
            this.Close();
        }

        private void button5_Click(object sender, EventArgs e)//novo emprestimo
        {
            if (clienteSelecionado != null) 
            {
                DialogResult result = MessageBox.Show("Confirme o ID de cliente\nID de Cliente: " + clienteSelecionado.GetPK().ToString() + "\nDeseja prosseguir?", "Novo Emprestimo", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes) new Form5(clienteSelecionado).Show();
            }
            else
            {
                MessageBox.Show("Selecione um cliente antes de prosseguir", "Erro Novo Empréstimo",MessageBoxButtons.OK);
            }
        }

        private void button2_Click(object sender, EventArgs e)//novo cliente
        {
            new Form3().Show();
        }

        private void dataGridView1_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)//selecionar cliente
        {
            try
            {
                clienteSelecionado = new Banco().PegarCliente((int)dataGridView1.Rows[e.RowIndex].Cells[0].Value);
                if (clienteSelecionado == null) throw new Exception();
                textBox5.Text = clienteSelecionado.GetPK().ToString();
                textBox6.Text = clienteSelecionado.nome;
                textBox7.Text = clienteSelecionado.cpf.ToString();
                textBox8.Text = clienteSelecionado.tel.ToString();
                textBox9.Text = clienteSelecionado.endereco;
                emprestimos = null;
                emprestimoAtual = -1;
                EsconderEmprestimo();
                btnEmprestimoEsqr.Visible = false;
                btnEmprestimoDirt.Visible = true;
            }
            catch (Exception)
            {
                clientes = null;
                multas = null;
                emprestimos = null;
                multaAtual = -1;
                emprestimoAtual = -1;
                clienteSelecionado = null;
                textBox5.Text = "";
                textBox6.Text = "";
                textBox7.Text = "";
                textBox8.Text = "";
                textBox9.Text = "";
                btnEmprestimoDirt.Visible = false;
                EsconderEmprestimo();
            }
        }

        private void btnEmprestimoDirt_Click(object sender, EventArgs e)//proximo emprestimo
        {
            if (emprestimos == null && clienteSelecionado != null)//cliente selecionado; emprestimos não carregados.
            {
                emprestimoAtual = -1;
                if (!AdquirirEmprestimos())
                {
                    textBox10.Text = "0";
                    emprestimos = null;
                    return;
                }
                btnEmprestimoEsqr.Visible = true;
            }
            if (emprestimos != null) //emprestimos carregados
            {
                if( emprestimoAtual < emprestimos.Count-1) emprestimoAtual++;
                else emprestimoAtual = 0;
                MostrarEmprestimo();
            }
        }

        private void btnEmprestimoEsqr_Click(object sender, EventArgs e)
        {
            if (emprestimos != null) //emprestimos carregados
            {
                if (emprestimoAtual > 0) emprestimoAtual--;
                else emprestimoAtual = emprestimos.Count-1;
                MostrarEmprestimo();
            }
            else (sender as Button).Visible = false;
        }

        private void MostrarEmprestimo()
        {
            textBox10.Text = emprestimos.ElementAt(emprestimoAtual).GetPK().ToString();
            textBox11.Text = emprestimos.ElementAt(emprestimoAtual).GetFKCarro().ToString();
            textBox3.Text = emprestimos.ElementAt(emprestimoAtual).data_inicial;
            textBox4.Text = emprestimos.ElementAt(emprestimoAtual).data_final;
            EsconderMulta();
            multas = null;
            multaAtual = -1;
            btnMultaDirt.Visible = true;
        }
        private void EsconderEmprestimo()
        {
            textBox10.Text = "";
            textBox11.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            btnMultaDirt.Visible = false;
            EsconderMulta();
        }
        private bool AdquirirEmprestimos()
        {
            Emprestimo rootListItem;
            this.emprestimos = new List<Emprestimo>();
            object[] emprestimo;
            DataTable emprestimos = banco.BuscarEmprestimo(Emprestimo.FKCLIENTE + " = " + clienteSelecionado.GetPK().ToString());
            for (int i = 0; i < emprestimos.Rows.Count; i++)
            {
                emprestimo = emprestimos.Rows[i].ItemArray;
                rootListItem = new Emprestimo((int)emprestimo[0], (int)emprestimo[1], (int)emprestimo[2]);
                rootListItem.data_inicial = emprestimo[3].ToString();
                rootListItem.data_final = emprestimo[4].ToString();
                this.emprestimos.Add(rootListItem);
            }
            return this.emprestimos.Count > 0;

        }

        private bool AdquirirMultas()
        {
            if (emprestimos!=null&&emprestimoAtual>=0) {
                Multa rootListItem;
                this.multas = new List<Multa>();
                object[] multa;
                DataTable multas = banco.BuscarMulta(Multa.FKEMPRESTIMO + " = " + emprestimos[emprestimoAtual].GetPK().ToString());
                for (int i = 0; i < multas.Rows.Count; i++)
                {
                    multa = multas.Rows[i].ItemArray;
                    rootListItem = new Multa((int)multa[0], (int)multa[1], (int)multa[3]);
                    rootListItem.descricao = multa[2].ToString();
                    rootListItem.data_emissao = multa[4].ToString();
                    rootListItem.data_expiracao = multa[5].ToString();
                    this.multas.Add(rootListItem);
                }
                return this.multas.Count > 0;
            }
            return false;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (multas != null && multaAtual > -1)
            {
                Form6 editarMulta = new Form6(multas.ElementAt(multaAtual));//reutilizo da tela de registro
                Banco banco = new Banco();
                editarMulta.Show();
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void btnMultaDirt_Click(object sender, EventArgs e)
        {
            if (multas == null && (emprestimoAtual>=0&&emprestimos!=null))//emprestimo selecionado; multas não carregadas.
            {
                multaAtual = -1;
                multas = null;
                if (!AdquirirMultas())
                {
                    multas = null;
                    textBox2.Text = "0";
                    return;
                }
                btnMultaEsqr.Visible = true;
            }
            if (multas != null&&multas.Count()>0) //multas carregadas
            {
                if (multaAtual < multas.Count-1) multaAtual++;
                else multaAtual = 0;
                MostrarMulta();
            }
        }

        private void btnMultaEsqr_Click(object sender, EventArgs e)
        {
            if (multas != null) //emprestimos carregados
            {
                if (multaAtual > 0) multaAtual--;
                else multaAtual = multas.Count - 1;
                MostrarMulta();
            }
            else (sender as Button).Visible = false;
        }

        private void button9_Click(object sender, EventArgs e)//Calcular total
        {

            if(emprestimos!=null&&emprestimoAtual>=0)
            {
                if (multas == null) AdquirirMultas();

                Emprestimo emprestimo = emprestimos.ElementAt(emprestimoAtual);
                int valorDiario /*veiculo*/ = banco.PegarCarro(emprestimo.GetFKCarro()).valor;
                try
                {
                    int dias = (int)(
                    (Convert.ToDateTime(emprestimo.data_final.Substring(0, 10)))
                    -
                    (Convert.ToDateTime(emprestimo.data_inicial.Substring(0, 10)))
                ).TotalDays;

                    int valorTotal = valorDiario * dias;

                    if(multas!=null)
                    for (int i = 0; i < multas.Count(); i++)
                    {
                        valorTotal += multas.ElementAt(i).valor;
                    }
                    textBox15.Text = "R$" + valorTotal.ToString();
                    textBox14.Text = "R$" + (valorDiario * dias).ToString();
                }
                catch (Exception)
                {
                    MessageBox.Show("Erro no preço","certifique-se que todos os campos de datas e valores estão preenchidos");
                }
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            clientes = null;
            multas = null;
            emprestimos = null;
            multaAtual = -1;
            emprestimoAtual = -1;
            clienteSelecionado = null;
            Banco banco = new Banco();
            clientes = banco.BuscarCliente(null);
            dataGridView1.DataSource = clientes;
            dataGridView1.Refresh();
            textBox5.Text = "";
            textBox6.Text = "";
            textBox7.Text = "";
            textBox8.Text = "";
            textBox9.Text = "";
            btnEmprestimoDirt.Visible = false;
            EsconderEmprestimo();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (multas != null&&multaAtual>-1)
            {
                Banco banco = new Banco();
                DialogResult diagResult = MessageBox.Show( "Você tem certeza que deseja a multa de pk: " + multas.ElementAt(multaAtual).GetPK().ToString() + " ?", "Deletar Multa", MessageBoxButtons.YesNo);
                if (diagResult == DialogResult.Yes)
                {
                    if (banco.DeletarMulta(multas.ElementAt(multaAtual).GetPK()))
                        MessageBox.Show("Registro deletado");
                    else
                        MessageBox.Show("ERRO, registro não deletado");
                }
            }
        }

        private void MostrarMulta()
        {
            textBox2.Text = multas.ElementAt(multaAtual).GetPK().ToString();
            textBox1.Text = "R$"+multas.ElementAt(multaAtual).valor.ToString();
            textBox13.Text = multas.ElementAt(multaAtual).data_emissao.ToString();
            textBox12.Text = multas.ElementAt(multaAtual).data_expiracao.ToString();
        }

        private void button12_Click(object sender, EventArgs e)
        {
            if (emprestimos != null && emprestimoAtual > -1)
            {
                new Form5(emprestimos[emprestimoAtual]).Show();
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            if (emprestimos!= null && emprestimoAtual>-1)
            {
                Banco banco = new Banco();
                DialogResult diagResult = MessageBox.Show("Você tem certeza que deseja deletar o emprestimo de pk: " + emprestimos[emprestimoAtual].GetPK().ToString() + " ?", "Deletar Emprestimo", MessageBoxButtons.YesNo);
                if (diagResult == DialogResult.Yes)
                {
                    if (banco.DeletarEmprestimo(emprestimos[emprestimoAtual].GetPK()))
                        MessageBox.Show("Registro deletado");
                    else
                        MessageBox.Show("ERRO, registro não deletado");
                }
            }
        }

        private void EsconderMulta()
        {
            textBox2.Text = "";
            textBox1.Text = "";
            textBox13.Text ="";
            textBox12.Text = "";
            btnMultaEsqr.Visible = false;
        }
    }
    }
