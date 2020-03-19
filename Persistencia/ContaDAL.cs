using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using Modelo;

namespace Persistencia
{
    class ContaDAL
    {
        private SqlConnection conn;
        private Categoria categoria;

        public ContaDAL(SqlConnection conn)
        {
            this.conn = conn;
            string strConn = Db.Conexao.GetStringConnection();
            this.categoria = new Categoria(new SqlConnection(strConn));
        }

        public List<Conta> ListarTodos()
        {
            List<Conta> contas = new List<Conta>();

            string SQL;
            SQL  = "SELECT CON.ID, CON.DESCRICAO, CON.VALOR, CON.TIPO, CON.DATA_VENCIMENTO, CAT.NOME, CAT.ID AS CATEGORIA_ID ";
            SQL += "FROM CONTAS CON";
            SQL += "INNER JOIN CATEGORIAS CAT ON CON.CATEGORIA_ID = CAT.ID; ";

            var cmd = new SqlCommand(SQL, this.conn);
            
            this.conn.Open();

            using (SqlDataReader dr = cmd.ExecuteReader())
            {
                while(dr.Read())
                {
                    Conta conta = new Conta()
                    {
                        Id        = Convert.ToInt32(dr["id"].ToString()),
                        Descricao = dr["id"].ToString(),
                        Tipo      = Convert.ToChar(dr["id"].ToString()),
                        Valor     = Convert.ToDouble(dr["id"].ToString())
                    };

                    int id_categoria = Convert.ToInt32(dr["id"].ToString());
                    conta.categoria  = this.categoria.GetCategoria(id_categoria);
                    contas.Add(conta);
                }
            }

            return contas;

        }

    }
}
