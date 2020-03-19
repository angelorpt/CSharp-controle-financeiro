using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using Modelo;

namespace Persistencia
{
    public class ContaDAL
    {
        private SqlConnection conn;
        private CategoriaDAL categoria;

        public ContaDAL(SqlConnection conn)
        {
            this.conn = conn;
            string strConn = Db.Conexao.GetStringConnection();
            this.categoria = new CategoriaDAL(new SqlConnection(strConn));
        }

        public List<Conta> ListarTodos(string data_inicial = "", string data_final = "")
        {
            List<Conta> contas = new List<Conta>();

            StringBuilder SQL = new StringBuilder("");

            SQL.Append(" SELECT CON.ID, CON.DESCRICAO, CON.VALOR, CON.TIPO, CON.DATA_VENCIMENTO, CAT.NOME, CAT.ID AS CATEGORIA_ID ");
            SQL.Append(" FROM CONTAS CON ");
            SQL.Append(" INNER JOIN CATEGORIAS CAT ON CON.CATEGORIA_ID = CAT.ID");

            if (!data_inicial.Equals("") && !data_final.Equals(""))
            {
                SQL.Append(" WHERE CON.DATA_VENCIMENTO BETWEEN @data_ini AND @data_fim");
            }

            var cmd = new SqlCommand(SQL.ToString(), this.conn);

            if (!data_inicial.Equals("") && !data_final.Equals(""))
            {
                cmd.Parameters.AddWithValue("@data_ini", data_inicial);
                cmd.Parameters.AddWithValue("@data_fim", data_final);
            }

            this.conn.Open();

            using (SqlDataReader dr = cmd.ExecuteReader())
            {
                while(dr.Read())
                {
                    Conta conta = new Conta()
                    {
                        Id             = Convert.ToInt32(dr["ID"].ToString()),
                        Descricao      = dr["DESCRICAO"].ToString(),
                        Tipo           = Convert.ToChar(dr["TIPO"].ToString()),
                        DataVencimento = DateTime.Parse(dr["DATA_VENCIMENTO"].ToString()),
                        Valor          = Convert.ToDouble(dr["VALOR"].ToString())
                    };

                    int id_categoria = Convert.ToInt32(dr["id"].ToString());
                    conta.categoria  = this.categoria.GetCategoria(id_categoria);
                    contas.Add(conta);
                }
            }
            
            this.conn.Close();

            return contas;

        }

        public void Salvar(Conta conta)
        {
            if(conta.Id == null)
            {
                Cadastrar(conta);
            } else
            {
                Editar(conta);
            }
        }

        void Cadastrar(Conta conta)
        {
            this.conn.Open();
            SqlCommand cmd = this.conn.CreateCommand();

            string SQL;
            SQL  = " INSERT INTO CONTAS (DESCRICAO, TIPO, VALOR, DATA_VENCIMENTO, CATEGORIA_ID) ";
            SQL += " VALUES (@descricao, @tipo, @valor, @data_vencimento, @categoria_id) ";
            cmd.CommandText = SQL;
            
            cmd.Parameters.AddWithValue("@descricao", conta.Descricao);
            cmd.Parameters.AddWithValue("@tipo", conta.Tipo);
            cmd.Parameters.AddWithValue("@valor", conta.Valor);
            cmd.Parameters.AddWithValue("@data_vencimento", conta.DataVencimento);
            cmd.Parameters.AddWithValue("@categoria_id", conta.categoria.Id);

            cmd.ExecuteNonQuery();

            this.conn.Close();
        }

        void Editar(Conta conta)
        {

        }

    }
}
