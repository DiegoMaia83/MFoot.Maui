using MFoot.Maui.Dal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFoot.Maui.Aplicacao
{
    public class TimeAplicacao
    {
        private ConexaoSqLite _conexaoSqLite;

        public TimeAplicacao()
        {
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string dbDirectory = Path.Combine(appDataPath, "MFoot");

            string dbPath = Path.Combine(dbDirectory, "m_foot.db");

            _conexaoSqLite = new ConexaoSqLite(dbPath);
        }

        public void InserirTabelaTime()
        {
            var sql = new StringBuilder();
            sql.Append(" CREATE TABLE IF NOT EXISTS times ( ");
            sql.Append(" Id INTEGER PRIMARY KEY AUTOINCREMENT, ");
            sql.Append(" Nome TEXT NOT NULL, ");
            sql.Append(" Ataque REAL, ");
            sql.Append(" Defesa REAL, ");
            sql.Append(" Cidade TEXT NOT NULL, ");
            sql.Append(" Estado TEXT NOT NULL, ");
            sql.Append(" Pais TEXT NOT NULL, ");
            sql.Append(" CorPrimaria TEXT NOT NULL, ");
            sql.Append(" CorSecundaria TEXT NOT NULL, ");
            sql.Append(" CorTerciaria TEXT NOT NULL ");            
            sql.Append(" );");

            _conexaoSqLite.ExecuteNonQuery(sql.ToString());
        }
    }
}
