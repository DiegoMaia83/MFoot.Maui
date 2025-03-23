using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFoot.Maui.Domain
{
    public class Time
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public double Ataque { get; set; }
        public double Defesa { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }
        public string Pais { get; set; }
        public string CorPrimaria { get; set; }
        public string CorSecundaria { get; set; }
        public string CorTerciaria { get; set; }
        public int Divisao { get; set; }

        public List<Jogador> Jogadores { get; set; }
    }
}
