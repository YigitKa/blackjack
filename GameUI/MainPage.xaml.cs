using GameCore;

namespace GameUI;

public partial class MainPage : ContentPage
{
	int count = 0;

	public MainPage()
	{


        // Deste oluşturulur.
        List<Kart> deste = Kart.DesteOlustur();

        // Oyuncu ve Kurpiyer nesneleri oluşturulur.
      Oyuncu  oyuncu = new Oyuncu(100);
      Kurpiyer  kurpiyer = new Kurpiyer();

        // Oyun başlangıcında bahis alınır.
        oyuncu.BahisKoy(100);


        // Oyun oynanır.
        Blackjack blackjack = new Blackjack();

        blackjack.Oyna(deste, oyuncu, kurpiyer);

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

        // Oyuncu kartları
        var oyuncuKartlariLabel = new Label { Text = "Oyuncu Kartları" };
        grid.Children.Add(oyuncuKartlariLabel);

        var oyuncuKartlariStackLayout = new StackLayout();
        grid.Children.Add(oyuncuKartlariStackLayout);

        // Kurpiyer kartları
        var kurpiyerKartlariLabel = new Label { Text = "Kurpiyer Kartları" };
        grid.Children.Add(kurpiyerKartlariLabel);

        var kurpiyerKartlariStackLayout = new StackLayout();
        grid.Children.Add(kurpiyerKartlariStackLayout);

        // Oyuncu puanı
        var oyuncuPuanLabel = new Label { Text = "Oyuncu Puanı: " + oyuncu.Puan };
        grid.Children.Add(oyuncuPuanLabel);

        // Kurpiyer puanı
        var kurpiyerPuanLabel = new Label { Text = "Kurpiyer Puanı: " + kurpiyer.Puan };
        grid.Children.Add(kurpiyerPuanLabel);

        // Butonlar
        var kartCekButonu = new Button { Text = "Kart Çek" };
        grid.Children.Add(kartCekButonu);

        var pasButonu = new Button { Text = "Pas" };
        grid.Children.Add(pasButonu);

        // Butonlara tıklama olayları
        kartCekButonu.Clicked += (sender, e) =>
        {
            oyuncu.KartCek(deste);
            oyuncuKartlariStackLayout.Children.Add(new Label { Text = "" });
            oyuncuPuanLabel.Text = "Oyuncu Puanı: " + oyuncu.Puan;

            oyun.OyunuBitir(oyuncu, kurpiyer);
        };

        pasButonu.Clicked += (sender, e) =>
        {
            // Oyuncu pas geçtiğinde yapılacak işlemler
        };

        Content = grid;
    }

	private void OnCounterClicked(object sender, EventArgs e)
	{
	
	}
}


