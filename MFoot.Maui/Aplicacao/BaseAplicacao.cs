using MFoot.Maui.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MFoot.Maui.Aplicacao
{
    public class BaseAplicacao
    {



        public class JogadorJson
        {
            public string Nome { get; set; }
            public string Posicao { get; set; }
            public string PosicaoConvertida { get; set; }
            public string Valor { get; set; }
            public string ValorConvertido { get; set; }
            public string DataNascimento { get; set; }
            public DateTime DataNascimentoConvertida { get; set; }
            public string Numero { get; set; }
            public double Ataque { get; set; }
            public double Defesa { get; set; }
            public bool Titular { get; set; }
        }

        public class TimeJson
        {
            public int Id { get; set; }
            public string Nome { get; set; }
            public string Estadio { get; set; }
            public string Capacidade { get; set; }
            public List<JogadorJson> Jogadores { get; set; }
        }

        public static double EqualizarValorAtaque(string posicao)
        {
            try
            {
                Dictionary<string, double> dic = new Dictionary<string, double>()
            {
                { "GOL", 0.3 },
                { "ZAG", 0.3 },
                { "LE", 0.4 },
                { "LD", 0.4 },
                { "VOL", 0.5 },
                { "ME", 0.9 },
                { "SA", 1.0 },
                { "CA", 1.0 }
            };

                return dic[posicao];
            }
            catch
            {
                throw;
            }
        }

        public static double GerarValorAtaque(int input, string posicao)
        {
            // Limites
            int minInput = 100000;
            int maxInput = 15000000;
            double minOutput = 60.00;
            double maxOutput = 90.00;
            double rangeRandom = 3.00;

            // Verifica se o input está dentro do intervalo esperado
            if (input < minInput)
                return Math.Round(RetornarDoubleAleatorio(minOutput, rangeRandom) * EqualizarValorAtaque(posicao), 1);

            if (input > maxInput)
                return Math.Round(RetornarDoubleAleatorio(maxOutput, rangeRandom) * EqualizarValorAtaque(posicao), 1);

            // Interpolação linear para converter o valor de entrada
            double result = minOutput + (double)(input - minInput) / (maxInput - minInput) * (maxOutput - minOutput);
            return Math.Round(RetornarDoubleAleatorio(result, rangeRandom) * EqualizarValorAtaque(posicao), 1);
        }

        public static double EqualizarValorDefesa(string posicao)
        {
            try
            {
                Dictionary<string, double> dic = new Dictionary<string, double>()
            {
                { "GOL", 1.0 },
                { "ZAG", 1.0 },
                { "LE", 0.9 },
                { "LD", 0.9 },
                { "VOL", 0.8 },
                { "ME", 0.4 },
                { "SA", 0.3 },
                { "CA", 0.3 }
            };

                return dic[posicao];
            }
            catch
            {
                throw;
            }
        }

        public static double GerarValorDefesa(int input, string posicao)
        {
            // Limites
            int minInput = 100000;
            int maxInput = 15000000;
            double minOutput = 60.00;
            double maxOutput = 90.00;
            double rangeRandom = 3.00;

            // Verifica se o input está dentro do intervalo esperado
            if (input < minInput)
                return Math.Round(RetornarDoubleAleatorio(minOutput, rangeRandom) * EqualizarValorDefesa(posicao), 1);

            if (input > maxInput)
                return Math.Round(RetornarDoubleAleatorio(maxOutput, rangeRandom) * EqualizarValorDefesa(posicao), 1);

            // Interpolação linear para converter o valor de entrada
            double result = minOutput + (double)(input - minInput) / (maxInput - minInput) * (maxOutput - minOutput);
            return Math.Round(RetornarDoubleAleatorio(result, rangeRandom) * EqualizarValorDefesa(posicao), 1);
        }

        public static double RetornarDoubleAleatorio(double baseValue, double range)
        {
            Random random = new Random();

            // Calcula os limites inferior e superior
            double minValue = baseValue - range;
            double maxValue = baseValue + range;

            // Gera um valor double aleatório no intervalo [minValue, maxValue]
            double randomValue = random.NextDouble() * (maxValue - minValue) + minValue;
            return randomValue;
        }

        public static string ConverterPosicao(string posicao)
        {
            try
            {
                Dictionary<string, string> dic = new Dictionary<string, string>()
            {
                { "Goleiro", "GOL" },
                { "Zagueiro", "ZAG" },
                { "Lateral Esq.", "LE" },
                { "Lateral Dir.", "LD" },
                { "Volante", "VOL" },
                { "Meia Central", "VOL" },
                { "Meia Direita", "ME" },
                { "Meia Esquerda", "ME" },
                { "Meia Ofensivo", "ME" },
                { "Ponta Esquerda", "SA" },
                { "Ponta Direita", "SA" },
                { "Seg. Atacante", "SA" },
                { "Centroavante", "CA" }
            };

                return dic[posicao];
            }
            catch
            {
                throw;
            }
        }

        public static string ConverterZona(string posicao)
        {
            try
            {
                Dictionary<string, string> dic = new Dictionary<string, string>()
            {
                { "Goleiro", "G" },
                { "Zagueiro", "D" },
                { "Lateral Esq.", "D" },
                { "Lateral Dir.", "D" },
                { "Volante", "M" },
                { "Meia Central", "M" },
                { "Meia Direita", "M" },
                { "Meia Esquerda", "M" },
                { "Meia Ofensivo", "M" },
                { "Ponta Esquerda", "A" },
                { "Ponta Direita", "A" },
                { "Seg. Atacante", "A" },
                { "Centroavante", "A" }
            };

                return dic[posicao];
            }
            catch
            {
                throw;
            }
        }

        public static string ConverterValor(string valor)
        {
            try
            {
                var valorStr = "";

                if (valor.EndsWith("mi. €"))
                {
                    valorStr = valor.Replace(" mi. €", "0000").Replace(",", "");

                    // 2,00
                    // 2000000
                }
                else if (valor.EndsWith("mil €"))
                {
                    valorStr = valor.Replace(" mil €", "000").Replace(",", ""); ;
                }
                else
                {
                    valorStr = "200000";
                }

                return valorStr;

            }
            catch
            {
                throw;
            }
        }

        public static DateTime ConverterDataNascimento(string data)
        {
            try
            {
                var dt = data.Split('/');

                if (!System.String.IsNullOrEmpty(data))
                {
                    return new DateTime(Convert.ToInt32(dt[2]), Convert.ToInt32(dt[1]), Convert.ToInt32(dt[0]));
                }
                else
                {
                    return new DateTime(1994, 8, 1);
                }

            }
            catch
            {
                throw;
            }
        }



        public IEnumerable<Time> ListarTimesBase()
        {
            try
            {
                var listaTimes = new List<Time>();

                Dictionary<int, string> dict = new Dictionary<int, string>
                {
                    { 1, "jogadores_serie_a" },
                    { 2, "jogadores_serie_b" },
                    { 3, "jogadores_serie_c" }
                };

                foreach (var file in dict)
                {
                    string caminhoArquivo = $"C:\\Users\\diego\\Desktop\\Scripts\\GetPlayers\\GetPlayers\\JSON\\{file.Value}.json";                

                    if (File.Exists(caminhoArquivo))
                    {
                        // Lê o conteúdo do arquivo JSON
                        string json = File.ReadAllText(caminhoArquivo);

                        // Deserializa o JSON para uma lista de objetos Time
                        List<TimeJson> data = JsonSerializer.Deserialize<List<TimeJson>>(json);

                        foreach (var time in data)
                        {
                            var t = new Time();
                            t.Id = time.Id;
                            t.Nome = time.Nome;
                            t.Estadio = time.Estadio;
                            t.Capacidade = time.Capacidade;
                            t.Divisao = file.Key;

                            listaTimes.Add(t);
                        }
                    }
                }

                return listaTimes;
            }
            catch
            {
                throw;
            }
        }

        public IEnumerable<Jogador> ListarJogadoresBase()
        {
            try
            {
                var listaJogadores = new List<Jogador>();

                Dictionary<int, string> dict = new Dictionary<int, string>
                {
                    { 1, "jogadores_serie_a" },
                    { 2, "jogadores_serie_b" }
                    
                };

                foreach (var file in dict)
                {
                    string caminhoArquivo = $"C:\\Users\\diego\\Desktop\\Scripts\\GetPlayers\\GetPlayers\\JSON\\{file.Value}.json";
                    //string caminhoArquivo = $"D:\\Desenvolvimento\\Jogos\\MFoot\\MFoot.Maui\\MFoot.Maui\\Data\\Json\\{file.Value}.json";

                    if (File.Exists(caminhoArquivo))
                    {
                        // Lê o conteúdo do arquivo JSON
                        string json = File.ReadAllText(caminhoArquivo);

                        // Deserializa o JSON para uma lista de objetos Time
                        List<TimeJson> data = JsonSerializer.Deserialize<List<TimeJson>>(json);

                        foreach (var time in data)
                        {
                            var listaJogadoreTime = new List<Jogador>();

                            foreach (var jogador in time.Jogadores)
                            {
                                var valorConvertido = ConverterValor(jogador.Valor);

                                var j = new Jogador();
                                j.Nome = jogador.Nome;
                                j.Posicao = ConverterPosicao(jogador.Posicao);
                                j.Zona = ConverterZona(jogador.Posicao);
                                j.Ataque = GerarValorAtaque(Convert.ToInt32(valorConvertido), j.Posicao);
                                j.Finalizacao = j.Ataque;
                                j.Resistencia = 100.00;
                                j.Defesa = GerarValorDefesa(Convert.ToInt32(valorConvertido), j.Posicao);
                                j.TimeId = time.Id;
                                j.Titular = false;
                                j.Valor = Convert.ToDouble(valorConvertido);
                                j.DataNascimento = ConverterDataNascimento(jogador.DataNascimento);

                                /*
                                j.ValorConvertido = ConverterValor(jogador.Valor);
                                j.DataNascimento = jogador.DataNascimento;
                                j.DataNascimentoConvertida = ConverterDataNascimento(j.DataNascimento);
                                j.Numero = jogador.Numero;
                                */

                                listaJogadoreTime.Add(j);
                            }

                            var jogadoresTitular = new List<Jogador>();

                            var goleiroTitular = listaJogadoreTime.Where(j => j.Posicao == "GOL").OrderByDescending(j => j.Valor).FirstOrDefault();
                            var zagueirosTitulares = listaJogadoreTime.Where(j => j.Posicao == "ZAG").OrderByDescending(j => j.Valor).Take(2).ToList();
                            var lateralDireito = listaJogadoreTime.Where(j => j.Posicao == "LD").OrderByDescending(j => j.Valor).FirstOrDefault();
                            var lateralEsquerdo = listaJogadoreTime.Where(j => j.Posicao == "LE").OrderByDescending(j => j.Valor).FirstOrDefault();
                            var segundoAtacante = listaJogadoreTime.Where(j => j.Posicao == "SA").OrderByDescending(j => j.Valor).FirstOrDefault();
                            var centroAvante = listaJogadoreTime.Where(j => j.Posicao == "CA").OrderByDescending(j => j.Valor).FirstOrDefault();
                            
                            var meiasTitulares = listaJogadoreTime.Where(j => j.Posicao == "ME").OrderByDescending(j => j.Valor).Take(2).ToList();

                            var qtdVolantes = 4 - meiasTitulares.Count;
                            var volantesTitulares = listaJogadoreTime.Where(j => j.Posicao == "VOL").OrderByDescending(j => j.Valor).Take(qtdVolantes).ToList();

                            jogadoresTitular.Add(goleiroTitular);
                            jogadoresTitular.AddRange(zagueirosTitulares);
                            jogadoresTitular.Add(lateralDireito);
                            jogadoresTitular.Add(lateralEsquerdo);
                            jogadoresTitular.AddRange(volantesTitulares);
                            jogadoresTitular.AddRange(meiasTitulares);
                            jogadoresTitular.Add(segundoAtacante);
                            jogadoresTitular.Add(centroAvante);

                            foreach (var jogadorTitular in jogadoresTitular)
                            {
                                jogadorTitular.Titular = true;
                            }

                            listaJogadores = listaJogadores.Concat(listaJogadoreTime).ToList();
                        }
                    }

                }


                return listaJogadores;
            }
            catch
            {
                throw;
            }
        }


        /*
        public IEnumerable<Time> ListarTimesBase()
        {
            var listaTimes = new List<Time>();
                listaTimes.Add(new Time { Id = 1, Nome = "Corinthians", Cidade = "São Paulo", Estado = "SP", Pais = "Brasil", CorPrimaria = "#000000", CorSecundaria = "#FFFFFF", CorTerciaria = "#363636", Divisao = 1 });
                listaTimes.Add(new Time { Id = 2, Nome = "Flamengo", Cidade = "Rio de Janeiro", Estado = "RJ", Pais = "Brasil", CorPrimaria = "#FF0000", CorSecundaria = "#000000", CorTerciaria = "#FF0000", Divisao = 1 });
                listaTimes.Add(new Time { Id = 3, Nome = "Cruzeiro", Cidade = "Belo Horizonte", Estado = "MG", Pais = "Brasil", CorPrimaria = "#0000FF", CorSecundaria = "#FFFFFF", CorTerciaria = "#0000CD", Divisao = 1 });
                listaTimes.Add(new Time { Id = 4, Nome = "Grêmio", Cidade = "Porto Alegre", Estado = "RS", Pais = "Brasil", CorPrimaria = "#1E90FF", CorSecundaria = "#000000", CorTerciaria = "#FFFFFF", Divisao = 1 });
                listaTimes.Add(new Time { Id = 5, Nome = "Palmeiras", Cidade = "São Paulo", Estado = "SP", Pais = "Brasil", CorPrimaria = "#006400", CorSecundaria = "#FFFFFF", CorTerciaria = "#008000", Divisao = 1 });
                listaTimes.Add(new Time { Id = 6, Nome = "Internacional", Cidade = "Porto Alegre", Estado = "RS", Pais = "Brasil", CorPrimaria = "#FF0000", CorSecundaria = "#FFFFFF", CorTerciaria = "#FF0000", Divisao = 1 });
                listaTimes.Add(new Time { Id = 7, Nome = "Athletico Paranaense", Cidade = "Curitiba", Estado = "PR", Pais = "Brasil", CorPrimaria = "#FF0000", CorSecundaria = "#000000", CorTerciaria = "#FF0000", Divisao = 1 });
                listaTimes.Add(new Time { Id = 8, Nome = "Bahia", Cidade = "Salvador", Estado = "BA", Pais = "Brasil", CorPrimaria = "#0000FF", CorSecundaria = "#FFFFFF", CorTerciaria = "#FF0000", Divisao = 1 });
                listaTimes.Add(new Time { Id = 9, Nome = "Botafogo", Cidade = "Rio de Janeiro", Estado = "RJ", Pais = "Brasil", CorPrimaria = "#000000", CorSecundaria = "#FFFFFF", CorTerciaria = "#000000", Divisao = 1 });
                listaTimes.Add(new Time { Id = 10, Nome = "Fortaleza", Cidade = "Fortaleza", Estado = "CE", Pais = "Brasil", CorPrimaria = "#0000FF", CorSecundaria = "#FF0000", CorTerciaria = "#FFFFFF", Divisao = 1 });
                listaTimes.Add(new Time { Id = 11, Nome = "Fluminense", Cidade = "Rio de Janeiro", Estado = "RJ", Pais = "Brasil", CorPrimaria = "#008000", CorSecundaria = "#FFFFFF", CorTerciaria = "#800000", Divisao = 1 });
                listaTimes.Add(new Time { Id = 12, Nome = "Vasco da Gama", Cidade = "Rio de Janeiro", Estado = "RJ", Pais = "Brasil", CorPrimaria = "#000000", CorSecundaria = "#FFFFFF", CorTerciaria = "#FF0000", Divisao = 1 });
                listaTimes.Add(new Time { Id = 13, Nome = "São Paulo", Cidade = "São Paulo", Estado = "SP", Pais = "Brasil", CorPrimaria = "#FF0000", CorSecundaria = "#FFFFFF", CorTerciaria = "#000000", Divisao = 1 });
                listaTimes.Add(new Time { Id = 14, Nome = "Red Bull Bragantino", Cidade = "Bragança Paulista", Estado = "SP", Pais = "Brasil", CorPrimaria = "#FFFFFF", CorSecundaria = "#FF0000", CorTerciaria = "#FFFFFF", Divisao = 1 });
                listaTimes.Add(new Time { Id = 15, Nome = "Cuiabá", Cidade = "Cuiabá", Estado = "MT", Pais = "Brasil", CorPrimaria = "#FFFF00", CorSecundaria = "#006400", CorTerciaria = "#008000", Divisao = 1 });
                listaTimes.Add(new Time { Id = 16, Nome = "Atlético Mineiro", Cidade = "Belo Horizonte", Estado = "MG", Pais = "Brasil", CorPrimaria = "#000000", CorSecundaria = "#FFFFFF", CorTerciaria = "#000000", Divisao = 1 });
                listaTimes.Add(new Time { Id = 17, Nome = "Atlético Goianiense", Cidade = "Goiânia", Estado = "GO", Pais = "Brasil", CorPrimaria = "#FF0000", CorSecundaria = "#000000", CorTerciaria = "#FFFFFF", Divisao = 1 });
                listaTimes.Add(new Time { Id = 18, Nome = "Criciúma", Cidade = "Criciúma", Estado = "SC", Pais = "Brasil", CorPrimaria = "#FFD700", CorSecundaria = "#FFFFFF", CorTerciaria = "#000000", Divisao = 1 });
                listaTimes.Add(new Time { Id = 19, Nome = "Juventude", Cidade = "Caxias do Sul", Estado = "RS", Pais = "Brasil", CorPrimaria = "#008000", CorSecundaria = "#FFFFFF", CorTerciaria = "#008000", Divisao = 1 });
                listaTimes.Add(new Time { Id = 20, Nome = "Vitória", Cidade = "Salvador", Estado = "BA", Pais = "Brasil", CorPrimaria = "#FF0000", CorSecundaria = "#000000", CorTerciaria = "#FFFFFF", Divisao = 1 });

            return listaTimes;
        }
        */

        /*
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

            listaJogadores.Add(new Jogador { Id = 23, Nome = "Cássio", Ataque = 25, Defesa = 88, Finalizacao = 33, Zona = "G", TimeId = 3 });
            listaJogadores.Add(new Jogador { Id = 24, Nome = "William", Ataque = 40, Defesa = 70, Finalizacao = 40, Zona = "D", TimeId = 3 });
            listaJogadores.Add(new Jogador { Id = 25, Nome = "Zé Ivaldo", Ataque = 38, Defesa = 78, Finalizacao = 40, Zona = "D", TimeId = 3 });
            listaJogadores.Add(new Jogador { Id = 26, Nome = "João Marcelo", Ataque = 32, Defesa = 78, Finalizacao = 38, Zona = "D", TimeId = 3 });
            listaJogadores.Add(new Jogador { Id = 27, Nome = "Kaiki", Ataque = 38, Defesa = 64, Finalizacao = 39, Zona = "D", TimeId = 3 });
            listaJogadores.Add(new Jogador { Id = 28, Nome = "Lucas Romero", Ataque = 47, Defesa = 44, Finalizacao = 52, Zona = "V", TimeId = 3 });
            listaJogadores.Add(new Jogador { Id = 29, Nome = "Matheus Henrique", Ataque = 77, Defesa = 43, Finalizacao = 68, Zona = "V", TimeId = 3 });
            listaJogadores.Add(new Jogador { Id = 30, Nome = "Barreal", Ataque = 74, Defesa = 29, Finalizacao = 77, Zona = "M", TimeId = 3 });
            listaJogadores.Add(new Jogador { Id = 31, Nome = "Matheus Pereira", Ataque = 84, Defesa = 28, Finalizacao = 82, Zona = "M", TimeId = 3 });
            listaJogadores.Add(new Jogador { Id = 32, Nome = "Lautaro Díaz", Ataque = 78, Defesa = 25, Finalizacao = 75, Zona = "A", TimeId = 3 });
            listaJogadores.Add(new Jogador { Id = 33, Nome = "Kaio Jorge", Ataque = 75, Defesa = 28, Finalizacao = 74, Zona = "A", TimeId = 3 });

            listaJogadores.Add(new Jogador { Id = 34, Nome = "Marchesín", Ataque = 25, Defesa = 75, Finalizacao = 33, Zona = "G", TimeId = 4 });
            listaJogadores.Add(new Jogador { Id = 35, Nome = "João Pedro", Ataque = 40, Defesa = 70, Finalizacao = 44, Zona = "D", TimeId = 4 });
            listaJogadores.Add(new Jogador { Id = 36, Nome = "Rodrigo Ely", Ataque = 38, Defesa = 77, Finalizacao = 40, Zona = "D", TimeId = 4 });
            listaJogadores.Add(new Jogador { Id = 37, Nome = "Jemerson", Ataque = 32, Defesa = 66, Finalizacao = 38, Zona = "D", TimeId = 4 });
            listaJogadores.Add(new Jogador { Id = 38, Nome = "Reinaldo", Ataque = 34, Defesa = 64, Finalizacao = 29, Zona = "D", TimeId = 4 });
            listaJogadores.Add(new Jogador { Id = 39, Nome = "Villasanti", Ataque = 70, Defesa = 50, Finalizacao = 52, Zona = "V", TimeId = 4 });
            listaJogadores.Add(new Jogador { Id = 40, Nome = "Dodi", Ataque = 65, Defesa = 45, Finalizacao = 54, Zona = "V", TimeId = 4 });
            listaJogadores.Add(new Jogador { Id = 41, Nome = "Cristaldo", Ataque = 77, Defesa = 24, Finalizacao = 78, Zona = "M", TimeId = 4 });
            listaJogadores.Add(new Jogador { Id = 42, Nome = "Edenilson", Ataque = 55, Defesa = 55, Finalizacao = 75, Zona = "M", TimeId = 4 });
            listaJogadores.Add(new Jogador { Id = 43, Nome = "Soteldo", Ataque = 84, Defesa = 30, Finalizacao = 72, Zona = "A", TimeId = 4 });
            listaJogadores.Add(new Jogador { Id = 44, Nome = "Pavon", Ataque = 75, Defesa = 28, Finalizacao = 77, Zona = "A", TimeId = 4 });

            listaJogadores.Add(new Jogador { Id = 44, Nome = "Cássio", Ataque = 25, Defesa = 88, Finalizacao = 33, Zona = "G", TimeId = 5 });
            listaJogadores.Add(new Jogador { Id = 45, Nome = "William", Ataque = 40, Defesa = 70, Finalizacao = 40, Zona = "D", TimeId = 5 });
            listaJogadores.Add(new Jogador { Id = 46, Nome = "Zé Ivaldo", Ataque = 38, Defesa = 78, Finalizacao = 40, Zona = "D", TimeId = 5 });
            listaJogadores.Add(new Jogador { Id = 47, Nome = "João Marcelo", Ataque = 32, Defesa = 78, Finalizacao = 38, Zona = "D", TimeId = 5 });
            listaJogadores.Add(new Jogador { Id = 48, Nome = "Kaiki", Ataque = 38, Defesa = 64, Finalizacao = 39, Zona = "D", TimeId = 5 });
            listaJogadores.Add(new Jogador { Id = 49, Nome = "Lucas Romero", Ataque = 47, Defesa = 44, Finalizacao = 52, Zona = "V", TimeId = 5 });
            listaJogadores.Add(new Jogador { Id = 50, Nome = "Matheus Henrique", Ataque = 77, Defesa = 43, Finalizacao = 68, Zona = "V", TimeId = 5 });
            listaJogadores.Add(new Jogador { Id = 51, Nome = "Barreal", Ataque = 74, Defesa = 29, Finalizacao = 77, Zona = "M", TimeId = 5 });
            listaJogadores.Add(new Jogador { Id = 52, Nome = "Matheus Pereira", Ataque = 84, Defesa = 28, Finalizacao = 82, Zona = "M", TimeId = 5 });
            listaJogadores.Add(new Jogador { Id = 53, Nome = "Lautaro Díaz", Ataque = 78, Defesa = 25, Finalizacao = 75, Zona = "A", TimeId = 5 });
            listaJogadores.Add(new Jogador { Id = 54, Nome = "Kaio Jorge", Ataque = 75, Defesa = 28, Finalizacao = 74, Zona = "A", TimeId = 5 });

            listaJogadores.Add(new Jogador { Id = 55, Nome = "Marchesín", Ataque = 25, Defesa = 75, Finalizacao = 33, Zona = "G", TimeId = 6 });
            listaJogadores.Add(new Jogador { Id = 55, Nome = "João Pedro", Ataque = 40, Defesa = 70, Finalizacao = 44, Zona = "D", TimeId = 6 });
            listaJogadores.Add(new Jogador { Id = 56, Nome = "Rodrigo Ely", Ataque = 38, Defesa = 77, Finalizacao = 40, Zona = "D", TimeId = 6 });
            listaJogadores.Add(new Jogador { Id = 57, Nome = "Jemerson", Ataque = 32, Defesa = 66, Finalizacao = 38, Zona = "D", TimeId = 6 });
            listaJogadores.Add(new Jogador { Id = 58, Nome = "Reinaldo", Ataque = 34, Defesa = 64, Finalizacao = 29, Zona = "D", TimeId = 6 });
            listaJogadores.Add(new Jogador { Id = 59, Nome = "Villasanti", Ataque = 70, Defesa = 50, Finalizacao = 52, Zona = "V", TimeId = 6 });
            listaJogadores.Add(new Jogador { Id = 60, Nome = "Dodi", Ataque = 65, Defesa = 45, Finalizacao = 54, Zona = "V", TimeId = 6 });
            listaJogadores.Add(new Jogador { Id = 61, Nome = "Cristaldo", Ataque = 77, Defesa = 24, Finalizacao = 78, Zona = "M", TimeId = 6 });
            listaJogadores.Add(new Jogador { Id = 62, Nome = "Edenilson", Ataque = 55, Defesa = 55, Finalizacao = 75, Zona = "M", TimeId = 6 });
            listaJogadores.Add(new Jogador { Id = 63, Nome = "Soteldo", Ataque = 84, Defesa = 30, Finalizacao = 72, Zona = "A", TimeId = 6 });
            listaJogadores.Add(new Jogador { Id = 64, Nome = "Pavon", Ataque = 75, Defesa = 28, Finalizacao = 77, Zona = "A", TimeId = 6 });

            return listaJogadores;
        }
        */
    }
}
