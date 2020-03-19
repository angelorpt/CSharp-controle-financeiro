using System;
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
    }
}
