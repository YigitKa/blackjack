using GameCore;

namespace GameUI;

public class GameHistory : ContentPage
{
	public GameHistory(List<BitenOyun> bitenOyunlar)
	{
        // Sayfa içeriğini runtime'da oluşturuyoruz

        var stackLayout = new StackLayout();

        var oyunlarListesi = new ListView();
        oyunlarListesi.ItemsSource = bitenOyunlar;

        var dataTemplate = new DataTemplate(() =>
        {
            var viewCell = new ViewCell();

            var horizontalStackLayout = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Padding = new Thickness(10)
            };

            var oyuncuKartDesiFotograf = new Image
            {
                WidthRequest = 50,
                HeightRequest = 50
            };

            var verticalStackLayout = new StackLayout
            {
                VerticalOptions = LayoutOptions.Center
            };

            var oyuncuPuanLabel = new Label
            {
                FontSize = 16
            };

            var masaPuanLabel = new Label
            {
                FontSize = 16
            };

            var maçSonucuLabel = new Label
            {
                FontSize = 16,
                HorizontalOptions = LayoutOptions.End
            };

            verticalStackLayout.Children.Add(oyuncuPuanLabel);
            verticalStackLayout.Children.Add(masaPuanLabel);

            horizontalStackLayout.Children.Add(oyuncuKartDesiFotograf);
            horizontalStackLayout.Children.Add(verticalStackLayout);
            horizontalStackLayout.Children.Add(maçSonucuLabel);

            viewCell.View = horizontalStackLayout;

            return viewCell;
        });

        oyunlarListesi.ItemTemplate = dataTemplate;

        stackLayout.Children.Add(oyunlarListesi);

        Content = stackLayout;
    }
}
