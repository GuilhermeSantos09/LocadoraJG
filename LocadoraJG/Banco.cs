using MySql.Data.MySqlClient;
using System;
using System.Data;

namespace LocadoraJG
{
    class Banco
    {
        public MySqlConnection connection;

        //Nomeclatura de Tabelas
        string CARRO = "Carro",
               CLIENTE = "Cliente",
               EMPRESTIMO = "Emprestimo",
               MULTA = "Multa";

        public Banco()
        {
            string conString = "SERVER=localhost;DATABASE=locadoraJG;UID=root;PASSWORD=guillherme;";
            connection = new MySqlConnection(conString);
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

        internal void DeletarCarro(Carro carro)
        {
            throw new NotImplementedException();
        }

        public Carro PegarCarro(int pk)
        {
            if (connection.State == ConnectionState.Closed) connection.Open(); else return null;
            MySqlCommand command = new MySqlCommand("select * from "+CARRO+" where "+Carro.PK+" = "+pk.ToString(),connection);
            MySqlDataReader reader = command.ExecuteReader();
            Carro carro = null;
            if (reader.HasRows)
                while (reader.Read())
                {
                    carro = new Carro(
                        int.Parse(reader[Carro.PK] + ""),
                        reader[Carro.PLACA]+"",
                        reader[Carro.MODELO]+"",
                        reader[Carro.MARCA]+"",
                        int.Parse(reader[Carro.ANO] + ""),
                        int.Parse(reader[Carro.VALOR]+"")
                    );
                }
            if(!reader.IsClosed)reader.Close();
            connection.Close();
            return carro;
        }

        public bool AtualizarCarro(Carro carro)
        {
            if (!carro.HasPK()) return false;
            if (connection.State == ConnectionState.Closed) connection.Open(); else return false;
            MySqlCommand command = new MySqlCommand(
                "update " + CARRO + " set " +
                Carro.PLACA + "='"+ carro.placa +"',"+
                Carro.MODELO + "='" + carro.modelo +"',"+
                Carro.MARCA + "='" + carro.marca +"',"+
                Carro.ANO + "=" + carro.ano +","+
                Carro.VALOR + "=" + carro.valor +
                " where "+ Carro.PK + "=" + carro.GetPK().ToString(), connection);
            int result = command.ExecuteNonQuery();
            connection.Close();
            return result == 1;
        }

        public bool DeletarCarro(int pk)
        {
            if (connection.State == ConnectionState.Closed) connection.Open(); else return false;
            MySqlCommand command = new MySqlCommand(
                "delete from "+CARRO+" where "+Carro.PK+"="+pk.ToString(),connection);
            int result = command.ExecuteNonQuery();
            connection.Close();
            return result == 1;
        }

        public DataTable BuscarCarro(string nullableWhereArg)
        {
            if (connection.State == ConnectionState.Closed) connection.Open(); else return null;
            MySqlCommand command = new MySqlCommand("select * from "+CARRO+
                nullableWhereArg != null?" where " + nullableWhereArg:"", connection);
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            adapter.SelectCommand = command;
            DataTable result = new DataTable("");
            adapter.Fill(result);
            connection.Close();
            return result;
        }

    }
}