using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace WebBrowser
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        String currentUrl;
        bool favoritesBar;
        List<String> favorites = new List<String>();
        bool favesFirstCreation = true;
        public MainPage()
        {
            this.InitializeComponent();
            tbUrl.Text = ".../";
            currentUrl = "";
            favorites.Add("https://amazon.com");
            favorites.Add("https://nasa.gov");
            favoritesBar = false;
        }

        private void TextBlock_SelectionChanged(object sender, RoutedEventArgs e)
        {

        }

        private void btnHome_Click(object sender, RoutedEventArgs e)
        {
            String url = "https://www.yahoo.com";
            wvMain.Navigate(new Uri(url));
            updateTbUrl(url);
        }

        private void updateTbUrl(String url)
        {
            currentUrl = url;
            tbUrl.Text = url;
        }

        private void btnFavorites_Click(object sender, RoutedEventArgs e)
        {
            Button favorite = new Button();
            if (favoritesBar)
            {
                wvMain.Width = 1920;
                favoritesBar = false;
                btnFavoritesTemplate.Opacity = 0;
                btnFavoritesTemplate.IsEnabled = false;

                for (int i = 0; i < favorites.Count; i++)
                {
                    Object findBtn = Grid.FindName("favorite" + i);
                    if (findBtn is Button)
                    {
                        Button found = findBtn as Button;
                        found.Opacity = 0;
                        found.IsEnabled = false;
                    }
                }

            } 
            else
            {
                wvMain.Width = 1680;
                favoritesBar = true;
                btnFavoritesTemplate.Opacity = 100;
                btnFavoritesTemplate.IsEnabled = true;

                favorite.Opacity = 100;

                if (favesFirstCreation)
                {
                    for (int i = 0; i < favorites.Count; i++)
                    {
                        favorite = new Button();
                        favorite.Name = "favorite" + i;
                        favorite.VerticalAlignment = VerticalAlignment.Top;
                        favorite.Margin = new Thickness(0, 75 + 50 * i, 10, 0);
                        favorite.Content = favorites[i];
                        favorite.Width = 205;
                        favorite.Height = 30;
                        favorite.HorizontalAlignment = HorizontalAlignment.Right;
                        favorite.Click += favorite_click;
                        Grid.Children.Add(favorite);
                    }
                }

                else
                {
                    for (int i = 0; i < favorites.Count; i++)
                    {
                        Object findBtn = Grid.FindName("favorite" + i);
                        if (findBtn is Button)
                        {
                            Button found = findBtn as Button;
                            found.Opacity = 100;
                            found.IsEnabled = true;
                        }
                    }
                }
                
            }
        }

        private void favorite_click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            Object url = button.Content;
            wvMain.Navigate(new Uri((string)url));

            updateTbUrl((string)url);
        }

        private void tbUrl_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void btnFavoritesTemplate_Click(object sender, RoutedEventArgs e)
        {
            String url = "https://www.amazon.com";
            wvMain.Navigate(new Uri(url));

            updateTbUrl(url);
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            String url = tbUrl.Text;
            wvMain.Navigate(new Uri(url));
            updateTbUrl(url);
        }

        private void wvMain_SizeChanged(object sender, SizeChangedEventArgs e)
        {

        }
    }
}
