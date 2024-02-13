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

    int kurpiyerSkor = 0;
    int oyuncuSkor = 0;
    int bakiye = 0;

    Button durButonu = new Button
    {
        Text = "Dur",
        IsEnabled = false,
        WidthRequest = 200,
        HorizontalOptions = LayoutOptions.Center,
        VerticalOptions = LayoutOptions.Center,
        Margin = new Thickness(10)
    };

    Button kartCekButonu = new Button
    {
        Text = "Kart İste",
        IsEnabled = false,
        WidthRequest = 200,
        HorizontalOptions = LayoutOptions.Center,
        VerticalOptions = LayoutOptions.Center,
        Margin = new Thickness(10)
    };

    Button oyunuBaslat = new Button
    {
        Text = "Oyunu Başlat",
        WidthRequest = 200,
        HorizontalOptions = LayoutOptions.Center,
        VerticalOptions = LayoutOptions.Center,
        Margin = new Thickness(10),
    };


    Label bilgiLabel = new Label
    {
        Text = "Oyuncu: 0\nKurpiyer: 0\nBakiye: 0",
        HorizontalOptions = LayoutOptions.Center,
        VerticalOptions = LayoutOptions.Center,
        Margin = new Thickness(10),
        FontSize = 25,
    };

    public MainPage(IAudioManager audioManager)
    {
        this.BackgroundImageSource = ImageSource.FromFile("table.jpg");

        this.audioManager = audioManager;

        // Oyun başlangıcında bahis alınır.
        oyuncu.KartCekildi += Oyuncu_KartCekildi;
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
                new RowDefinition { Height = GridLength.Star },
                new RowDefinition { Height = GridLength.Auto },
                new RowDefinition { Height = GridLength.Star },
                new RowDefinition { Height = GridLength.Auto }
            }
        };
        Blackjack oyun = new Blackjack();
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
        bottomBar.Children.Add(oyunuBaslat);
        bottomBar.Children.Add(kartCekButonu);
        bottomBar.Children.Add(durButonu);

        oyunuBaslat.Clicked += async (sender, e) =>
        {
            var bahis = await DisplayPromptAsync("Bahis Gir", "Oynanacak bahis miktarını girin:", "OK", "Cancel", "100", -1, Keyboard.Numeric, "100");

            kurpiyerKartlarYatay.Children.Clear();
            oyuncuKartlarYatay.Children.Clear();
            oyuncu.Kartlar = new List<Kart>();
            kurpiyer.Kartlar = new List<Kart>();
            oyuncu.BahisKoy(Convert.ToInt32(bahis));

            IAudioPlayer player = audioManager.CreatePlayer(await FileSystem.OpenAppPackageFileAsync("shuffle-cards.mp3"));
            player.Play();
            // Deste oluşturulur.
            deste = Kart.DesteOlustur();

            oyun.Oyna(deste, oyuncu, kurpiyer);
            kartCekButonu.IsEnabled = durButonu.IsEnabled = true;
            oyunuBaslat.IsEnabled = false;
        };

        // Butonlara tıklama olayları
        kartCekButonu.Clicked += (sender, e) =>
        {
            oyuncu.KartCek(deste);

            if (oyuncu.Puan > 21)
            {
                oyun.OyunuBitir(oyuncu, kurpiyer);
            }
        };

        durButonu.Clicked += (sender, e) =>
        {
            while (kurpiyer.Puan < 17)
            {
                kurpiyer.KartCek(deste);
            }

            oyun.OyunuBitir(oyuncu, kurpiyer);
        };

        Content = grid;
    }

    private async void Oyun_OyunBitti(object sender, OyunBittiEventArgs e)
    {
        Image image = new Image();
        image.Source = ImageSource.FromFile($"{kurpiyer.Kartlar[0].ToString().ToLower().Replace(" ", "_")}.png");

        image.HeightRequest = 200;
        image.HorizontalOptions = LayoutOptions.Center;

        kurpiyerKartlarYatay.Children[0] = image;
        IAudioPlayer player = null;

        switch (e.KazanmaDurumu)
        {
            case KazanmaDurumu.OyuncuKazandi:
                player = audioManager.CreatePlayer(await FileSystem.OpenAppPackageFileAsync("win.mp3"));
                bakiye = bakiye + oyuncu.Bahis;
                oyuncuSkor++;
                break;
            case KazanmaDurumu.KurpiyerKazandi:
                player = audioManager.CreatePlayer(await FileSystem.OpenAppPackageFileAsync("lose.mp3"));
                bakiye -= oyuncu.Bahis;
                kurpiyerSkor++;
                break;
            case KazanmaDurumu.Beraberlik:
                break;
        }

        kartCekButonu.IsEnabled = durButonu.IsEnabled = false;
        oyunuBaslat.IsEnabled = true;

        player?.Play(); // null check gerekiyor. hata vermiyor. 
        bilgiLabel.Text = $"Oyuncu: {oyuncuSkor}\nKurpiyer: {kurpiyerSkor}\nBakiye: {bakiye}";
        
        _ = DisplayAlert("Oyun Bitti", $"{e.KazanmaDurumu}\r\nOyuncu Puanı: {e.OyuncuPuan}\r\nKurpiyer Puanı: {e.KurpiyerPuan}", "Tamam");
    }

    private void Kurpiyer_KartCekildi(object sender, Kurpiyer.KartCekildiEventArgs e)
    {
        Image image = new Image();
        image.HeightRequest = 200;
        image.HorizontalOptions = LayoutOptions.Center;
        image.Source = ImageSource.FromFile($"card_back.png");

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

        image.HeightRequest = 200;
        image.HorizontalOptions = LayoutOptions.Center;

        oyuncuKartlarYatay.Children.Add(image);
    }

    private void Oyun_OyunBasladi(object sender, EventArgs e)
    {

    }
}


