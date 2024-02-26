using GameCore;

namespace GameUI;

public class GameHistory : ContentPage
{
	public GameHistory(List<BitenOyun> bitenOyunlar)
	{
		Content = new VerticalStackLayout
		{
			Children =
			{
				new Label
				{
					HorizontalOptions = LayoutOptions.Center,
					VerticalOptions = LayoutOptions.Center,
					Text = "Welcome to .NET MAUI!"
				}
			}
		};
	}
}
