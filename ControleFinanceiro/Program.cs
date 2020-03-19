using System;
using static System.Console;
using System.Collections.Generic;

using Modelo;
using Persistencia;
using System.Data.SqlClient;

using ConsoleTables;

namespace ControleFinanceiro
{
    class Program
    {

        private List<Conta> contas;
        private List<Categoria> categorias;

        private ContaDAL conta;
        private CategoriaDAL categoria;

        public Program()
        {
            string strConn = Db.Conexao.GetStringConnection();

            this.conta     = new ContaDAL(new SqlConnection(strConn));
            this.categoria = new CategoriaDAL(new SqlConnection(strConn));
        }

        static void Main(string[] args)
        {

            int opc;

            Program p = new Program();

            do
            {
                Title = "Controle Financeiro";
                Uteis.MontaMenu();

                opc = Convert.ToInt32(ReadLine());

                if ( !(opc >= 1 && opc <= 6) )
                {
                    Clear();
                    BackgroundColor = ConsoleColor.Red;
                    ForegroundColor = ConsoleColor.White;
                    Uteis.MontaHeader("INFORME OPÇÃO VÁLIDA!", 'X', 40);
                    ResetColor();
                } else
                {
                    Clear();
                    switch(opc)
                    {
                        case 1:

                            Title = "LISTAGEM DE CONTAS - CONTROLE FINANCEIRO";
                            Uteis.MontaHeader("LISTAGEM DE CONTAS");

                            p.contas = p.conta.ListarTodos();

                            ConsoleTable table = new ConsoleTable("ID", "Descrição", "Tipo", "Valor");

                            foreach (var conta in p.contas)
                            {
                                table.AddRow(conta.Id, conta.Descricao, conta.Tipo.Equals('R') ? "Receber" : "Pagar", string.Format("{0:c}", conta.Valor));
                            }
                            table.Write();
                            ReadLine();
                            Clear();

                            break;

                        case 2:
                            Write("Cadastrar");
                            break;

                        case 3:
                            Write("Editar");
                            break;

                        case 4:
                            Write("Excluir");
                            break;

                        case 5:
                            Write("Relatório");
                            break;
                    }
                }

            } while (opc != 6);

        }
    }
}
