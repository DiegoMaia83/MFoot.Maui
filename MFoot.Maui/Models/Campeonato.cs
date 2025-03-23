using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFoot.Maui.Domain
{
    public class Campeonato
    {
        public int Id { get; set; }
        public string Tipo { get; set; }
        public int TemporadaId { get; set; }
        public DateTime DataInicio { get; set; }
        public int TimesParticipantes { get; set; }
        public int TimesPromocao { get; set; }
        public int TimesRebaixamento { get; set; }
        public int Divisao { get; set; }

        public List<CampeonatoRodada> Rodadas { get; set; }
    }
}
