using GameCore;

namespace GameUI;

public partial class MainPage : ContentPage
{
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
        kurpiyerKartlarYatay.HorizontalOptions = LayoutOptions.Center;
        kurpiyerKartlarYatay.VerticalOptions = LayoutOptions.Center;

        // Oyuncu kartları
        grid.Add(oyuncuKartlariStackLayout, 0, 2);
        oyuncuKartlariStackLayout.Children.Add(new Label { Text = "Oyuncu Kartları", HorizontalTextAlignment = TextAlignment.Center, VerticalTextAlignment = TextAlignment.Center });
        oyuncuKartlariStackLayout.Children.Add(oyuncuKartlarYatay);
        oyuncuKartlarYatay.HorizontalOptions = LayoutOptions.Center;
        oyuncuKartlarYatay.VerticalOptions = LayoutOptions.Center;

        // Bilgi alanı
        var bilgiLabel = new Label();
        grid.Add(bilgiLabel, 0, 3);

        grid.Add(bottomBar, 0, 4);
        bottomBar.HorizontalOptions = LayoutOptions.Center;
        bottomBar.VerticalOptions = LayoutOptions.Center;
        
        // Butonlar
        var oyunuBaslat = new Button {
            Text = "Oyunu Başlat",
            WidthRequest = 200,
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center,
            Margin = new Thickness(10) };
        bottomBar.Children.Add(oyunuBaslat);

        var kartCekButonu = new Button {
            Text = "Kart İste",
            IsEnabled = false,
            WidthRequest = 200,
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center,
            Margin = new Thickness(10) };
        bottomBar.Children.Add(kartCekButonu);

        var durButonu = new Button {
            Text = "Dur",
            IsEnabled = false,
            WidthRequest = 200,
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center,
            Margin = new Thickness(10) };
        bottomBar.Children.Add(durButonu);

        oyunuBaslat.Clicked += async (sender, e) =>
        {
            var bahis = await DisplayPromptAsync("Bahis Gir", "Oynanacak bahis miktarını girin:", "OK", "Cancel", "100", -1, Keyboard.Numeric, "100");
          
            oyuncu.BahisKoy(Convert.ToInt32(bahis));
            
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
}


