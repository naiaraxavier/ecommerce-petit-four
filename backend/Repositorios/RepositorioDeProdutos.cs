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
    public class RepositorioDeProdutos
    {
        /// <summary>
        /// Conexão do Banco de Dados pego do Web.Config
        /// </summary>
        protected string con { get; } = WebConfigurationManager.ConnectionStrings["HowTeste"].ConnectionString;

        /// <summary>
        /// Lista os produtos contidos na tabela de PRODUTOS no banco de dados.
        /// </summary>
        /// <returns>Retorna todos os produtos dentro da tabela para os cards na View.</returns>
        public List<Produtos> Listar()
        {
            List<Produtos> listaDeProdutos = new List<Produtos>();

            using (SqlConnection conn = new SqlConnection(con))
            {
                string sql = "select * from PRODUTOS";

                SqlCommand cmd = new SqlCommand(sql, conn);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                try
                {
                    while (reader.Read())
                    {
                        Produtos objProduto = new Produtos
                        {
                            ID = (int)reader["ID"],
                            NOME = reader["NOME_PRODUTO"].ToString(),
                            PRECO = reader["PRECO_PRODUTO"] != DBNull.Value ? (decimal)reader["PRECO_PRODUTO"] : default(decimal),
                            DESCRICAO = reader["DESCRICAO_PRODUTO"].ToString(),
                            IMAGEM = reader["IMGEM_PRODUTO"] != DBNull.Value ? (byte[])reader["IMGEM_PRODUTO"] : null
                        };

                        listaDeProdutos.Add(objProduto);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                return listaDeProdutos;
            }
        }

        public Produtos ListarPorId(int id)
        {
            Produtos objProdutos = new Produtos();

            using (SqlConnection conn = new SqlConnection(con))
            {
                string sql = "select * from PRODUTOS where ID = @id";

                SqlCommand cmd = new SqlCommand(sql, conn);

                objProdutos.ID = id;

                cmd.Parameters.AddWithValue("@id", objProdutos.ID);

                conn.Open();

                cmd.ExecuteNonQuery();
            }

            return objProdutos;
        }

        public List<ProdutosQuantidade> ListarProdutosComQuantidade(int[] ids)
        {
            List<ProdutosQuantidade> listaProdutos = new List<ProdutosQuantidade>();

            var produtosAgrupados = ids.GroupBy(id => id);

            foreach (var grupo in produtosAgrupados)
            {
                ProdutosQuantidade produtosQuantidade = new ProdutosQuantidade
                {
                    ID = grupo.Key,
                    QUANTIDADE = grupo.Count()
                };

                listaProdutos.Add(produtosQuantidade);
            }

            return listaProdutos;
        }

        public DataTable ImagemProduo(int id)
        {
            using (var conn = new SqlConnection(con))
            {
                if (conn.State != System.Data.ConnectionState.Open)
                    conn.Open();

                string sql = "select IMGEM_PRODUTO from PRODUTOS where id = @id";

                DataTable dt = new DataTable();

                SqlDataAdapter dtp = new SqlDataAdapter(sql, conn);
                dtp.Fill(dt);

                if (conn.State == System.Data.ConnectionState.Open)
                    conn.Close();

                return dt;
            }
        }

        public DataTable ListarProdutos()
        {
            DataTable dt = new DataTable();

            List<Produtos> listaProdutos = new List<Produtos>();

            using (SqlConnection conn = new SqlConnection(con))
            {
                string sql = "select * from PRODUTOS";

                SqlCommand cmd = new SqlCommand(sql, conn);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                try
                {
                    while (reader.Read())
                    {
                        Produtos objProduto = new Produtos
                        {
                            ID = (int)reader["ID"],
                            NOME = reader["NOME_PRODUTO"].ToString(),
                            PRECO = reader["PRECO_PRODUTO"] != DBNull.Value ? (decimal)reader["PRECO_PRODUTO"] : default(decimal),
                            DESCRICAO = reader["DESCRICAO_PRODUTO"].ToString(),
                            IMAGEM = reader["IMGEM_PRODUTO"] != DBNull.Value ? (byte[])reader["IMGEM_PRODUTO"] : null
                        };

                        listaProdutos.Add(objProduto);
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                return ConvertList(listaProdutos);
            }
        }

        public DataTable ConvertList(List<Produtos> listaProdutos)
        {
            DataTable dt = new DataTable();

            dt.Columns.Add("ID", typeof(int));
            dt.Columns.Add("NOME_PRODUTO", typeof(string));
            dt.Columns.Add("PRECO_PRODUTO", typeof(decimal));
            dt.Columns.Add("DESCRICAO_PRODUTO", typeof(string));
            dt.Columns.Add("IMGEM_PRODUTO", typeof(byte[]));

            foreach (var produto in listaProdutos)
            {
                dt.Rows.Add(produto.ID, produto.NOME, produto.PRECO, produto.DESCRICAO, produto.IMAGEM);
            }

            return dt;
        }

        /// <summary>
        /// Busca no banco de dados, na tabela de produtos, o id do produto que é correspondente com o nome mandado.
        /// </summary>
        /// <param name="productNames"></param>
        /// <returns></returns>
        public List<int> BuscarIdProduto(List<string> productNames)
        {
            List<int> productIds = new List<int>();

            using (SqlConnection connection = new SqlConnection(con))
            {
                connection.Open();

                // Construir a consulta SQL com base na lista de nomes de produto
                string query = "SELECT ID FROM PRODUTOS WHERE NOME_PRODUTO IN (";
                query += string.Join(",", productNames.Select((name, index) => $"@Param{index}"));
                query += ")";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Adiciona os parâmetros para cada nome de produto
                    for (int i = 0; i < productNames.Count; i++)
                    {
                        command.Parameters.AddWithValue($"@Param{i}", productNames[i]);
                    }

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int productId = reader.GetInt32(0);
                            productIds.Add(productId);
                        }
                    }
                }
            }

            return productIds;
        }
    }
}