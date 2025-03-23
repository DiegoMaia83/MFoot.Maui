using MFoot.Maui.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFoot.Maui.Domain
{
    public class CampeonatoPartida : INotifyPropertyChanged
    {
        private int placarCasa;
        private int placarVisitante;
        private string evento;
        private int eventoId;

        public int Id { get; set; }
        public int RodadaId { get; set; }
        public int TimeCasaId { get; set; }
        public int TimeVisitanteId { get; set; }
        public Time TimeCasa { get; set; }
        public Time TimeVisitante { get; set; }

        public List<CampeonatoPartidaEvento> Eventos { get; set; } = new List<CampeonatoPartidaEvento>();

        public int PlacarCasa
        {
            get => placarCasa;
            set
            {
                if (placarCasa != value)
                {
                    placarCasa = value;
                    OnPropertyChanged(nameof(PlacarCasa));
                }
            }
        }

        public int PlacarVisitante
        {
            get => placarVisitante;
            set
            {
                if (placarVisitante != value)
                {
                    placarVisitante = value;
                    OnPropertyChanged(nameof(PlacarVisitante));
                }
            }
        }

        public string Evento
        {
            get => evento;
            set
            {
                if (evento != value)
                {
                    evento = value;
                    OnPropertyChanged(nameof(Evento));
                    // Notifica que o EventoIcone também mudou
                    OnPropertyChanged(nameof(EventoIcone));
                }
            }
        }

        public int EventoId
        {
            get => eventoId;
            set
            {
                if (eventoId != value)
                {
                    eventoId = value;
                    // Quando EventoId mudar, notifique que EventoIcone mudou
                    OnPropertyChanged(nameof(EventoId));
                    OnPropertyChanged(nameof(EventoIcone));
                }
            }
        }

        public string EventoIcone
        {
            get
            {
                // Retorna o ícone correspondente ao evento atual
                switch (EventoId)
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

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
