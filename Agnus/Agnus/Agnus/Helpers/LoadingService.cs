using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agnus.Helpers
{
    public class LoadingService
    {
        private static ContentView _overlay;
        private static ActivityIndicator _indicator;

        public static void ShowLoading(Page page, string message = "Carregando...")
        {
            if (_overlay != null) return;

            // Garante que estamos tratando uma ContentPage
            if (page is not ContentPage contentPage)
                return;

            if (contentPage.Content is not Layout layout)
                return;

            var grid = new Grid
            {
                BackgroundColor = new Color(0, 0, 0, 0.4f),
                VerticalOptions = LayoutOptions.Fill,
                HorizontalOptions = LayoutOptions.Fill
            };

            _indicator = new ActivityIndicator
            {
                IsRunning = true,
                Color = Colors.White,
                WidthRequest = 50,
                HeightRequest = 50,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center
            };

            var label = new Label
            {
                Text = message,
                TextColor = Colors.White,
                FontSize = 14,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                Margin = new Thickness(0, 60, 0, 0)
            };

            grid.Children.Add(_indicator);
            grid.Children.Add(label);

            _overlay = new ContentView
            {
                Content = grid,
                InputTransparent = false // Bloqueia cliques
            };

            layout.Children.Add(_overlay);
        }

        public static void HideLoading(Page page)
        {
            if (_overlay == null) return;

            if (page is ContentPage contentPage && contentPage.Content is Layout layout)
            {
                layout.Children.Remove(_overlay);
            }

            _overlay = null;
            _indicator = null;
        }
    }
}
