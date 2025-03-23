using MFoot.Maui.Domain;
using MFoot.Maui.Models;
using MFoot.Maui.Views.Times;
using MFoot.Maui.Common;
using MFoot.Maui.Views.Partidas;
using Microsoft.Extensions.Logging;

namespace MFoot.Maui.Builder.Views
{
    public static class CampeonatoViewBuilder
    {
        public static StackLayout BuildJogadoresTimeGrid(List<Jogador> jogadores)
        {
            var stackLayout = new StackLayout();

            int rowIndex = 0;

            foreach (var jogador in jogadores)
            {
                // Grid Principal
                var grid = new Grid()
                {
                    ColumnDefinitions = new ColumnDefinitionCollection
                    {
                        new ColumnDefinition { Width = 40 },
                        new ColumnDefinition { Width = GridLength.Star },
                        new ColumnDefinition { Width = GridLength.Auto }
                    }                    
                };

                var framePosicao = new Frame
                {
                    BackgroundColor = Color.FromArgb(Util.RetornarCorPorZona(jogador.Zona)),
                    BorderColor = Colors.Transparent,
                    HasShadow = false,
                    CornerRadius = 5,
                    Padding = 4,
                    VerticalOptions = LayoutOptions.StartAndExpand,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    Content = new Label
                    {
                        Text = jogador.Posicao,
                        TextColor = Colors.White,
                        FontSize = 11,
                        HorizontalTextAlignment = TextAlignment.Center
                    }
                };

                grid.Add(framePosicao, 0, rowIndex);

                var gridNome = new Grid()
                {
                    RowDefinitions = new RowDefinitionCollection
                    {
                        new RowDefinition { Height = GridLength.Auto },
                        new RowDefinition { Height = GridLength.Auto }
                    }
                };

                var stack = new HorizontalStackLayout();

                var labelNome = new Label()
                {
                    Text = jogador.Nome,
                    Padding = new Thickness(10,0),
                    VerticalOptions = LayoutOptions.CenterAndExpand
                };

                stack.Children.Add(labelNome);

                if (jogador.Cartoes.Count > 0)
                {
                    foreach (var cartao in jogador.Cartoes)
                    {
                        var iconCartoes = new Image()
                        { 
                            Source = cartao.TipoId == 2 ? "yellow_card.png" : cartao.TipoId == 3 ? "red_card.png" : "", 
                            WidthRequest = 12,
                            HeightRequest = 12,
                            VerticalOptions = LayoutOptions.Center
                        };
                
                        stack.Children.Add(iconCartoes);
                    }
                }

                gridNome.Add(stack, 0, 0);

                var gridAtributos = new Grid()
                {
                    ColumnDefinitions = new ColumnDefinitionCollection
                    {                        
                        new ColumnDefinition { Width = GridLength.Auto },
                        new ColumnDefinition { Width = GridLength.Auto },
                        new ColumnDefinition { Width = GridLength.Auto },
                        new ColumnDefinition { Width = GridLength.Auto }
                    },
                    Padding = new Thickness(10,0)
                };

                gridAtributos.Add(new Label() { Text = $"ATQ: {jogador.Ataque.ToString("F2")}", TextColor = Colors.Gray, Padding = new Thickness(0, 0, 5, 0), FontSize = 11, VerticalOptions = LayoutOptions.CenterAndExpand }, 0, 0);
                gridAtributos.Add(new Label() { Text = $"DEF: {Math.Round(jogador.Defesa, 1)}", TextColor = Colors.Gray, Padding = new Thickness(0, 0, 5, 0), FontSize = 11, VerticalOptions = LayoutOptions.CenterAndExpand }, 1, 0);
                //gridAtributos.Add(new Label() { Text = $"FIN: {Math.Round(jogador.Finalizacao, 1)}", TextColor = Colors.Gray, Padding = new Thickness(0, 0, 5, 0), FontSize = 11, VerticalOptions = LayoutOptions.CenterAndExpand }, 2, 0);
                gridAtributos.Add(new Label() { Text = $"RES: {jogador.Resistencia}", TextColor = Colors.Gray, Padding = new Thickness(0, 0, 5, 0), FontSize = 11, VerticalOptions = LayoutOptions.CenterAndExpand }, 2, 0);
                gridAtributos.Add(new Label() { Text = $"IDA: {jogador.Idade}", TextColor = Colors.Gray, Padding = new Thickness(0, 0, 5, 0), FontSize = 11, VerticalOptions = LayoutOptions.CenterAndExpand }, 3, 0);

                gridNome.Add(gridAtributos, 0, 1);

                grid.Add(gridNome, 1, rowIndex);


                var gridValores = new Grid
                {
                    RowDefinitions = new RowDefinitionCollection
                    {
                        new RowDefinition { Height = GridLength.Auto },
                        new RowDefinition { Height = GridLength.Auto }
                    }
                };

                var labelValorMercado = new Label
                {
                    Text = jogador.Valor.ToString("N2"),
                    TextColor = Colors.Gray,
                    FontSize = 11,
                    Padding = new Thickness(10, 0),
                    VerticalOptions = LayoutOptions.StartAndExpand,
                    HorizontalOptions = LayoutOptions.EndAndExpand
                };
                
                var resistencia = Math.Round(jogador.Resistencia / 100.0,1);

                var labelResistencia = new ProgressBar
                {
                    BackgroundColor = Color.FromArgb("#e8e8e8"),
                    ProgressColor = Color.FromArgb(Util.RetornarCorResistencia(jogador.Resistencia)),
                    Progress = resistencia,
                    Margin = new Thickness(0,8),
                    WidthRequest = 90,
                    HorizontalOptions = LayoutOptions.EndAndExpand,
                    FlowDirection = FlowDirection.RightToLeft
                };

                /*
                var labelSalario = new Label
                {
                    Text = "-",
                    TextColor = Colors.Gray,
                    FontSize = 11,
                    Padding = new Thickness(10, 0),
                    VerticalOptions = LayoutOptions.StartAndExpand,
                    HorizontalOptions = LayoutOptions.EndAndExpand
                };
                */

                gridValores.Add(labelValorMercado, 0, 0);
                gridValores.Add(labelResistencia, 0, 1);

                grid.Add(gridValores, 2, rowIndex);

                rowIndex++;

                stackLayout.Children.Add(grid);
            }

            return stackLayout;
        }

        public static StackLayout BuildRodadasGrid(List<CampeonatoRodada> rodadas, Action<CampeonatoPartida> abrirModalHandler)
        {
            var stackLayout = new StackLayout();

            stackLayout.Children.Add(BuildTitleLayout("Jogos"));

            foreach (var rodada in rodadas)
            {
                // Criar um label para o nome da rodada
                var rodadaLabel = new Label
                {
                    Text = $"Rodada {rodada.Numero.ToString()}",
                    FontAttributes = FontAttributes.Bold,
                    FontSize = 14,
                    Padding = new Thickness(0,5),
                    TextColor = Colors.Black
                };

                stackLayout.Children.Add(rodadaLabel);

                var rowIndex = 0;

                // Criar uma lista de partidas para a rodada
                foreach (var partida in rodada.Partidas)
                {
                    var grid = new Grid()
                    {
                        ColumnDefinitions = new ColumnDefinitionCollection
                        {
                            new ColumnDefinition { Width = GridLength.Star },
                            new ColumnDefinition { Width = GridLength.Auto },
                            new ColumnDefinition { Width = GridLength.Auto },
                            new ColumnDefinition { Width = GridLength.Auto },
                            new ColumnDefinition { Width = GridLength.Star },
                            new ColumnDefinition { Width = GridLength.Auto }
                        }
                    };

                    var iconDetalhes = new Image
                    {
                        Source = "chart_bar_icon.png",
                        WidthRequest = 12,
                        Margin = new Thickness(15,0,0,0),
                        Opacity = 0.7
                    };

                    // Cria o TapGestureRecognizer
                    var tapGesture = new TapGestureRecognizer();
                    tapGesture.Tapped += (sender, e) => abrirModalHandler(partida);

                    // Adiciona o TapGestureRecognizer à imagem
                    iconDetalhes.GestureRecognizers.Add(tapGesture);

                    grid.Add(new Label { Text = partida.TimeCasa.Nome, HorizontalOptions = LayoutOptions.Start }, 0, rowIndex);
                    grid.Add(new Label { Text = partida.PlacarCasa.ToString() }, 1, rowIndex);
                    grid.Add(new Label { Text = "x" }, 2, rowIndex);
                    grid.Add(new Label { Text = partida.PlacarVisitante.ToString() }, 3, rowIndex);
                    grid.Add(new Label { Text = partida.TimeVisitante.Nome, HorizontalOptions = LayoutOptions.End }, 4, rowIndex);
                    grid.Add(iconDetalhes, 5, rowIndex);

                    rowIndex++;

                    stackLayout.Children.Add(grid);
                }

            }
            
            return stackLayout;
        }

        public static StackLayout BuildPartidaDetalhesGrid(CampeonatoPartida partida)
        {
            var stackLayout = new StackLayout();

            var grid = new Grid()
            {
                ColumnDefinitions = new ColumnDefinitionCollection
                {
                    new ColumnDefinition { Width = GridLength.Star },
                    new ColumnDefinition { Width = GridLength.Auto },
                    new ColumnDefinition { Width = GridLength.Auto },
                    new ColumnDefinition { Width = GridLength.Auto },
                    new ColumnDefinition { Width = GridLength.Star }
                }

            };

            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

            grid.Add(new Label { Text = partida.TimeCasa.Nome, FontSize = 18, HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.Center }, 0, 0);
            grid.Add(new Label { Text = partida.PlacarCasa.ToString(), FontSize = 18, VerticalOptions = LayoutOptions.Center }, 1, 0);
            grid.Add(new Label { Text = "x", FontSize = 14, VerticalOptions = LayoutOptions.Center, Padding = new Thickness(3,0) }, 2, 0);
            grid.Add(new Label { Text = partida.PlacarVisitante.ToString(), FontSize = 18, VerticalOptions = LayoutOptions.Center }, 3, 0);
            grid.Add(new Label { Text = partida.TimeVisitante.Nome, FontSize = 18, HorizontalOptions = LayoutOptions.End, VerticalOptions = LayoutOptions.Center }, 4, 0);

            stackLayout.Children.Add(grid);

            var separator1 = new BoxView
            {
                HeightRequest = 1,
                Color = Color.FromArgb("#CCCCCC"),
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Margin = new Thickness(0,10)
            };

            stackLayout.Children.Add(separator1);

            var gridEventos = new Grid()
            {
                ColumnDefinitions = new ColumnDefinitionCollection
                {
                    new ColumnDefinition { Width = GridLength.Star },
                    new ColumnDefinition { Width = GridLength.Auto },
                    new ColumnDefinition { Width = GridLength.Star }
                }
            };

            var rowIndex = 0;

            foreach (var evento in partida.Eventos)
            {
                gridEventos.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

                var gridEventoCasa = new Grid
                {
                    ColumnDefinitions = new ColumnDefinitionCollection
                    {
                        new ColumnDefinition { Width = GridLength.Star },
                        new ColumnDefinition { Width = GridLength.Auto }
                    }
                };

                gridEventoCasa.Add(new Label { Text = evento.Jogador.Nome.ToString(), VerticalOptions = LayoutOptions.Center, HorizontalOptions = LayoutOptions.End }, 0, 0);
                gridEventoCasa.Add(new Image { Source = evento.EventoIcone, WidthRequest = 12, HeightRequest = 12, VerticalOptions = LayoutOptions.Center, Margin = new Thickness(5,0,0,0) }, 1, 0);

                var gridEventoVisitante = new Grid
                {
                    ColumnDefinitions = new ColumnDefinitionCollection
                    {
                        new ColumnDefinition { Width = GridLength.Auto },
                        new ColumnDefinition { Width = GridLength.Star }
                    }
                };

                gridEventoVisitante.Add(new Image { Source = evento.EventoIcone, WidthRequest = 12, HeightRequest = 12, VerticalOptions = LayoutOptions.Center, Margin = new Thickness(0, 0, 5, 0) }, 0, 0);
                gridEventoVisitante.Add(new Label { Text = evento.Jogador.Nome.ToString(), VerticalOptions = LayoutOptions.Center, HorizontalOptions = LayoutOptions.Start }, 1, 0);

                gridEventos.Add((evento.TimeId == partida.TimeCasaId ? gridEventoCasa : new Label()), 0, rowIndex);
                gridEventos.Add(new Label { Text = $"{evento.Tempo.ToString("00")}'", Margin = new Thickness(20, 0)}, 1, rowIndex);
                gridEventos.Add((evento.TimeId == partida.TimeVisitanteId ? gridEventoVisitante : new Label()), 2, rowIndex);

                rowIndex++;
            }

            stackLayout.Children.Add(gridEventos);

            return stackLayout;
        }

        public static StackLayout BuildTimesLayout(List<Time> times)
        {
            var stackLayout = new StackLayout();

            var grid = new Grid()
            {
                ColumnDefinitions = new ColumnDefinitionCollection
                {
                    new ColumnDefinition { Width = GridLength.Auto },
                    new ColumnDefinition { Width = GridLength.Star },
                    new ColumnDefinition { Width = GridLength.Star }
                }
            };

            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

            grid.Add(new Label { Text = "" }, 0, 0);
            grid.Add(new Label { Text = "ATQ", HorizontalOptions = LayoutOptions.Center }, 1, 0);
            grid.Add(new Label { Text = "DEF", HorizontalOptions = LayoutOptions.Center }, 2, 0);

            var rowIndex = 1;

            foreach (var time in times)
            {
                grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

                var timeLabel = new Label { Text = time.Nome, VerticalOptions = LayoutOptions.Center };
                var ataqueLabel = new Label { Text = Math.Round(time.Ataque, 1).ToString().Replace(",", "."), HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center };
                var defesaLabel = new Label { Text = Math.Round(time.Defesa, 1).ToString().Replace(",", "."), HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center };

                // Adicionar os labels diretamente ao Grid principal
                grid.Add(timeLabel, 0, rowIndex);
                grid.Add(ataqueLabel, 1, rowIndex);
                grid.Add(defesaLabel, 2, rowIndex);

                // Adicionar TapGestureRecognizer à linha inteira
                var tapGestureRecognizer = new TapGestureRecognizer();
                tapGestureRecognizer.Tapped += async (s, e) =>
                {
                    // Abrir o modal com os detalhes do time
                    await Application.Current.MainPage.Navigation.PushModalAsync(new TimeDetalhesPage(time));
                };

                // Adicionar o gesto ao grid principal, aplicando-o a cada célula da linha
                timeLabel.GestureRecognizers.Add(tapGestureRecognizer);
                ataqueLabel.GestureRecognizers.Add(tapGestureRecognizer);
                defesaLabel.GestureRecognizers.Add(tapGestureRecognizer);

                rowIndex++;
            }

            stackLayout.Children.Add(BuildTitleLayout("Ranking de Times"));
            stackLayout.Children.Add(grid);

            return stackLayout;
        }

        public static StackLayout BuildClassificacaoLayout(List<CampeonatoClassificacao> classificacao)
        {
            var stackLayout = new StackLayout();

            var grid = new Grid()
            {
                ColumnDefinitions = new ColumnDefinitionCollection
                {
                    new ColumnDefinition { Width = GridLength.Auto },
                    new ColumnDefinition { Width = GridLength.Star },
                    new ColumnDefinition { Width = GridLength.Star },
                    new ColumnDefinition { Width = GridLength.Star },
                    new ColumnDefinition { Width = GridLength.Star },
                    new ColumnDefinition { Width = GridLength.Star },
                    new ColumnDefinition { Width = GridLength.Star },
                    new ColumnDefinition { Width = GridLength.Star },
                    new ColumnDefinition { Width = GridLength.Star }
                }
            };

            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

            grid.Add(new Label { Text = "" }, 0, 0);
            grid.Add(new Label { Text = "J", HorizontalOptions = LayoutOptions.Center }, 1, 0);
            grid.Add(new Label { Text = "PG", HorizontalOptions = LayoutOptions.Center }, 2, 0);
            grid.Add(new Label { Text = "V", HorizontalOptions = LayoutOptions.Center }, 3, 0);
            grid.Add(new Label { Text = "E", HorizontalOptions = LayoutOptions.Center }, 4, 0);
            grid.Add(new Label { Text = "D", HorizontalOptions = LayoutOptions.Center }, 5, 0);
            grid.Add(new Label { Text = "GF", HorizontalOptions = LayoutOptions.Center }, 6, 0);
            grid.Add(new Label { Text = "GC", HorizontalOptions = LayoutOptions.Center }, 7, 0);
            grid.Add(new Label { Text = "SG", HorizontalOptions = LayoutOptions.Center }, 8, 0);

            var rowIndex = 1;

            foreach (var time in classificacao)
            {
                grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

                grid.Add(new Label { Text = time.Time.Nome }, 0, rowIndex);
                grid.Add(new Label { Text = time.Jogos.ToString(), HorizontalOptions = LayoutOptions.Center }, 1, rowIndex);
                grid.Add(new Label { Text = time.Pontos.ToString(), HorizontalOptions = LayoutOptions.Center }, 2, rowIndex);
                grid.Add(new Label { Text = time.Vitoria.ToString(), HorizontalOptions = LayoutOptions.Center }, 3, rowIndex);
                grid.Add(new Label { Text = time.Empate.ToString(), HorizontalOptions = LayoutOptions.Center }, 4, rowIndex);
                grid.Add(new Label { Text = time.Derrota.ToString(), HorizontalOptions = LayoutOptions.Center }, 5, rowIndex);
                grid.Add(new Label { Text = time.GolFavor.ToString(), HorizontalOptions = LayoutOptions.Center }, 6, rowIndex);
                grid.Add(new Label { Text = time.GolContra.ToString(), HorizontalOptions = LayoutOptions.Center }, 7, rowIndex);
                grid.Add(new Label { Text = time.GolSaldo.ToString(), HorizontalOptions = LayoutOptions.Center }, 8, rowIndex);

                rowIndex++;
            }

            stackLayout.Children.Add(BuildTitleLayout("Classificação"));
            stackLayout.Children.Add(grid);

            return stackLayout;
        }

        public static StackLayout BuildArtilheirosLayout(List<CampeonatoArtilharia> artilharia)
        {
            var stackLayout = new StackLayout();

            var grid = new Grid
            {
                ColumnDefinitions = new ColumnDefinitionCollection
                {
                    new ColumnDefinition { Width = GridLength.Star },
                    new ColumnDefinition { Width = GridLength.Star },
                    new ColumnDefinition { Width = GridLength.Auto },
                }
            };

            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

            grid.Add(new Label { Text = "Nome" }, 0, 0);
            grid.Add(new Label { Text = "Time" }, 1, 0);
            grid.Add(new Label { Text = "Gols" }, 2, 0);

            var rowIndex = 1;

            foreach (var artilheiro in artilharia)
            {
                grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

                grid.Add(new Label { Text = artilheiro.Jogador }, 0, rowIndex);
                grid.Add(new Label { Text = artilheiro.Time }, 1, rowIndex);
                grid.Add(new Label { Text = artilheiro.Gols.ToString() }, 2, rowIndex);

                rowIndex++;
            }

            stackLayout.Children.Add(BuildTitleLayout("Artilharia"));
            stackLayout.Children.Add(grid);

            return stackLayout;
        }

        private static Frame BuildTitleLayout(string title)
        {
            var labelTitle = new Label
            {
                Text = title,
                TextColor = Colors.White
            };

            var frameTitle = new Frame
            {
                CornerRadius = 5,
                BackgroundColor = Color.FromArgb("#0ea2a2"),
                Padding = new Thickness(10, 5),
                HasShadow = false,
                BorderColor = Colors.Transparent,
                Margin = new Thickness(0, 0, 0, 10),
                Content = labelTitle
            };

            return frameTitle;
        }
    }

}
