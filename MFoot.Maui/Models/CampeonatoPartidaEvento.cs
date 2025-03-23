using MFoot.Maui.Domain;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFoot.Maui.Models
{
    public class CampeonatoPartidaEvento
    {
        public int Id { get; set; }
        public int TipoId { get; set; }
        public int PartidaId { get; set; }
        public int TimeId { get; set; }
        public int JogadorId { get; set; }
        public int Tempo { get; set; }

        public string EventoIcone
        {
            get
            {
                // Retorna o ícone correspondente ao evento atual
                switch (TipoId)
                {
                    case 1:
                        return "goal.png"; // Caminho da imagem para gol do time da casa
                    case 2:
                        return "yellow_card.png"; // Caminho da imagem para cartão amarelo
                    case 3:
                        return "red_card.png"; // Caminho da imagem para cartão amarelo                                          
                    default:
                        return ""; // Caminho da imagem padrão
                }
            }
        }

        public Jogador Jogador { get; set; }
    }
}
