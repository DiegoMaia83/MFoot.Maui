using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFoot.Maui.Domain
{
    public class CampeonatoClassificacao
    {
        public int Id { get; set; }
        public int CampeonatoId { get; set; }
        public int TimeId { get; set; }
        public int Jogos { get; set; }
        public int Pontos { get; set; }
        public int Vitoria { get; set; }
        public int Empate { get; set; }
        public int Derrota { get; set; }
        public int GolFavor { get; set; }
        public int GolContra { get; set; }
        public int GolSaldo { get; set; }
        public int CartaoAmarelo { get; set; }
        public int CartaoVermelho { get; set; }

        public Time Time { get; set; }
    }
}
