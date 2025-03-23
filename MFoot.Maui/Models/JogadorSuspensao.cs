using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFoot.Maui.Models
{
    public class JogadorSuspensao
    {
        public int Id { get; set; }
        public int CampeonatoId { get; set; }
        public int TimeId { get; set; }
        public int JogadorId { get; set; }
        public int QuantidadeJogos { get; set; }
        public bool Concluido { get; set; }
    }
}
