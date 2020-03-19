using System;
using System.Data.SqlClient;
using ModelCategoria = Modelo.Categoria;

namespace Persistencia
{
    class Categoria
    {
        private SqlConnection conn;

        public Categoria (SqlConnection conn)
        {
            this.conn = conn;
        }

        public ModelCategoria GetCategoria(int id)
        {
            ModelCategoria categoria = new ModelCategoria();

            string SQL;
            SQL = "SELECT ID, NOME FROM CATEGORIA WHERE ID = @id";
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
