using System;

namespace Db
{
    class Conexao
    {
        private static readonly string server   = "LAPTOP-B2ADTHUP\\SQLEXPRESS";
        private static readonly string database = "SoN_Financeiro";
        private static readonly string user     = "sa";
        private static readonly string password = "root";

        public static string GetStringConnection() => $"Server={server};Database={database};User Id={user};Password={password}";
    }
}
