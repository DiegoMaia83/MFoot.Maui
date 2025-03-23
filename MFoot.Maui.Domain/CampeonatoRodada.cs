using MFoot.Maui.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFoot.Maui.Domain
{
    public class CampeonatoRodada
    {
        public int Id { get; set; }
        public int CampeonatoId { get; set; }
        public int Numero { get; set; }
        public int Turno { get; set; }
        public bool Concluida { get; set; }

        public List<CampeonatoPartida> Partidas { get; set; }
    }
}
