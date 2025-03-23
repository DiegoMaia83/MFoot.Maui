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

        public int Id { get; set; }
        public int RodadaId { get; set; }
        public int TimeCasaId { get; set; }
        public int TimeVisitanteId { get; set; }
        public Time TimeCasa { get; set; }
        public Time TimeVisitante { get; set; }

        public List<string> Eventos { get; set; } = new List<string>();

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
