using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using TrabalhoHowV.Models;

namespace TrabalhoHowV.Repositorios
{
    public abstract class RepositorioDeLogin<TEntity, Tkey> where TEntity : class
    {
        protected string con { get; } = WebConfigurationManager.ConnectionStrings["HowTeste"].ConnectionString;

        public abstract TEntity GetByEmail(string email);

        public Loginn GetLoginByEmail(int id)
        {
            // Lógica para obter um login pelo id do banco de dados
            using (SqlConnection connection = new SqlConnection(con))
            {
                string query = "SELECT * FROM LOGIN WHERE ID = @ID";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@ID", id);
                Loginn loginn = null;
                try
                {
                    connection.Open();
                    using (var reader = command.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        if (reader.HasRows)
                        {

                            if (reader.Read())
                            {
                                {
                                    loginn = new Loginn();
                                    loginn.ID = Convert.ToInt32(reader["id"]);
                                    loginn.EMAIL = Convert.ToString(reader["email"]);
                                    loginn.NOME_COMPLETO = Convert.ToString(reader["NOME_COMPLETO"]);
                                    loginn.USUARIO = Convert.ToString(reader["usuario"]);
                                    loginn.SENHA = Convert.ToString(reader["senha"]);
                                    loginn.NUMERO_TELEFONE = Convert.ToInt32(reader["NUMERO_TELEFONE"]);
                                    loginn.APELIDO = Convert.ToString(reader["APELIDO"]);
                                    loginn.MANTER_CONECTADO = Convert.ToBoolean(reader["manter_conectado"]);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return loginn;
            }
        }
    }
}