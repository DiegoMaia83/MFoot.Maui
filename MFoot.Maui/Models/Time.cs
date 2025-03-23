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
        public string Estadio { get; set; }
        public string Capacidade { get; set; }
        public double Ataque { get; set; }
        public double Defesa { get; set; }
        public int Divisao { get; set; }

        public double AtaqueTitular { get; set; }
        public double DefesaTitular { get; set; }

        public List<Jogador> JogadoresTitulares { get; set; }
        public List<Jogador> JogadoresReservas { get; set; }
        public List<Jogador> Jogadores { get; set; }
    }
}
