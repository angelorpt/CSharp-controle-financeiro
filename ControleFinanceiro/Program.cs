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

                            foreach (var c in p.contas)
                            {
                                table.AddRow(c.Id, c.Descricao, c.Tipo.Equals('R') ? "Receber" : "Pagar", string.Format("{0:c}", c.Valor));
                            }
                            table.Write();
                            ReadLine();
                            Clear();

                            break;

                        case 2:
                            
                            Title = "NOVA CONTA - CONTROLE FINANCEIRO";
                            Uteis.MontaHeader("CADASTRAR NOVA CONTA");

                            string desc = "";
                            do
                            {
                                Write("Informe a descrição da conta: ");
                                desc = ReadLine();

                                if (desc.Equals(""))
                                {
                                    BackgroundColor = ConsoleColor.Red;
                                    ForegroundColor = ConsoleColor.White;
                                    Uteis.MontaHeader("INFORME UMA DESCRIÇÃO", '*', 30);
                                    ResetColor();
                                }

                            } while (desc.Equals(""));

                            Write("Valor: ");
                            double valor = Convert.ToDouble(ReadLine());

                            Write("Tipo: ");
                            char tipo = Convert.ToChar(ReadLine());

                            Write("Data Vencimento (dd/mm/aaaa): ");
                            DateTime dataVencimento = DateTime.Parse(ReadLine());

                            Write("Selecione uma categoria pleo ID: \n");
                            p.categorias = p.categoria.ListarTodos();

                            table = new ConsoleTable("ID", "NOME");
                            
                            foreach(Categoria categoria in p.categorias)
                            {
                                table.AddRow(categoria.Id, categoria.Nome);
                            }
                            table.Write();

                            Write("Categoria: ");
                            int categoria_id = Convert.ToInt32(ReadLine());

                            Categoria categoria_cad = p.categoria.GetCategoria(categoria_id);

                            Conta conta = new Conta()
                            {
                                Descricao = desc,
                                Valor = valor,
                                Tipo = tipo,
                                DataVencimento = dataVencimento,
                                categoria = categoria_cad,
                            };

                            p.conta.Salvar(conta);

                            BackgroundColor = ConsoleColor.Green;
                            ForegroundColor = ConsoleColor.White;
                            Uteis.MontaHeader("CONTA SALVA COM SUCESSO", '+', 30);
                            ResetColor();

                            ReadLine();
                            Clear();

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
