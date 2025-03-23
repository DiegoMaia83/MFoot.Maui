using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFoot.Maui.Models
{
    public class JogadorCartao
    {
        public int Id { get; set; }
        public int TipoId { get; set; }
        public int CampeonatoId { get; set; }
        public int TimeId { get; set; }
        public int JogadorId { get; set; }
        public bool Concluido { get; set; }
    }
}
