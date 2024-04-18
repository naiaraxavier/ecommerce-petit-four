using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using TrabalhoHowV.Models;

namespace TrabalhoHowV.Repositorios
{
    public class RepositorioDeCarrinho
    {
        /// <summary>
        /// Conexão do banco de dados pega do Web.Config.
        /// </summary>
        protected string con { get; } = WebConfigurationManager.ConnectionStrings["HowTeste"].ConnectionString;

        /// <summary>
        /// Lista todos os itens do histórico do Cliente que ficam guardados dentro da tabela de CARRINHO.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Todos os itens da tabela pelo ID do Cliente.</returns>
        public List<Carrinho> Listar(int id)
        {
            List<Carrinho> listaCarrinho = new List<Carrinho>();

            string sql = "select * from CARRINHO where id = @id";

            using (SqlConnection conn = new SqlConnection(con))
            {
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Carrinho objCarrinho = new Carrinho
                            {
                                ID = Convert.ToInt32(reader["ID"]),
                                NOME_PRODUTO = reader["NOME_PRODUTO"].ToString(),
                                PRECO_PRODUTO = Convert.ToInt32(reader["PRECO_PRODUTO"]),
                                QUANTIDADE = Convert.ToInt32(reader["QUANTIDADE"]),
                                ID_CLIENTE = Convert.ToInt32(reader["ID_CLIENTE"]),
                                DATA_COMPRA = Convert.ToDateTime(reader["DATA_COMPRA"])
                            };

                            listaCarrinho.Add(objCarrinho);
                        }
                    }
                }
            }

            return listaCarrinho;
        }

        public void AdicionarItemCarrinho(Carrinho carrinho)
        {
            // Cria um dicionário para armazenar os IDs e quantidades dos produtos
            Dictionary<int, int> produtosNoCarrinho = new Dictionary<int, int>();

            // Verifica se o produto já está no dicionário
            if (produtosNoCarrinho.ContainsKey(carrinho.ID))
            {
                // Se sim, incrementa a quantidade
                produtosNoCarrinho[carrinho.ID]++;
            }
            else
            {
                // Se não, adiciona o produto com quantidade 1
                produtosNoCarrinho.Add(carrinho.ID, 1);
            }

            // Percorre o dicionário e atualiza a quantidade no banco de dados
            foreach (var item in produtosNoCarrinho)
            {
                string sql = "update CARRINHO set QUANTIDADE = @quantidade where ID = @id";

                using (SqlConnection conn = new SqlConnection(con))
                {
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@quantidade", item.Value);
                        cmd.Parameters.AddWithValue("@id", item.Key);
                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

        /// <summary>
        /// Salva o histórico de compra do cliente usando a lista que foi trazida do controller.
        /// </summary>
        /// <param name="carrinho"></param>
        public void SalvarCarrinho(List<Carrinho> carrinho)
        {
            using (SqlConnection connection = new SqlConnection(con))
            {
                connection.Open();

                foreach (var item in carrinho)
                {
                    // Ajusta a data e hora para o fuso horário
                    DateTime dataHoraAjustada = item.DATA_COMPRA.ToUniversalTime().AddHours(-3); // Ajuste manual de -3 horas

                    // Insere os dados na tabela CARRINHO
                    using (SqlCommand insertCommand = new SqlCommand("INSERT INTO CARRINHO (NOME_PRODUTO, PRECO_PRODUTO, QUANTIDADE, IMAGEM_PRODUTO, ID_CLIENTE, DATA_COMPRA, ID_PRODUTO) VALUES (@NomeProduto, @PrecoProduto, @Quantidade, @ImagemProduto, @IdCliente, @DataCompra, @IdProduto)", connection))
                    {
                        insertCommand.Parameters.AddWithValue("@NomeProduto", item.NOME_PRODUTO);
                        insertCommand.Parameters.AddWithValue("@PrecoProduto", item.PRECO_PRODUTO);
                        insertCommand.Parameters.AddWithValue("@Quantidade", item.QUANTIDADE);
                        insertCommand.Parameters.AddWithValue("@ImagemProduto", item.IMAGEM_PRODUTO);
                        insertCommand.Parameters.AddWithValue("@IdCliente", item.ID_CLIENTE);
                        insertCommand.Parameters.AddWithValue("@DataCompra", dataHoraAjustada);
                        insertCommand.Parameters.AddWithValue("@IdProduto", item.ID_PRODUTO);

                        insertCommand.ExecuteNonQuery();
                    }
                }
            }
        }

    }
}