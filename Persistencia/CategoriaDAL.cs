using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Modelo;

namespace Persistencia
{
    public class CategoriaDAL
    {
        private SqlConnection conn;

        public CategoriaDAL (SqlConnection conn)
        {
            this.conn = conn;
        }

        public Categoria GetCategoria(int id)
        {
            Categoria categoria = new Categoria();

            string SQL;
            SQL = "SELECT ID, NOME FROM CATEGORIAS WHERE ID = @id";
            var cmd = new SqlCommand(SQL, this.conn);
            cmd.Parameters.AddWithValue("@id", id);

            this.conn.Open();

            using (SqlDataReader dr = cmd.ExecuteReader())
            {
                dr.Read();
                categoria.Id   = Convert.ToInt32(dr["id"].ToString());
                categoria.Nome = dr["nome"].ToString();
            }

            this.conn.Close();
            return categoria;
        }

        public List<Categoria> ListarTodos()
        {
            List<Categoria> categorias = new List<Categoria>();

            string SQL = "SELECT ID, NOME FROM CATEGORIAS";
            SqlCommand cmd = new SqlCommand(SQL, this.conn);

            this.conn.Open();

            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                Categoria categoria = new Categoria()
                {
                    Id   = Convert.ToInt32(dr["id"].ToString()),
                    Nome = dr["nome"].ToString()
                };

                categorias.Add(categoria);

            }

            this.conn.Close();

            return categorias;
        }
    }
}
