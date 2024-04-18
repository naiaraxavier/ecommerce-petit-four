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
    internal class RepositorioDeUsuarioLogado
    {
        protected string con { get; } = WebConfigurationManager.ConnectionStrings["HowTeste"].ConnectionString;

        public UsuarioLogado Consultar(int id)
        {
            using (SqlConnection connection = new SqlConnection(con))
            {
                string strSql = "select id, nome_fisico_juridico, primeiro_nome, email from cliente where id = @id";
                SqlCommand cmd = new SqlCommand(strSql, connection);
                cmd.Parameters.AddWithValue("@id", id);
                UsuarioLogado usuario = null;
                try
                {
                    connection.Open();
                    using (var reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        if (reader.HasRows)
                        {
                            if (reader.Read())
                            {
                                usuario = new UsuarioLogado();
                                usuario.ID = Convert.ToInt32(reader["id"]);
                                usuario.EMAIL = Convert.ToString(reader["email"]);
                                usuario.NOME_PESSOAL = Convert.ToString(reader["nome_fisico_juridico"]);
                                usuario.USUARIO = Convert.ToString(reader["primeiro_nome"]);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return usuario;
            }
        }
    }
}