using Plugin.Maui.Audio;
using GameCore;

namespace GameUI;

public partial class MainPage : ContentPage
{
    private readonly IAudioManager audioManager;

    HorizontalStackLayout bilgiPaneliYatay = new HorizontalStackLayout()
    {
        HorizontalOptions = LayoutOptions.Center,
        VerticalOptions = LayoutOptions.End
    };
    StackLayout kurpiyerKartlariStackLayout = new StackLayout();
    StackLayout oyuncuKartlariStackLayout = new StackLayout();

    HorizontalStackLayout oyuncuKartlarYatay = new HorizontalStackLayout()
    {
        HorizontalOptions = LayoutOptions.Center,
        VerticalOptions = LayoutOptions.Center
    };
    HorizontalStackLayout oyuncuSplitKartlarYatay = new HorizontalStackLayout()
    {
        HorizontalOptions = LayoutOptions.Center,
        VerticalOptions = LayoutOptions.Center
    };
    HorizontalStackLayout kurpiyerKartlarYatay = new HorizontalStackLayout()
    {
        HorizontalOptions = LayoutOptions.Center,
        VerticalOptions = LayoutOptions.Center
    };
    HorizontalStackLayout bottomBar = new HorizontalStackLayout()
    {
        HorizontalOptions = LayoutOptions.Center,
        VerticalOptions = LayoutOptions.End
    };

    // Oyuncu ve Kurpiyer nesneleri oluşturulur.
    Oyuncu oyuncu = new Oyuncu(100);
    Kurpiyer kurpiyer = new Kurpiyer();
    List<Kart> deste;

    Guid splitViewGuid = new Guid();

    Blackjack oyun = new Blackjack();

    int kurpiyerSkor = 0;
    int oyuncuSkor = 0;
    decimal bakiye = 0;
    private bool _sesAcikMi = true;
    private bool sesAcikMi
    {
        get
        {
            return _sesAcikMi;
        }
        set
        {
            _sesAcikMi = value;
            sesKontrolLabel.Text = value ? "Sesi Kapat" : "Sesi Aç";
            sesKontrolImage.Source = value ? "sound_on.png" : "sound_off.png";
        }
    }

    Label historyLabel = new Label
    {
        Text = "Geçmiş Oyunlar",
        WidthRequest = 100,
        HeightRequest = 50,
        FontSize = 15,
        HorizontalTextAlignment = TextAlignment.Center,
        VerticalTextAlignment = TextAlignment.Center,
    };

    ImageButton historyImage = new ImageButton
    {
        Source = "history.png",
        WidthRequest = 75,
        HeightRequest = 50,
        Margin = -10
    };

    Label kasaLabel = new Label
    {
        Text = "0",
        WidthRequest = 100,
        HeightRequest = 50,
        FontSize = 25,
        HorizontalTextAlignment = TextAlignment.Center,
        VerticalTextAlignment = TextAlignment.Center,
    };

    ImageButton kasaImage = new ImageButton
    {
        Source = "kasa.png",
        WidthRequest = 75,
        HeightRequest = 50,
        Margin = -10
    };

    Label oyuncuPuanLabel = new Label
    {
        Text = "0",
        WidthRequest = 50,
        HeightRequest = 50,
        FontSize = 25,
        HorizontalTextAlignment = TextAlignment.Center,
        VerticalTextAlignment = TextAlignment.Center,
    };

    ImageButton oyuncuPuanImage = new ImageButton
    {
        Source = "oyuncu.png",
        WidthRequest = 75,
        HeightRequest = 50,
        Margin = -10
    };

    Label kurpiyerPuanLabel = new Label
    {
        Text = "0",
        WidthRequest = 50,
        HeightRequest = 50,
        FontSize = 25,
        HorizontalTextAlignment = TextAlignment.Center,
        VerticalTextAlignment = TextAlignment.Center,
    };

    ImageButton kurpiyerPuanImage = new ImageButton
    {
        Source = "kurpiyer.png",
        WidthRequest = 75,
        HeightRequest = 50,
        Margin = -10
    };

    Label durLabel = new Label
    {
        Text = "Dur",
        IsEnabled = false,
        WidthRequest = 125,
        HeightRequest = 50,
        FontSize = 25,
        HorizontalTextAlignment = TextAlignment.Center,
        VerticalTextAlignment = TextAlignment.Center,
    };

    ImageButton durImage = new ImageButton
    {
        Source = "stop.png",
        IsEnabled = false,
        WidthRequest = 125,
        HeightRequest = 150,
        Margin = -10
    };

    Label kartCekLabel = new Label
    {
        Text = "Kart İste",
        IsEnabled = false,
        WidthRequest = 125,
        HeightRequest = 50,
        FontSize = 25,
        HorizontalTextAlignment = TextAlignment.Center,
        VerticalTextAlignment = TextAlignment.Center,
    };


    ImageButton kartCekImage = new ImageButton
    {
        Source = "card_icon.png",
        IsEnabled = false,
        WidthRequest = 125,
        HeightRequest = 150,
        Margin = -10
    };

    Label oyunuBaslatLabel = new Label
    {
        Text = "Bahis Koy",
        WidthRequest = 125,
        HeightRequest = 50,
        FontSize = 25,
        HorizontalTextAlignment = TextAlignment.Center,
        VerticalTextAlignment = TextAlignment.Center,
    };

    ImageButton oyunuBaslatImage = new ImageButton
    {
        Source = "bet.png",
        IsEnabled = true,
        WidthRequest = 125,
        HeightRequest = 150,
        Margin = -10
    };

    Label splitLabel = new Label
    {
        Text = "Böl",
        WidthRequest = 125,
        HeightRequest = 50,
        FontSize = 25,
        HorizontalTextAlignment = TextAlignment.Center,
        VerticalTextAlignment = TextAlignment.Center,
    };

    ImageButton splitImage = new ImageButton
    {
        Source = "split.png",
        IsEnabled = true,
        WidthRequest = 125,
        HeightRequest = 150,
        Margin = -10
    };

    Label sesKontrolLabel = new Label
    {
        Text = "Sesi Kapat",
        WidthRequest = 125,
        HeightRequest = 50,
        FontSize = 25,
        HorizontalOptions = LayoutOptions.End,
        VerticalOptions = LayoutOptions.End,
        HorizontalTextAlignment = TextAlignment.End,
        VerticalTextAlignment = TextAlignment.End,
    };

    ImageButton sesKontrolImage = new ImageButton
    {
        Source = "sound_on.png",
        WidthRequest = 125,
        HeightRequest = 150,
        HorizontalOptions = LayoutOptions.End,
        VerticalOptions = LayoutOptions.End,
        Margin = -10
    };

    IAudioPlayer _playerAnalog;
    IAudioPlayer _playerShuffleCards;
    IAudioPlayer _playerFlipCard;
    IAudioPlayer _playerWin;
    IAudioPlayer _playerLose;
    IAudioPlayer _playerGasp;
    IAudioPlayer _playerApplause;

    private async void LoadSounds()
    {
        try
        {
            _playerAnalog = audioManager.CreatePlayer(await FileSystem.OpenAppPackageFileAsync("analog_click.mp3"));
            _playerShuffleCards = audioManager.CreatePlayer(await FileSystem.OpenAppPackageFileAsync("shuffle-cards.mp3"));
            _playerFlipCard = audioManager.CreatePlayer(await FileSystem.OpenAppPackageFileAsync("flipcard.mp3"));
            _playerWin = audioManager.CreatePlayer(await FileSystem.OpenAppPackageFileAsync("win.mp3"));
            _playerLose = audioManager.CreatePlayer(await FileSystem.OpenAppPackageFileAsync("lose.mp3"));
            _playerGasp = audioManager.CreatePlayer(await FileSystem.OpenAppPackageFileAsync("gasp.mp3"));
            _playerApplause = audioManager.CreatePlayer(await FileSystem.OpenAppPackageFileAsync("applause.mp3"));
        }
        catch (Exception ex)
        {
            ;
        }
    }

    public MainPage(IAudioManager audioManager)
    {
        this.BackgroundImageSource = ImageSource.FromFile("table.jpg");

        this.audioManager = audioManager;
        LoadSounds();

        oyuncu.KartCekildi += Oyuncu_KartCekildi;
        oyuncu.SplitYapildi += Oyuncu_SplitYapildi;
        kurpiyer.KartCekildi += Kurpiyer_KartCekildi;

        var grid = new Grid
        {
            ColumnDefinitions =
            {
                new ColumnDefinition { Width = GridLength.Star }
            },
            RowDefinitions =
            {
                new RowDefinition { Height = GridLength.Auto },
                new RowDefinition { Height = GridLength.Star },
                new RowDefinition { Height = GridLength.Star },
                new RowDefinition { Height = GridLength.Star }
            },
            Margin = 20,
        };

        oyun = new Blackjack();
        oyun.OyunBasladi += Oyun_OyunBasladi;
        oyun.OyunBitti += Oyun_OyunBitti;

        VerticalStackLayout kurpiyerPuanView = new VerticalStackLayout() { Margin = 10 };
        kurpiyerPuanView.Add(kurpiyerPuanImage);
        kurpiyerPuanView.Add(kurpiyerPuanLabel);
        bilgiPaneliYatay.Children.Add(kurpiyerPuanView);

        VerticalStackLayout oyuncuPuanView = new VerticalStackLayout() { Margin = 10 };
        oyuncuPuanView.Add(oyuncuPuanImage);
        oyuncuPuanView.Add(oyuncuPuanLabel);
        bilgiPaneliYatay.Children.Add(oyuncuPuanView);

        VerticalStackLayout kasaView = new VerticalStackLayout() { Margin = 10 };
        kasaView.Add(kasaImage);
        kasaView.Add(kasaLabel);
        bilgiPaneliYatay.Children.Add(kasaView);

        VerticalStackLayout historyView = new VerticalStackLayout() { Margin = 10 };
        historyView.Add(historyImage);
        historyView.Add(historyLabel);
        bilgiPaneliYatay.Children.Add(historyView);

        var historyRecognizer = new TapGestureRecognizer();
        historyRecognizer.Tapped += (s, e) =>
        {
            Navigation.PushAsync(new GameHistory());
        };

        historyImage.GestureRecognizers.Add(historyRecognizer);
        historyView.GestureRecognizers.Add(historyRecognizer);

        grid.Add(bilgiPaneliYatay, 0, 0);

        // Kurpiyer kartları
        grid.Add(kurpiyerKartlariStackLayout, 0, 1);
        kurpiyerKartlariStackLayout.Children.Add(new Label { Text = "Kurpiyer Kartları", FontSize = 20, HorizontalTextAlignment = TextAlignment.Center, VerticalTextAlignment = TextAlignment.Center });
        kurpiyerKartlariStackLayout.Children.Add(kurpiyerKartlarYatay);

        // Oyuncu kartları
        grid.Add(oyuncuKartlariStackLayout, 0, 2);
        oyuncuKartlariStackLayout.Children.Add(new Label { Text = "Oyuncu Kartları", FontSize = 20, HorizontalTextAlignment = TextAlignment.Center, VerticalTextAlignment = TextAlignment.Center });
        oyuncuKartlariStackLayout.Children.Add(oyuncuKartlarYatay);

        grid.Add(bottomBar, 0, 3);

        // Butonlar
        VerticalStackLayout oyunuBaslatView = new VerticalStackLayout() { Margin = 10 };
        var tapGestureRecognizer = new TapGestureRecognizer();
        tapGestureRecognizer.Tapped += async (s, e) =>
        {
            if (!oyunuBaslatLabel.IsEnabled && !oyunuBaslatImage.IsEnabled)
            {
                return;
            }

            if (sesAcikMi)
            {
                _playerAnalog.Play();
            }
            var bahis = await DisplayPromptAsync("Bahis Gir", "Oynanacak bahis miktarını girin:", "OK", "Cancel", "100", -1, Keyboard.Numeric, "100");

            kurpiyerKartlarYatay.Children.Clear();
            oyuncuKartlarYatay.Children.Clear();
            oyuncu.Kartlar = new List<Kart>();
            kurpiyer.Kartlar = new List<Kart>();
            oyuncu.BahisKoy(Convert.ToDecimal(bahis));
            if (sesAcikMi)
            {
                _playerShuffleCards.Play();
            }
            // Deste oluşturulur.
            deste = Kart.DesteOlustur();

            oyun.Oyna(deste, oyuncu, kurpiyer);

            kartCekLabel.IsEnabled = kartCekImage.IsEnabled = durLabel.IsEnabled = durImage.IsEnabled = true;
            oyunuBaslatLabel.IsEnabled = oyunuBaslatImage.IsEnabled = false;
            oyunuBaslatImage.Opacity = 50;
            durImage.Opacity = 100;
            kartCekImage.Opacity = 100;
        };

        oyunuBaslatView.Add(oyunuBaslatImage);
        oyunuBaslatView.Add(oyunuBaslatLabel);
        oyunuBaslatImage.GestureRecognizers.Add(tapGestureRecognizer);
        oyunuBaslatView.GestureRecognizers.Add(tapGestureRecognizer);

        VerticalStackLayout kartCekView = new VerticalStackLayout() { Margin = 10 };
        var kartCekRecognizer = new TapGestureRecognizer();
        kartCekRecognizer.Tapped += (s, e) =>
        {
            if (oyunuBaslatLabel.IsEnabled && oyunuBaslatImage.IsEnabled)
            {
                return;
            }
            if (sesAcikMi)
            {
                _playerFlipCard.Play();
            }

            oyuncu.KartCek(deste);
            if (oyuncu.Puan > 21)
            {
                oyun.OyunuBitir();
            }
        };

        kartCekView.Add(kartCekImage);
        kartCekView.Add(kartCekLabel);
        kartCekImage.GestureRecognizers.Add(kartCekRecognizer);
        kartCekView.GestureRecognizers.Add(kartCekRecognizer);

        VerticalStackLayout durView = new VerticalStackLayout() { Margin = 10 };
        var durRecognizer = new TapGestureRecognizer();
        durRecognizer.Tapped += (s, e) =>
        {
            if (oyunuBaslatLabel.IsEnabled && oyunuBaslatImage.IsEnabled)
            {
                return;
            }
            if (sesAcikMi)
            {
                _playerAnalog.Play();
            }
            while (kurpiyer.Puan < 17)
            {
                kurpiyer.KartCek(deste);
            }
            oyun.OyunuBitir();
        };

        durView.Add(durImage);
        durView.Add(durLabel);
        durImage.GestureRecognizers.Add(durRecognizer);
        durView.GestureRecognizers.Add(durRecognizer);

        VerticalStackLayout sesKontrolView = new VerticalStackLayout() { Margin = 10 };
        var sesKontrolRecognizer = new TapGestureRecognizer();
        sesKontrolRecognizer.Tapped += (s, e) =>
        {
            if (sesAcikMi)
            {
                _playerAnalog.Play();
            }

            sesAcikMi = !_sesAcikMi;
        };

        sesKontrolView.Add(sesKontrolImage);
        sesKontrolView.Add(sesKontrolLabel);
        sesKontrolImage.GestureRecognizers.Add(sesKontrolRecognizer);
        sesKontrolView.GestureRecognizers.Add(sesKontrolRecognizer);

        bottomBar.Children.Add(oyunuBaslatView);
        bottomBar.Children.Add(kartCekView);
        bottomBar.Children.Add(durView);
        // TODO: tepeye taşınacak (ya da menu) bottomBar.Children.Add(sesKontrolView);

        Content = grid;
    }

    private void Oyuncu_SplitYapildi(object sender, SplitYapildiEventArgs e)
    {
        oyuncuKartlarYatay.Clear();

        oyuncuKartlariStackLayout.Children.Add(oyuncuSplitKartlarYatay);

        foreach (Kart kart in oyuncu.Kartlar)
        {
            oyuncuKartlarYatay.Children.Add(CardImage(e.Kartlar.ToString()));
        }

        foreach (Kart kart in oyuncu.SplitKartlar)
        {
            oyuncuSplitKartlarYatay.Children.Add(CardImage(e.Kartlar.ToString()));
        }
    }

    private void Oyun_OyunBitti(object sender, OyunBittiEventArgs e)
    {
        string playerMessage = string.Empty;
        kurpiyerKartlarYatay.Children[0] = CardImage(kurpiyer.Kartlar[0].ToString());
        IAudioPlayer player = null;
        kartCekLabel.IsEnabled = kartCekImage.IsEnabled = durLabel.IsEnabled = durImage.IsEnabled = false;
        oyunuBaslatLabel.IsEnabled = oyunuBaslatImage.IsEnabled = true;
        switch (e.KazanmaDurumu)
        {
            case KazanmaDurumu.OyuncuKazandi:
                player = _playerWin;
                bakiye = bakiye + oyuncu.Bahis;
                playerMessage = "Tebrikler. Kazandın!!";
                oyuncuSkor++;
                break;
            case KazanmaDurumu.KurpiyerKazandi:
                player = _playerLose;
                bakiye -= oyuncu.Bahis;
                playerMessage = "Üzgünüm. Kaybettin!!";
                kurpiyerSkor++;
                break;
            case KazanmaDurumu.Beraberlik:
                player = _playerGasp;
                playerMessage = "Beraberlik!!";
                break;
            case KazanmaDurumu.OyuncuBlackjackYapti:
                player = _playerApplause;
                bakiye = bakiye + (oyuncu.Bahis * 2);
                playerMessage = "TEBRİKLER! Blackjack yaptın!!";
                oyuncuSkor++;
                break;
        }

        if (sesAcikMi)
        {
            player?.Play(); // null check gerekiyor. hata vermiyor. 
        }

        oyuncuPuanLabel.Text = $"{oyuncuSkor}";
        kurpiyerPuanLabel.Text = $"{kurpiyerSkor}";
        kasaLabel.Text = $"{bakiye}";

        _ = DisplayAlert("Oyun Bitti", $"{playerMessage}\r\nOyuncu Kartları Puanı: {e.OyuncuPuan}\r\nKurpiyer Kartları Puanı: {e.KurpiyerPuan}", "Tamam");
    }

    private void Kurpiyer_KartCekildi(object sender, Kurpiyer.KartCekildiEventArgs e)
    {
        Image image = CardImage("card_back");
        if (kurpiyer.Kartlar.Count > 1)
        {
            image.Source = ImageSource.FromFile($"{e.CekilenKart.ToString().ToLower().Replace(" ", "_")}.png");
        }

        kurpiyerKartlarYatay.Children.Add(image);
    }

    private void Oyuncu_KartCekildi(object sender, KartCekildiEventArgs e)
    {
        Image image = CardImage(e.CekilenKart.ToString());

        if (e.SplitKartMi)
        {
            oyuncuSplitKartlarYatay.Children.Add(image);
        }
        else
        {
            oyuncuKartlarYatay.Children.Add(image);
        }

        if (oyuncu.SplitYapabilir)
        {
            for (int i = 0; i < bottomBar.Children.Count; i++)
            {
                if ((bottomBar.Children[i] as StackBase).Id == splitViewGuid)
                {
                    return;
                }
            }

            VerticalStackLayout splitYapView = new VerticalStackLayout() { Margin = 10 };
            splitViewGuid = splitYapView.Id;
            var splitRecognizer = new TapGestureRecognizer();
            splitRecognizer.Tapped += (s, e) =>
            {
                if (sesAcikMi)
                {
                    _playerFlipCard.Play();
                }

                oyuncu.SplitYap();
            };

            splitYapView.Add(splitImage);
            splitYapView.Add(splitLabel);
            splitImage.GestureRecognizers.Add(splitRecognizer);
            splitYapView.GestureRecognizers.Add(splitRecognizer);
            bottomBar.Children.Add(splitYapView);
        }
        else
        {
            for (int i = 0; i < bottomBar.Children.Count; i++)
            {
                if ((bottomBar.Children[i] as StackBase).Id == splitViewGuid)
                {
                    bottomBar.Remove(bottomBar.Children[i]);
                }
            }
        }
    }

    private void Oyun_OyunBasladi(object sender, EventArgs e)
    {

    }

    private Image CardImage(string cardName)
    {
        Image image = new Image();
        image.Source = ImageSource.FromFile($"{cardName.ToLower().Replace(" ", "_")}.png");
        image.Margin = new Thickness(10);
        image.HeightRequest = 200;
        image.HorizontalOptions = LayoutOptions.Center;

        return image;
    }
}


