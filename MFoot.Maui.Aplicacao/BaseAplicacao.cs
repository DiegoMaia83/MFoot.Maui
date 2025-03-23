using MFoot.Maui.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFoot.Maui.Aplicacao
{
    public class BaseAplicacao
    {
        public IEnumerable<Time> ListarTimesBase()
        {
            var listaTimes = new List<Time>();

            listaTimes.Add(new Time { 
                Id = 1,
                Nome = "Corinthians",
                Ataque = ListarJogadoresBase().Where(x => x.TimeId == 1).Sum(x => x.Ataque),
                Defesa = ListarJogadoresBase().Where(x => x.TimeId == 1).Sum(x => x.Defesa),
                Cidade = "São Paulo",
                Estado = "SP",
                Pais = "Brasil",
                CorPrimaria = "#000000",
                CorSecundaria = "#000000",
                CorTerciaria = "#000000"
            });

            listaTimes.Add(new Time
            {
                Id = 2,
                Nome = "Flamengo",
                Ataque = ListarJogadoresBase().Where(x => x.TimeId == 2).Sum(x => x.Ataque),
                Defesa = ListarJogadoresBase().Where(x => x.TimeId == 2).Sum(x => x.Defesa),
                Cidade = "Rio de Janeiro",
                Estado = "RJ",
                Pais = "Brasil",
                CorPrimaria = "#000000",
                CorSecundaria = "#000000",
                CorTerciaria = "#000000"
            });

            return listaTimes;
        }

        public IEnumerable<Jogador> ListarJogadoresBase()
        {
            var listaJogadores = new List<Jogador>();

            listaJogadores.Add(new Jogador { Id = 1, Nome = "Hugo Souza", Ataque = 25, Defesa = 75, Finalizacao = 33, Zona = "G", TimeId = 1 });
            listaJogadores.Add(new Jogador { Id = 2, Nome = "Fagner", Ataque = 40, Defesa = 70, Finalizacao = 44, Zona = "D", TimeId = 1 });
            listaJogadores.Add(new Jogador { Id = 3, Nome = "André Ramalho", Ataque = 38, Defesa = 77, Finalizacao = 40, Zona = "D", TimeId = 1 });
            listaJogadores.Add(new Jogador { Id = 4, Nome = "Cacá", Ataque = 32, Defesa = 66, Finalizacao = 38, Zona = "D", TimeId = 1 });
            listaJogadores.Add(new Jogador { Id = 5, Nome = "Hugo", Ataque = 34, Defesa = 64, Finalizacao = 29, Zona = "D", TimeId = 1 });
            listaJogadores.Add(new Jogador { Id = 6, Nome = "Raniele", Ataque = 44, Defesa = 50, Finalizacao = 52, Zona = "V", TimeId = 1 });
            listaJogadores.Add(new Jogador { Id = 7, Nome = "Alex Teixeira", Ataque = 46, Defesa = 45, Finalizacao = 54, Zona = "V", TimeId = 1 });
            listaJogadores.Add(new Jogador { Id = 8, Nome = "Igor Coronado", Ataque = 69, Defesa = 24, Finalizacao = 72, Zona = "M", TimeId = 1 });
            listaJogadores.Add(new Jogador { Id = 9, Nome = "R. Garro", Ataque = 80, Defesa = 26, Finalizacao = 83, Zona = "M", TimeId = 1 });
            listaJogadores.Add(new Jogador { Id = 10, Nome = "Romero", Ataque = 78, Defesa = 30, Finalizacao = 72, Zona = "A", TimeId = 1 });
            listaJogadores.Add(new Jogador { Id = 11, Nome = "Yuri Alberto", Ataque = 80, Defesa = 28, Finalizacao = 77, Zona = "A", TimeId = 1 });

            listaJogadores.Add(new Jogador { Id = 12, Nome = "Rossi", Ataque = 25, Defesa = 77, Finalizacao = 33, Zona = "G", TimeId = 2 });
            listaJogadores.Add(new Jogador { Id = 13, Nome = "Varela", Ataque = 40, Defesa = 70, Finalizacao = 44, Zona = "D", TimeId = 2 });
            listaJogadores.Add(new Jogador { Id = 14, Nome = "Fabrício Bruno", Ataque = 38, Defesa = 77, Finalizacao = 40, Zona = "D", TimeId = 2 });
            listaJogadores.Add(new Jogador { Id = 15, Nome = "Léo Pereira", Ataque = 32, Defesa = 80, Finalizacao = 38, Zona = "D", TimeId = 2 });
            listaJogadores.Add(new Jogador { Id = 16, Nome = "Viña", Ataque = 38, Defesa = 64, Finalizacao = 38, Zona = "D", TimeId = 2 });
            listaJogadores.Add(new Jogador { Id = 17, Nome = "Allan", Ataque = 47, Defesa = 44, Finalizacao = 52, Zona = "V", TimeId = 2 });
            listaJogadores.Add(new Jogador { Id = 18, Nome = "Gerson", Ataque = 77, Defesa = 43, Finalizacao = 68, Zona = "V", TimeId = 2 });
            listaJogadores.Add(new Jogador { Id = 19, Nome = "De la Cruz", Ataque = 82, Defesa = 29, Finalizacao = 80, Zona = "M", TimeId = 2 });
            listaJogadores.Add(new Jogador { Id = 20, Nome = "Arrascaeta", Ataque = 84, Defesa = 28, Finalizacao = 83, Zona = "M", TimeId = 2 });
            listaJogadores.Add(new Jogador { Id = 21, Nome = "Everton Cebolinha", Ataque = 78, Defesa = 25, Finalizacao = 75, Zona = "A", TimeId = 2 });
            listaJogadores.Add(new Jogador { Id = 22, Nome = "Pedro", Ataque = 82, Defesa = 28, Finalizacao = 86, Zona = "A", TimeId = 2 });

            return listaJogadores;
        }
    }
}
