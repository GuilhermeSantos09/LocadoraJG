using System;

public abstract class ContemPK
{

    private int pk = 0;

    //Nomeclatura de atributo (BD)
    public static readonly string PK = "pk";

    public bool HasPK()
    {
        return pk > 0;
    }
    public int GetPK()
    {
        return pk;
    }
    public void SetPK(Int32 pk)
    {
        if (this.pk > 0) throw new Exception("Set() Em PK já existente.");
        else this.pk = pk;
    }
}

public class Carro : ContemPK
{

    public string placa,
                  modelo,
                  marca;

    public int ano,
               valor;//Alugel

    //Nomeclatura de atributos (BD)
    public static readonly string PLACA = "placa", MODELO = "modelo", MARCA = "marca", ANO = "ano", VALOR = "valor";

    public Carro(string placa, string modelo, string marca, int ano, int valor)
    {
        this.placa = placa;
        this.modelo = modelo;
        this.marca = marca;
        this.ano = ano;
        this.valor = valor;
    }

    public Carro(Int32 pk, string placa, string modelo, string marca, int ano, int valor)
    {
        this.placa = placa;
        this.modelo = modelo;
        this.marca = marca;
        this.ano = ano;
        this.valor = valor;
        SetPK(pk);
    }
}

public class Cliente : ContemPK
{
    public string nome, endereco;
    public int cpf, tel;

    //Nomeclatura de atributos (BD)
    public static readonly string NOME = "nome", ENDERECO = "endereco", CPF = "cpf", TEL = "tel";
        public Cliente(string nome, string endereco, int cpf, int tel)
    {
        this.nome = nome;
        this.endereco = endereco;
        this.cpf = cpf;
        this.tel = tel;
    }

    public Cliente(int pk, string nome, string endereco, int cpf, int tel)
    {
        this.nome = nome;
        this.endereco = endereco;
        this.cpf = cpf;
        this.tel = tel;
        SetPK(pk);
    }
}

public class Emprestimo : ContemPK
{
    private int fkCarro, fkCliente;
    public int data_inicial, data_final;
    public string situacao;

    //Nomeclatura de atributos (BD)
    public static readonly string FKCARRO = "fk_carro", FKCLIENTE = "fk_cliente", DATA_INICIAL = "data_inicial", DATA_FINAL = "data_final", SITUACAO = "";

    public Emprestimo(Carro carro, Cliente cliente)
    {
        if (!carro.HasPK() || !cliente.HasPK()) throw new Exception("Sem referencia de FK");
        else
        {
            fkCarro = carro.GetPK();
            fkCliente = cliente.GetPK();
        }
    }

    public Emprestimo(int pk, Carro carro, Cliente cliente)
    {
        if (!carro.HasPK() || !cliente.HasPK()) throw new Exception("Sem referencia de FK");
        else
        {
            fkCarro = carro.GetPK();
            fkCliente = cliente.GetPK();
            SetPK(pk);
        }

    }

    public int GetFKCarro()
    {
        return fkCarro;
    }

    public int GetFKCliente()
    {
        return fkCliente;
    }

}

class Multa : ContemPK
{
    private int fkEmprestimo;
    public int valor, data_emissao, data_expiracao;
    public string descricao, situacao;

    //Nomeclatura de atributos (BD)
    public static readonly string FKEMPRESTIMO = "fk_emprestimo", VALOR = "valor", DATA_EMISSAO = "data_emissao", DATA_EXPIRACAO = "data_expiracao", DESCRICAO = "descricao", SITUACAO = "situacao";

    public Multa(Emprestimo emprestimo, int valor)
    {
        if (!emprestimo.HasPK()) throw new Exception("Sem referencia de FK");
        else
        {
            fkEmprestimo = emprestimo.GetPK();
            this.valor = valor;
        }
    }

    public Multa(int pk, Emprestimo emprestimo, int valor)
    {
        if (!emprestimo.HasPK()) throw new Exception("Sem referencia de FK");
        else
        {
            fkEmprestimo = emprestimo.GetPK();
            this.valor = valor;
            SetPK(pk);
        }
    }

    public int GetFk()
    {
        return fkEmprestimo;
    }
}