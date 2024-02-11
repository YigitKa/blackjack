using GameCore;

namespace GameUI;

public partial class MainPage : ContentPage
{
    int count = 0;
    StackLayout kurpiyerKartlariStackLayout = new StackLayout();
    StackLayout oyuncuKartlariStackLayout = new StackLayout();

    HorizontalStackLayout oyuncuKartlarYatay = new HorizontalStackLayout();
    HorizontalStackLayout kurpiyerKartlarYatay = new HorizontalStackLayout();
    HorizontalStackLayout bottomBar = new HorizontalStackLayout();
    // Oyuncu ve Kurpiyer nesneleri oluşturulur.
    Oyuncu oyuncu = new Oyuncu(100);
    Kurpiyer kurpiyer = new Kurpiyer();

    public MainPage()
    {
        // Deste oluşturulur.
        List<Kart> deste = Kart.DesteOlustur();

        // Oyun başlangıcında bahis alınır.
        oyuncu.BahisKoy(100);
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
                new RowDefinition { Height = GridLength.Auto },
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
        kurpiyerKartlariStackLayout.Children.Add(new Label { Text = "Kurpiyer Kartları", HorizontalTextAlignment = TextAlignment.Center, VerticalTextAlignment = TextAlignment.Center });
        kurpiyerKartlariStackLayout.Children.Add(kurpiyerKartlarYatay);

        // Oyuncu kartları
        grid.Add(oyuncuKartlariStackLayout, 0, 2);
        oyuncuKartlariStackLayout.Children.Add(new Label { Text = "Oyuncu Kartları", HorizontalTextAlignment = TextAlignment.Center, VerticalTextAlignment = TextAlignment.Center });
        oyuncuKartlariStackLayout.Children.Add(oyuncuKartlarYatay);

        // Bilgi alanı
        var bilgiLabel = new Label();
        grid.Add(bilgiLabel, 0, 3);

        // Butonlar
        var oyunuBaslat = new Button { Text = "Oyunu Başlat", MaximumWidthRequest = 200, HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center };
        grid.Add(bottomBar, 0, 4);
        bottomBar.Children.Add(oyunuBaslat);
        // Butonlar
        var kartCekButonu = new Button { Text = "Kart İste", IsEnabled = false, MaximumWidthRequest = 200, HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center };
        bottomBar.Children.Add(kartCekButonu);

        var durButonu = new Button { Text = "Dur", IsEnabled = false, MaximumWidthRequest = 200, HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center };
        bottomBar.Children.Add(durButonu);

        oyunuBaslat.Clicked += (sender, e) =>
        {
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

    private void Oyun_OyunBitti(object sender, OyunBittiEventArgs e)
    {
        Image image = new Image();
        image.Source = ImageSource.FromFile($"{kurpiyer.Kartlar[0].ToString().ToLower().Replace(" ", "_")}.png");

        image.HeightRequest = 200;
        image.HorizontalOptions = LayoutOptions.Center;

        kurpiyerKartlarYatay.Children[0] = image;

        DisplayAlert("Oyun Bitti", $"{e.KazanmaDurumu}\r\nOyuncu Puanı: {e.OyuncuPuan}\r\nKurpiyer Puanı: {e.KurpiyerPuan}", "Tamam");
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

    private void OnCounterClicked(object sender, EventArgs e)
    {

    }
}


