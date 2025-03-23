using MFoot.Maui.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFoot.Maui.Domain
{
    public class Jogador
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public double Ataque { get; set; }
        public double Defesa { get; set; }
        public double Finalizacao { get; set; }
        public double Resistencia { get; set; }
        public string Posicao { get; set; }
        public string Zona { get; set; }
        public DateTime DataNascimento { get; set; }
        public int Idade { get; set; }
        public int TimeId { get; set; }
        public double Valor { get; set; }
        public bool Titular { get; set; }

        public List<JogadorCartao> Cartoes { get; set; }
    }
}
