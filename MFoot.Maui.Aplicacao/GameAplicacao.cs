using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFoot.Maui.Aplicacao
{
    public class GameAplicacao
    {
        private readonly BaseAplicacao _baseAplicacao = new BaseAplicacao();
        private readonly TimeAplicacao _timeAplicacao = new TimeAplicacao();
        private readonly JogadorAplicacao _jogadorAplicacao = new JogadorAplicacao();

        public void IniciarJogo()
        {
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string dbDirectory = Path.Combine(appDataPath, "MFoot");

            // Certifique-se de que o diretório existe
            if (!Directory.Exists(dbDirectory))
            {
                Directory.CreateDirectory(dbDirectory);
            }

            string dbPath = Path.Combine(dbDirectory, "m_foot.db");

            if (!File.Exists(dbPath))
            {
                using (FileStream fs = File.Create(dbPath))
                {
                    // Create the file and close the stream
                }                
            }


            /*
            _timeAplicacao.InserirTabelaTime();

            var timesBase = _baseAplicacao.ListarTimesBase();

            foreach (var time in timesBase)
            {
                //_timeAplicacao.InserirJogador(jogador);
            }
            */




            _jogadorAplicacao.InserirTabelaJogador();

            var jogadoresBase = _baseAplicacao.ListarJogadoresBase();

            foreach (var jogador in jogadoresBase)
            {
                _jogadorAplicacao.InserirJogador(jogador);
            }            
        }
    }
}
