using GameCore;
using Plugin.Maui.Audio;

namespace GameUI;

public partial class MainPage : ContentPage
{
    private readonly IAudioManager audioManager;

    StackLayout kurpiyerKartlariStackLayout = new StackLayout();
    StackLayout oyuncuKartlariStackLayout = new StackLayout();

    HorizontalStackLayout oyuncuKartlarYatay = new HorizontalStackLayout();
    HorizontalStackLayout kurpiyerKartlarYatay = new HorizontalStackLayout();
    HorizontalStackLayout bottomBar = new HorizontalStackLayout();

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
        Source = "sword.png",
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

    Label bilgiLabel = new Label
    {
        Text = "Oyuncu: 0\nKurpiyer: 0\nBakiye: 0",
        Margin = new Thickness(10),
        FontSize = 25,
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

        // Oyun başlangıcında bahis alınır.
        oyuncu.KartCekildi += Oyuncu_KartCekildi;
        oyuncu.SplitYapildi += Oyuncu_SplitYapildi;
        kurpiyer.KartCekildi += Kurpiyer_KartCekildi;
        // Oyun oynanır.
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
        // Kurpiyer kartları
        grid.Add(kurpiyerKartlariStackLayout, 0, 0);
        kurpiyerKartlariStackLayout.Children.Add(new Label { Text = "Kurpiyer Kartları", FontSize = 20, HorizontalTextAlignment = TextAlignment.Center, VerticalTextAlignment = TextAlignment.Center });
        kurpiyerKartlariStackLayout.Children.Add(kurpiyerKartlarYatay);
        kurpiyerKartlarYatay.HorizontalOptions = LayoutOptions.Center;
        kurpiyerKartlarYatay.VerticalOptions = LayoutOptions.Center;

        // Oyuncu kartları
        grid.Add(oyuncuKartlariStackLayout, 0, 1);
        oyuncuKartlariStackLayout.Children.Add(new Label { Text = "Oyuncu Kartları", FontSize = 20, HorizontalTextAlignment = TextAlignment.Center, VerticalTextAlignment = TextAlignment.Center });
        oyuncuKartlariStackLayout.Children.Add(oyuncuKartlarYatay);
        oyuncuKartlarYatay.HorizontalOptions = LayoutOptions.Center;
        oyuncuKartlarYatay.VerticalOptions = LayoutOptions.Center;

        // Bilgi alanı
        grid.Add(bilgiLabel, 0, 2);

        grid.Add(bottomBar, 0, 3);
        bottomBar.HorizontalOptions = LayoutOptions.Center;
        bottomBar.VerticalOptions = LayoutOptions.Center;

        // Butonlar
        VerticalStackLayout oyunuBaslatView = new VerticalStackLayout() { Margin = 10 };
        var tapGestureRecognizer = new TapGestureRecognizer();
        tapGestureRecognizer.Tapped +=  async (s, e) =>
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
        bottomBar.Children.Add(sesKontrolView);

        Content = grid;
    }

    private void Oyuncu_SplitYapildi(object sender, SplitYapildiEventArgs e)
    {

    }

    private async void Oyun_OyunBitti(object sender, OyunBittiEventArgs e)
    {
        Image image = new Image();
        image.Source = ImageSource.FromFile($"{kurpiyer.Kartlar[0].ToString().ToLower().Replace(" ", "_")}.png");

        image.HeightRequest = 200;
        image.HorizontalOptions = LayoutOptions.Center;
        string playerMessage = string.Empty;
        kurpiyerKartlarYatay.Children[0] = image;
        IAudioPlayer player = null;
        await Task.Yield();
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

        bilgiLabel.Text = $"Oyuncu: {oyuncuSkor}\nKurpiyer: {kurpiyerSkor}\nBakiye: {bakiye}";

        _ = DisplayAlert("Oyun Bitti", $"{playerMessage}\r\nOyuncu Kartları Puanı: {e.OyuncuPuan}\r\nKurpiyer Kartları Puanı: {e.KurpiyerPuan}", "Tamam");
    }

    private void Kurpiyer_KartCekildi(object sender, Kurpiyer.KartCekildiEventArgs e)
    {
        Image image = new Image();
        image.HeightRequest = 200;
        image.HorizontalOptions = LayoutOptions.Center;
        image.Source = ImageSource.FromFile($"card_back.png");
        image.Margin = new Thickness(10);
        if (kurpiyer.Kartlar.Count > 1)
        {
            image.Source = ImageSource.FromFile($"{e.CekilenKart.ToString().ToLower().Replace(" ", "_")}.png");
        }

        kurpiyerKartlarYatay.Children.Add(image);
    }

    private void Oyuncu_KartCekildi(object sender, KartCekildiEventArgs e)
    {
        Image image = new Image();
        image.Source = ImageSource.FromFile($"{e.CekilenKart.ToString().ToLower().Replace(" ", "_")}.png");
        image.Margin = new Thickness(10);
        image.HeightRequest = 200;
        image.HorizontalOptions = LayoutOptions.Center;

        oyuncuKartlarYatay.Children.Add(image);


        if (oyuncu.Kartlar.Count == 2 && (int)oyuncu.Kartlar[0].Deger == (int)oyuncu.Kartlar[1].Deger)
        {
            VerticalStackLayout splitYapView = new VerticalStackLayout() { Margin = 10 };
            splitViewGuid = splitYapView.Id;
            var sesKontrolRecognizer = new TapGestureRecognizer();
            sesKontrolRecognizer.Tapped += (s, e) =>
            {
                if (sesAcikMi)
                {
                    _playerFlipCard.Play();
                }

                oyuncu.SplitYap();
            };

            splitYapView.Add(splitImage);
            splitYapView.Add(splitLabel);
            splitImage.GestureRecognizers.Add(sesKontrolRecognizer);
            splitYapView.GestureRecognizers.Add(sesKontrolRecognizer);
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
}


