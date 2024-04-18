using Antlr.Runtime.Tree;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI.WebControls.WebParts;
using TrabalhoHowV.Models;
using TrabalhoHowV.Repositorios;

namespace Trabalho_HOW_V.Repositorio
{
    public class RepositorioDePessoas : RepositorioDeClientes<Clientes, int>
    {
        /// <summary>
        /// <param name="entity">Referência de Cliente que sera excluido</param>
        /// </summary>
        public override void Delete(Clientes entity)
        {
            using (var conn = new SqlConnection(con))
            {
                string sql = "delete cliente where id=@id";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", entity.id);
                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        ///<summary>Exclui uma pessoa pelo id
        ///<param name="id">id do registro que será excluído.</param>
        ///</summary>
        public override void DeleteById(int id)
        {
            using (var conn = new SqlConnection(con))
            {
                string sql = "delete cliente where id=@id";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", id);
                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        ///<summary>Obtém todas as pessoas
        ///<returns>Retorna as pessoas cadastradas.</returns>
        ///</summary>
        public override List<Clientes> GetAll()
        {
            string sql = "select id, nome_fisico_juridico, email, nome_endereco from cliente order by nome_fisico_juridico";
            using (var conn = new SqlConnection(con))
            {
                var cmd = new SqlCommand(sql, conn);
                List<Clientes> listaClientes = new List<Clientes>();
                Clientes objCllientes = null;
                try
                {
                    conn.Open();
                    using (var reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        while (reader.Read())
                        {
                            objCllientes = new Clientes();
                            objCllientes.id = (int)reader["id"];
                            objCllientes.NOMEPESSOAL = reader["nome_fisico_juridico"].ToString();
                            objCllientes.EMAIL = reader["email"].ToString();
                            objCllientes.ENDERECO = reader["nome_endereco"].ToString();
                            listaClientes.Add(objCllientes);
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return listaClientes;
            }
        }

        ///s<summary>
        ///<param name="id">Id do registro que foi pego.</param>
        ///<returns>Retorna uma referência de Clietne do registro encontrado ou null se ele não for encontrado.</returns>
        ///</summary>
        public override Clientes GetById(int id)
        {
            using (var conn = new SqlConnection(con))
            {
                string sql = "select id, nome_fisico_juridico, email, nome_endereco from cliente where id=@id";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", id);
                Clientes objClientes = null;
                try
                {
                    conn.Open();
                    using (var reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        if (reader.HasRows)
                        {
                            if (reader.Read())
                            {
                                objClientes = new Clientes();
                                objClientes.id = (int)reader["id"];
                                objClientes.NOMEPESSOAL = reader["nome_fisico_juridico"].ToString();
                                objClientes.EMAIL = reader["email"].ToString();
                                objClientes.CIDADDE = reader["nome_endereco"].ToString();
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    throw e;
                }
                return objClientes;
            }
        }

        ///<summary>Puxa um id pelo Email.
        ///<param name="email"/>Referência do usuario que será pego no banco de dados</param>
        /// </summary>
        public Clientes ConsultarPorEmail(string email)
        {
            using (var conn = new SqlConnection(con))
            {
                string sql = "select id from login where email=@email";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@email", email);
                Clientes objClientes = null;
                try
                {
                    conn.Open();
                    using (var reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        if (reader.HasRows)
                        {
                            if (reader.Read())
                            {
                                objClientes = new Clientes();
                                objClientes.id = (int)reader["id"];
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return objClientes;
            }
        }

        ///<summary>Salva um novo registro no banco.
        ///<param name="entity">Referência de Cliente que será salva.</param>
        ///</summary>
        public override void Save(Clientes entity)
        {
            using (var conn = new SqlConnection(con))
            {
                string sql = @"
                                SET IDENTITY_INSERT cliente ON;

                                INSERT INTO LOGIN (PRIMEIRO_NOME, ULTIMO_NOME, SENHA, EMAIL) 
                                VALUES (@PRIMEIRO_NOME, @ULTIMO_NOME, @SENHA, @EMAIL);

                                DECLARE @id_login INT;
                                SET @id_login = (SELECT MAX(ID) FROM login);

                                INSERT INTO cliente (id, primeiro_nome, ultimo_nome, senha, email) 
                                VALUES (@id_login, @primeiro_nome, @ultimo_nome, @senha, @email);

                                SET IDENTITY_INSERT cliente OFF;";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@PRIMEIRO_NOME", entity.NOMEPESSOAL);
                cmd.Parameters.AddWithValue("@ULTIMO_NOME", entity.ULTIMO_NOME);
                cmd.Parameters.AddWithValue("@SENHA", entity.SENHA);
                cmd.Parameters.AddWithValue("@EMAIL", entity.EMAIL);
                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        ///<summary>Atualiza o Cliente no banco de dados.
        ///<param name="entity">Referência de Cliente que será atualizado.</param>
        ///</summary>
        public override void Update(Clientes entity)
        {
            using (var conn = new SqlConnection(con))
            {
                string sql = "update cliente set nome_fisico_juridico=@NOMEPESSOAL, email=@EMAIL, nome_endereco=@CIDADDE where id=@id";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", entity.id);
                cmd.Parameters.AddWithValue("@NOMEPESSOAL", entity.NOMEPESSOAL);
                cmd.Parameters.AddWithValue("@EMAIL", entity.EMAIL);
                cmd.Parameters.AddWithValue("@CIDADDE", entity.CIDADDE);
                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public void Save(Clientes objClientes, Loginn objLogin)
        {
            using (var conn = new SqlConnection(con))
            {
                string sql = @"
                                SET IDENTITY_INSERT cliente ON;

                                INSERT INTO LOGIN (PRIMEIRO_NOME, ULTIMO_NOME, SENHA, EMAIL, USUARIO, NOME_COMPLETO, NUMERO_TELEFONE) 
                                VALUES (@PRIMEIRO_NOME_CLIENTE, @ULTIMO_NOME_CLIENTE, @SENHA_LOGIN, @EMAIL_LOGIN, @USUARIO_CLIENTE, @NOME_COMPLETO_LOGIN, @NUMERO_TELEFONE_LOGIN);

                                DECLARE @id_login INT;
                                SET @id_login = (SELECT MAX(ID) FROM login);

                                INSERT INTO cliente (id, primeiro_nome, ultimo_nome, email, nome_fisico_juridico, numero_telefone) 
                                VALUES (@id_login, @PRIMEIRO_NOME_CLIENTE, @ULTIMO_NOME_CLIENTE, @EMAIL_CLIENTE, @NOME_COMPLETO_CLIENTE, @NUMERO_TELEFONE_CLIENTE);

                                SET IDENTITY_INSERT cliente OFF;";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@PRIMEIRO_NOME_LOGIN", objLogin.PRIMEIRO_NOME);
                cmd.Parameters.AddWithValue("@ULTIMO_NOME_LOGIN", objLogin.ULTIMO_NOME);
                cmd.Parameters.AddWithValue("@SENHA_LOGIN", objLogin.SENHA);
                cmd.Parameters.AddWithValue("@EMAIL_LOGIN", objLogin.EMAIL);
                cmd.Parameters.AddWithValue("@USUARIO_CLIENTE", objLogin.PRIMEIRO_NOME);
                cmd.Parameters.AddWithValue("@NOME_COMPLETO_LOGIN", $"{objLogin.PRIMEIRO_NOME} {objLogin.ULTIMO_NOME}");
                cmd.Parameters.AddWithValue("@NUMERO_TELEFONE_LOGIN", objLogin.NUMERO_TELEFONE);

                cmd.Parameters.AddWithValue("@PRIMEIRO_NOME_CLIENTE", objClientes.PRIMEIRO_NOME);
                cmd.Parameters.AddWithValue("@ULTIMO_NOME_CLIENTE", objClientes.ULTIMO_NOME);
                cmd.Parameters.AddWithValue("@EMAIL_CLIENTE", objClientes.EMAIL);
                cmd.Parameters.AddWithValue("@NOME_COMPLETO_CLIENTE", $"{objClientes.PRIMEIRO_NOME} {objClientes.ULTIMO_NOME}");
                cmd.Parameters.AddWithValue("@NUMERO_TELEFONE_CLIENTE", objClientes.NUMERO_TELEFONE);

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        /// <summary>
        /// Faz uma pesquisa no banco de dados da tabela de LOGIN. Caso exista um campo com o mesmo valor do parâmetro, ele retorna como true. O parâmetro passado é o email.
        /// </summary>
        /// <param name="emailLogin"></param>
        /// <returns></returns>
        public bool ConsultaEmailLoginUsuario(string emailLogin)
        {
            //método para identificar se algum campo possui ou não os mesmo valores que serão salvos.
            bool consulta = false;

            try
            {
                using (var conn = new SqlConnection(con))
                {
                    //abre a conexão com o banco de dados.
                    conn.Open();
                    using (var cmd = new SqlCommand())
                    {
                        cmd.Connection = conn;
                        //select em forma de string para puxar os dados do banco de dados com base no que queremos salvar.
                        cmd.CommandText = string.Format("select count(*) from LOGIN where EMAIL='{0}'", emailLogin);
                        //converte o valor retornado no booleano que declaramos acima.
                        consulta = ((int)cmd.ExecuteScalar() > 0);
                    }
                }
            }
            //caso dê algum erro durante a execução da verificação, será exibido por esse "chatch".
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            //retorna o valor da consulta.
            return consulta;
        }

        /// <summary>
        /// Faz uma pesquisa no banco de dados da tabela de LOGIN. Caso exista um campo com o mesmo valor do parâmetro, ele retorna como true. O parâmetro passado é o numero de telefone.
        /// </summary>
        /// <param name="numeroTelefoneLogin"></param>
        /// <returns></returns>
        public bool ConsultaNumeroLoginUsuario(int numeroTelefoneLogin)
        {
            //método para identificar se algum campo possui ou não os mesmo valores que serão salvos.
            bool consulta = false;

            try
            {
                using (var conn = new SqlConnection(con))
                {
                    //abre a conexão com o banco de dados.
                    conn.Open();
                    using (var cmd = new SqlCommand())
                    {
                        cmd.Connection = conn;
                        //select em forma de string para puxar os dados do banco de dados com base no que queremos salvar.
                        cmd.CommandText = string.Format("select count(*) from LOGIN where NUMERO_TELEFONE='{0}'", Convert.ToInt32(numeroTelefoneLogin));
                        //converte o valor retornado no booleano que declaramos acima.
                        consulta = ((int)cmd.ExecuteScalar() > 0);
                    }
                }
            }
            //caso dê algum erro durante a execução da verificação, será exibido por esse "chatch".
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            //retorna o valor da consulta.
            return consulta;
        }

        /// <summary>
        /// Faz uma pesquisa no banco de dados da tabela de cliente. Caso exista um campo com o mesmo valor do parâmetro, ele retorna como true. O parâmetro passado é o email.
        /// </summary>
        /// <param name="emailCliente"></param>
        /// <returns></returns>
        public bool ConsultaEmailClienteUsuario(string emailCliente)
        {
            //método para identificar se algum campo possui ou não os mesmo valores que serão salvos.
            bool consulta = false;

            try
            {
                using (var conn = new SqlConnection(con))
                {
                    //abre a conexão com o banco de dados.
                    conn.Open();
                    using (var cmd = new SqlCommand())
                    {
                        cmd.Connection = conn;
                        //select em forma de string para puxar os dados do banco de dados com base no que queremos salvar.
                        cmd.CommandText = string.Format("select count(*) from cliente where email='{0}'", emailCliente);
                        //converte o valor retornado no booleano que declaramos acima.
                        consulta = ((int)cmd.ExecuteScalar() > 0);
                    }
                }
            }
            //caso dê algum erro durante a execução da verificação, será exibido por esse "chatch".
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            //retorna o valor da consulta.
            return consulta;
        }

        /// <summary>
        /// Faz uma pesquisa no banco de dados da tabela de cliente. Caso exista um campo com o mesmo valor do parâmetro, ele retorna como true. O parâmetro passado é o numero de telefone.
        /// </summary>
        /// <param name="numeroTelefoneClientes"></param>
        /// <returns></returns>
        public bool ConsultaNumeroClienteUsuario(int numeroTelefoneClientes)
        {
            //método para identificar se algum campo possui ou não os mesmo valores que serão salvos.
            bool consulta = false;

            try
            {
                using (var conn = new SqlConnection(con))
                {
                    //abre a conexão com o banco de dados.
                    conn.Open();
                    using (var cmd = new SqlCommand())
                    {
                        cmd.Connection = conn;
                        //select em forma de string para puxar os dados do banco de dados com base no que queremos salvar.
                        cmd.CommandText = string.Format("select count(*) from cliente where numero_telefone='{0}'", Convert.ToString(numeroTelefoneClientes));
                        //converte o valor retornado no booleano que declaramos acima.
                        consulta = ((int)cmd.ExecuteScalar() > 0);
                    }
                }
            }
            //caso dê algum erro durante a execução da verificação, será exibido por esse "chatch".
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            //retorna o valor da consulta.
            return consulta;
        }
    }
}