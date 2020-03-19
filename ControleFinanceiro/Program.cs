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
            ConsoleTable table;

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

                            ListarContas(p);

                            ReadLine();
                            Clear();

                            break;

                        case 2:
                            
                            Title = "NOVA CONTA - CONTROLE FINANCEIRO";
                            Uteis.MontaHeader("CADASTRAR NOVA CONTA");

                            CadastrarConta(p);

                            ReadLine();
                            Clear();

                            break;

                        case 3:
                            Title = "EDITAR CONTA - CONTROLE FINANCEIRO";
                            Uteis.MontaHeader("EDITAR CONTA");

                            ReadLine();
                            Clear();
                            break;

                        case 4:
                            Title = "EXCLUIR CONTA - CONTROLE FINANCEIRO";
                            Uteis.MontaHeader("EXCLUIR CONTA");

                            ReadLine();
                            Clear();
                            break;

                        case 5:
                            Title = "RELATÓRIO - CONTROLE FINANCEIRO";
                            Uteis.MontaHeader("RELATÓRIO");

                            Write("Data Inicial (dd/mm/yyyy): ");
                            string data_inicial = ReadLine();

                            Write("Data Final (dd/mm/yyyy): ");
                            string data_final = ReadLine();

                            ListarContas(p, data_inicial, data_final); 
                            
                            ReadLine();
                            Clear();
                            break;
                    }
                }

            } while (opc != 6);

        }
        static void ListarContas(Program p, string data_inicial = "", string data_final = "")
        {
            p.contas = p.conta.ListarTodos(data_inicial, data_final);

            ConsoleTable table;
            table = new ConsoleTable("ID", "Descrição", "Tipo", "Valor", "Data Vencimento");

            foreach (var c in p.contas)
            {
                table.AddRow(c.Id, c.Descricao, c.Tipo.Equals('R') ? "Receber" : "Pagar", string.Format("{0:c}", c.Valor), string.Format("{0:dd/MM/yyyy}", c.DataVencimento));
            }
            table.Write();
        }

        static void CadastrarConta(Program p)
        {
            ConsoleTable table;
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

            foreach (Categoria categoria in p.categorias)
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
        }
    }
}
