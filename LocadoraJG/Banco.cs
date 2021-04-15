using MySql.Data.MySqlClient;
using System;
using System.Data;

namespace LocadoraJG
{
    class Banco
    {
        public MySqlConnection connection;
        public static string user,password;
        //Nomeclatura de Tabelas
        public static string CARRO = "Carro",
               CLIENTE = "Cliente",
               EMPRESTIMO = "Emprestimo",
               MULTA = "Multa";

        public Banco()
        {
            string conString = "SERVER=localhost;DATABASE=locadoraJG;UID=@u;PASSWORD=@p;".Replace("@u", user).Replace("@p", password);
            connection = new MySqlConnection(conString);
        }
        public static bool TestarConexao()
        {
            bool result;
            string conString = "SERVER=localhost;UID=@u;PASSWORD=@p;".Replace("@u", user).Replace("@p", password);
            MySqlConnection conn = new MySqlConnection(conString);
            try
            {
                conn.Open();
                conn.Close();
                return true;
            }
            catch
            {
                conn.Close();
                return false;
            }
        }
        public static bool DBexitste()
        {
            bool result;
            string conString = "SERVER=localhost;UID=@u;PASSWORD=@p;".Replace("@u", user).Replace("@p", password);
            MySqlConnection conn = new MySqlConnection(conString);
            conn.Open();
            try
            {
                result = new MySqlCommand("SHOW DATABASES LIKE 'locadoraJG'", conn).ExecuteScalar()+"" =="locadorajg";
            }
            catch(Exception)
            {
                
                conn.Close();
                result = false;
            }
            return result;
        }
        public static void Inicializar()
        {
            string conString = "SERVER=localhost;UID=@u;PASSWORD=@p;".Replace("@u", user).Replace("@p", password);
            MySqlConnection conn = new MySqlConnection(conString);
            conn.Open();
            try
            {
                new MySqlCommand("create database if not exists locadoraJG", conn).ExecuteNonQuery();
                new MySqlCommand("use locadoraJG", conn).ExecuteNonQuery();
                new MySqlCommand("create table Carro(pk int PRIMARY KEY,placa varchar(8),modelo varchar(30),marca varchar(30),ano smallint,valor int)", conn).ExecuteNonQuery();
                new MySqlCommand("create table Cliente(pk int PRIMARY KEY,nome varchar(30),cpf varchar(12),tel varchar(15),endereco varchar(75))", conn).ExecuteNonQuery();
                new MySqlCommand("create table Emprestimo(pk int PRIMARY KEY,fk_carro int not null,fk_cliente int not null,data_inicial date,data_final date,situacao varchar(10),foreign key(fk_carro) references Carro(pk) on delete restrict,foreign key(fk_cliente) references Cliente(pk) on delete restrict)", conn).ExecuteNonQuery();
                new MySqlCommand("create table Multa(pk int PRIMARY KEY,fk_emprestimo int not null,descricao varchar(130),valor int,data_emissao date,data_expiracao date,situacao varchar(10),foreign key(fk_emprestimo) references Emprestimo(pk) on delete cascade); ", conn).ExecuteNonQuery();
            }
            catch (Exception) {
                conn.Close();
            }
        }

        public int GetLastPK(string tabela)
        {
            if (connection.State == ConnectionState.Closed) connection.Open(); else return -1;
            MySqlCommand command = new MySqlCommand("select MAX(" + ContemPK.PK + ") from " + tabela, connection);
            object result = command.ExecuteScalar();
            int lastpk;
            try
            {
                lastpk = int.Parse(result + "");
            }
            catch
            {
                lastpk = 0;
            }
            connection.Close();
            return lastpk;
        }

        public int RegistrarCarro(Carro carro)//retorna pk criada ou -1
        {
            if (carro.HasPK()) return -1;
            int lastpk = GetLastPK(CARRO);
            if (connection.State == ConnectionState.Closed) connection.Open(); else return -1;
            MySqlCommand command = new MySqlCommand(
                "insert into " + CARRO + " (" +
                Carro.PK + "," +
                Carro.PLACA + "," +
                Carro.MODELO + "," +
                Carro.MARCA + "," +
                Carro.ANO + "," +
                Carro.VALOR +
                ") values (" +
                (lastpk + 1).ToString() + ",'" +
                carro.placa + "','" +
                carro.modelo + "','" +
                carro.marca + "'," +
                carro.ano.ToString() + "," +
                carro.valor.ToString() +
                ")", connection);

            int result = command.ExecuteNonQuery();

            connection.Close();
            return result == 1 ? lastpk + 1 : -1;
        }

        public Carro PegarCarro(int pk)
        {
            if (connection.State == ConnectionState.Closed) connection.Open(); else return null;
            MySqlCommand command = new MySqlCommand("select * from " + CARRO + " where " + Carro.PK + " = " + pk.ToString(), connection);
            MySqlDataReader reader = command.ExecuteReader();
            Carro carro = null;
            if (reader.HasRows)
                while (reader.Read())
                {
                    carro = new Carro(
                        int.Parse(reader[Carro.PK] + ""),
                        reader[Carro.PLACA] + "",
                        reader[Carro.MODELO] + "",
                        reader[Carro.MARCA] + "",
                        int.Parse(reader[Carro.ANO] + ""),
                        int.Parse(reader[Carro.VALOR] + "")
                    );
                }
            if (!reader.IsClosed) reader.Close();
            connection.Close();
            return carro;
        }

        public bool AtualizarCarro(Carro carro)
        {
            if (!carro.HasPK()) return false;
            if (connection.State == ConnectionState.Closed) connection.Open(); else return false;
            MySqlCommand command = new MySqlCommand(
                "update " + CARRO + " set " +
                Carro.PLACA + "='" + carro.placa + "'," +
                Carro.MODELO + "='" + carro.modelo + "'," +
                Carro.MARCA + "='" + carro.marca + "'," +
                Carro.ANO + "=" + carro.ano + "," +
                Carro.VALOR + "=" + carro.valor +
                " where " + Carro.PK + "=" + carro.GetPK().ToString(), connection);
            int result = command.ExecuteNonQuery();
            connection.Close();
            return result == 1;
        }

        public bool DeletarCarro(int pk)
        {
            if (connection.State == ConnectionState.Closed) connection.Open(); else return false;
            MySqlCommand command = new MySqlCommand(
                "delete from " + CARRO + " where " + Carro.PK + "=" + pk.ToString(), connection);
            int result = command.ExecuteNonQuery();
            connection.Close();
            return result == 1;
        }

        public DataTable BuscarCarro(string nullableWhereArg)
        {
            if (connection.State == ConnectionState.Closed) connection.Open(); else return null;

            MySqlCommand command = new MySqlCommand("select * from " +
                (nullableWhereArg != null ? (CARRO + " where " + nullableWhereArg) : CARRO), connection);
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            adapter.SelectCommand = command;
            DataTable result = new DataTable("");
            adapter.Fill(result);
            connection.Close();
            return result;
        }


        public int RegistrarCliente(Cliente cliente)//retorna pk criada ou -1
        {
            if (cliente.HasPK()) return -1;
            int lastpk = GetLastPK(CLIENTE);
            if (connection.State == ConnectionState.Closed) connection.Open(); else return -1;
            MySqlCommand command = new MySqlCommand(
                "insert into " + CLIENTE + " (" +
                Cliente.PK + "," +
                Cliente.CPF + "," +
                Cliente.NOME + "," +
                Cliente.ENDERECO + "," +
                Cliente.TEL +
                ") values (" +
                (lastpk + 1).ToString() + ",'" +
                cliente.cpf + "','" +
                cliente.nome + "','" +
                cliente.endereco + "','" +
                cliente.tel + "')"
                , connection);

            int result = command.ExecuteNonQuery();

            connection.Close();
            return result == 1 ? lastpk + 1 : -1;
        }

        public Cliente PegarCliente(int pk)
        {
            if (connection.State == ConnectionState.Closed) connection.Open(); else return null;
            MySqlCommand command = new MySqlCommand("select * from " + CLIENTE + " where " + Cliente.PK + " = " + pk.ToString(), connection);
            MySqlDataReader reader = command.ExecuteReader();
            Cliente cliente = null;
            if (reader.HasRows)
                while (reader.Read())
                {
                    cliente = new Cliente(
                        int.Parse(reader[Cliente.PK] + ""),
                        reader[Cliente.NOME] + "",
                        reader[Cliente.ENDERECO] + "",
                        reader[Cliente.CPF] + "",
                        reader[Cliente.TEL] + ""
                    );
                }
            if (!reader.IsClosed) reader.Close();
            connection.Close();
            return cliente;
        }

        public bool AtualizarCliente(Cliente cliente)
        {
            if (!cliente.HasPK()) return false;
            if (connection.State == ConnectionState.Closed) connection.Open(); else return false;
            MySqlCommand command = new MySqlCommand(
                "update " + CLIENTE + " set " +
                Cliente.NOME + "='" + cliente.nome + "'," +
                Cliente.CPF + "='" + cliente.cpf + "'," +
                Cliente.ENDERECO + "='" + cliente.endereco + "'," +
                Cliente.TEL + "='" + cliente.tel +
                "' where " + Cliente.PK + "=" + cliente.GetPK().ToString(), connection);
            int result = command.ExecuteNonQuery();
            connection.Close();
            return result == 1;
        }

        public bool DeletarCliente(int pk)
        {
            if (connection.State == ConnectionState.Closed) connection.Open(); else return false;
            MySqlCommand command = new MySqlCommand(
                "delete from " + CLIENTE + " where " + Cliente.PK + "=" + pk.ToString(), connection);
            int result = command.ExecuteNonQuery();
            connection.Close();
            return result == 1;
        }

        public DataTable BuscarCliente(string nullableWhereArg)
        {
            if (connection.State == ConnectionState.Closed) connection.Open(); else return null;
            MySqlCommand command = new MySqlCommand("select * from " +
                (nullableWhereArg != null ? (CLIENTE + " where " + nullableWhereArg) : CLIENTE), connection);
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            adapter.SelectCommand = command;
            DataTable result = new DataTable("");
            adapter.Fill(result);
            connection.Close();
            return result;
        }

        public int RegistrarEmprestimo(Emprestimo emprestimo)//retorna pk criada ou -1
        {
            try
            {
                if (emprestimo.HasPK()) return -1;
                int lastpk = GetLastPK(EMPRESTIMO);
                if (connection.State == ConnectionState.Closed) connection.Open(); else return -1;
                MySqlCommand command = new MySqlCommand(
                    "insert into " + EMPRESTIMO + " (" +
                    Emprestimo.PK + "," +
                    Emprestimo.FKCARRO + "," +
                    Emprestimo.FKCLIENTE + "," +
                    Emprestimo.DATA_FINAL + "," +
                    Emprestimo.DATA_INICIAL + "," +
                    Emprestimo.SITUACAO +
                    ") values (" +
                    (lastpk + 1).ToString() + ", " +
                    emprestimo.GetFKCarro().ToString() + ", " +
                    emprestimo.GetFKCliente().ToString() + ", " +
                    "(convert('" + emprestimo.data_final + "',date)), " +
                    "(convert('" + emprestimo.data_inicial + "',date)), '" +
                    emprestimo.situacao.ToString() +
                    "');", connection);

                int result = command.ExecuteNonQuery();

                connection.Close();
                return result == 1 ? lastpk + 1 : -1;
            }
            catch (Exception)
            {
                throw new Exception();
            } 
        }

        

        public Emprestimo PegarEmprestimo(int pk)
        {
            if (connection.State == ConnectionState.Closed) connection.Open(); else return null;
            MySqlCommand command = new MySqlCommand("select * from " + EMPRESTIMO + " where " + Emprestimo.PK + " = " + pk.ToString(), connection);
            MySqlDataReader reader = command.ExecuteReader();
            Emprestimo emprestimo = null;
            if (reader.HasRows)
                while (reader.Read())
                {
                    emprestimo = new Emprestimo(
                        int.Parse(reader[Emprestimo.PK] + ""),
                        int.Parse(reader[Emprestimo.FKCARRO] + ""),
                        int.Parse(reader[Emprestimo.FKCLIENTE]+"")
                    );
                    emprestimo.situacao = reader[Emprestimo.SITUACAO] + "";

                }
            if (!reader.IsClosed) reader.Close();
            connection.Close();
            return emprestimo;
        }

        public bool AtualizarEmprestimo(Emprestimo emprestimo)
        {
            if (!emprestimo.HasPK()) return false;
            if (connection.State == ConnectionState.Closed) connection.Open(); else return false;
            MySqlCommand command = new MySqlCommand(
                "update " + EMPRESTIMO + " set " +
                Emprestimo.DATA_FINAL + "="+emprestimo.data_final.ToString()+","+
                Emprestimo.DATA_FINAL + "=" + emprestimo.data_inicial.ToString() + ","+
                Emprestimo.SITUACAO+"='"+ emprestimo.situacao +"'"+
                " where " + Emprestimo.PK + "=" + emprestimo.GetPK().ToString(), connection);
            int result = command.ExecuteNonQuery();
            connection.Close();
            return result == 1;
        }

        public bool DeletarEmprestimo(int pk)
        {
            if (connection.State == ConnectionState.Closed) connection.Open(); else return false;
            MySqlCommand command = new MySqlCommand(
                "delete from " + EMPRESTIMO + " where " + Emprestimo.PK + "=" + pk.ToString(), connection);
            int result = command.ExecuteNonQuery();
            connection.Close();
            return result == 1;
        }

        public DataTable BuscarEmprestimo(string nullableWhereArg)
        {
            if (connection.State == ConnectionState.Closed) connection.Open(); else return null;
            MySqlCommand command = new MySqlCommand("select * from " +
                (nullableWhereArg != null ? EMPRESTIMO+" where " + nullableWhereArg : EMPRESTIMO), connection);
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            adapter.SelectCommand = command;
            DataTable result = new DataTable("");
            adapter.Fill(result);
            connection.Close();
            return result;
        }

        public int RegistrarMulta(Multa multa)//retorna pk criada ou -1
        {
            if (multa.HasPK()) return -1;
            int lastpk = GetLastPK(MULTA);
            if (connection.State == ConnectionState.Closed) connection.Open(); else return -1;
            MySqlCommand command = new MySqlCommand(
                "insert into " + MULTA + " (" +
                Multa.PK + "," +
                Multa.FKEMPRESTIMO + "," +
                Multa.DESCRICAO+","+
                Multa.VALOR+","+
                Multa.DATA_EMISSAO + "," +
                Multa.DATA_EXPIRACAO + "," +
                Multa.SITUACAO +
                ") values (" +
                (lastpk + 1).ToString() + "," +
                multa.GetFk()+ ",'" +
                multa.descricao + "'," +
                multa.valor+","+
                "convert('"+ multa.data_emissao + "',date)," +
                "convert('"+multa.data_expiracao + "',date),'" +
                multa.situacao+
                "')", connection);

            int result = command.ExecuteNonQuery();

            connection.Close();
            return result == 1 ? lastpk + 1 : -1;
        }

        public Multa PegarMulta(int pk)
        {
            if (connection.State == ConnectionState.Closed) connection.Open(); else return null;
            MySqlCommand command = new MySqlCommand("select * from " + MULTA + " where " + Multa.PK + " = " + pk.ToString(), connection);
            MySqlDataReader reader = command.ExecuteReader();
            Multa multa = null;
            if (reader.HasRows)
                while (reader.Read())
                {
                    multa = new Multa(
                        int.Parse(reader[Multa.PK] + ""),
                        int.Parse(reader[Multa.FKEMPRESTIMO] + ""),
                        int.Parse(reader[Multa.VALOR] + "")
                    );
                    multa.descricao = reader[Multa.DESCRICAO] + "";
                    multa.data_emissao = reader[Multa.DATA_EMISSAO] + "";
                    multa.data_expiracao = reader[Multa.DATA_EXPIRACAO] + "";
                }
            if (!reader.IsClosed) reader.Close();
            connection.Close();
            return multa;
        }

        public bool AtualizarMulta(Multa multa)
        {
            if (!multa.HasPK()) return false;
            if (connection.State == ConnectionState.Closed) connection.Open(); else return false;
            MySqlCommand command = new MySqlCommand(
                "update " + MULTA + " set " +
                Multa.DESCRICAO + "='" + multa.descricao + "'," +
                Multa.DATA_EMISSAO + "=convert("+multa.data_emissao+",date),"+
                Multa.DATA_EXPIRACAO + "=convert("+multa.data_expiracao+",date),"+
                Multa.VALOR +"="+multa.valor+
                " where " + Multa.PK + "=" + multa.GetPK().ToString(), connection);
            int result = command.ExecuteNonQuery();
            connection.Close();
            return result == 1;
        }

        public bool DeletarMulta(int pk)
        {
            if (connection.State == ConnectionState.Closed) connection.Open(); else return false;
            MySqlCommand command = new MySqlCommand(
                "delete from " + MULTA + " where " + Multa.PK + "=" + pk.ToString(), connection);
            int result = command.ExecuteNonQuery();
            connection.Close();
            return result == 1;
        }

        public DataTable BuscarMulta(string nullableWhereArg)
        {
            if (connection.State == ConnectionState.Closed) connection.Open(); else return null;
            MySqlCommand command = new MySqlCommand("select * from " +
                (nullableWhereArg != null ? (MULTA +" where " + nullableWhereArg) : MULTA), connection);
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            adapter.SelectCommand = command;
            DataTable result = new DataTable("");
            adapter.Fill(result);
            connection.Close();
            return result;
        }

    }
}
