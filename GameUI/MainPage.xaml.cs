using GameCore;

namespace GameUI;

public partial class MainPage : ContentPage
{
    int count = 0;
    StackLayout kurpiyerKartlariStackLayout = new StackLayout();
    StackLayout oyuncuKartlariStackLayout = new StackLayout();
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
                new ColumnDefinition { Width = GridLength.Star },
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
        kurpiyerKartlariStackLayout.Children.Add(new Label { Text = "Kurpiyer Kartları" });

        // Oyuncu kartları
        grid.Add(oyuncuKartlariStackLayout, 0, 2);
        oyuncuKartlariStackLayout.Children.Add(new Label { Text = "Oyuncu Kartları" });

        // Bilgi alanı
        var bilgiLabel = new Label();
        grid.Add(bilgiLabel, 0, 3);

        // Butonlar
        var oyunuBaslat = new Button { Text = "Oyunu Başlat" };
        grid.Add(oyunuBaslat, 0, 4);

        // Butonlar
        var kartCekButonu = new Button { Text = "Kart İste", IsVisible = false };
        grid.Add(kartCekButonu, 0, 4);

        var durButonu = new Button { Text = "Dur", IsVisible = false };
        grid.Add(durButonu, 1, 4);

        oyunuBaslat.Clicked += (sender, e) =>
        {
            oyun.Oyna(deste, oyuncu, kurpiyer);
            kartCekButonu.IsVisible = durButonu.IsVisible = true;
            oyunuBaslat.IsVisible = false;
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
        (kurpiyerKartlariStackLayout.Children[1] as Label).Text  =  kurpiyer.Kartlar[0].ToString();
        DisplayAlert("Oyun Bitti", $"{e.KazanmaDurumu}\r\nOyuncu Puanı: {e.OyuncuPuan}\r\nKurpiyer Puanı: {e.KurpiyerPuan}", "Tamam");
    }

    private void Kurpiyer_KartCekildi(object sender, Kurpiyer.KartCekildiEventArgs e)
    {
        if (kurpiyer.Kartlar.Count > 1)
        {
            kurpiyerKartlariStackLayout.Children.Add(new Label { Text = e.CekilenKart.ToString() });
            return;
        }

        kurpiyerKartlariStackLayout.Children.Add(new Label { Text = "****KAPALI KART*****" });
    }

    private void Oyuncu_KartCekildi(object sender, KartCekildiEventArgs e)
    {
        oyuncuKartlariStackLayout.Children.Add(new Label { Text = e.CekilenKart.ToString() });
    }

    private void Oyun_OyunBasladi(object sender, EventArgs e)
    {

    }

    private void OnCounterClicked(object sender, EventArgs e)
    {

    }
}


