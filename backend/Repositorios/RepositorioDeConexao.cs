using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using TrabalhoHowV.Models;

namespace TrabalhoHowV.Repositorios
{
    public class RepositorioDeConexao : RepositorioDeLogin<Loginn, int>
    {
        /// <summary>
        /// <param name="email"></param>
        /// </summary>

        public override Loginn GetByEmail(string email)
        {
            using (var conn = new SqlConnection(con))
            {
                string sql = "select ID from LOGIN where EMAIL=@email";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@email", email);
                Loginn objLogin = null;
                try
                {
                    conn.Open();
                    using (var reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        if (reader.HasRows)
                        {
                            if (reader.Read())
                            {
                                objLogin = new Loginn();
                                objLogin.ID = (int)reader["ID"];
                                //objLogin.NOME_COMPLETO = reader["NOME_COMPLETO"].ToString();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return objLogin;
            }
        }
    }
}