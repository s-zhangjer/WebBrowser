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
            tbUrl.Text = "https://www.";
            currentUrl = "";
            favorites.Add("https://www.amazon.com");
            favorites.Add("https://www.nasa.gov");
            favoritesBar = false;
            wvMain.Width = 2560;
            btnAddFavorites.Opacity = 0;
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
                wvMain.Width += 240;
                favoritesBar = false;
                btnAddFavorites.Opacity = 0;
                btnAddFavorites.IsEnabled = false;

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
                wvMain.Width -= 240;
                favoritesBar = true;
                btnAddFavorites.Opacity = 100;
                btnAddFavorites.IsEnabled = true;
                btnAddFavorites.Margin = new Thickness(0, 75 + 50 * favorites.Count, 10, 0);

                favorite.Opacity = 100;

                if (favesFirstCreation)
                {
                    for (int i = 0; i < favorites.Count; i++)
                    {
                        createFavButton(i);
                    }
                }

                else
                {
                    for (int i = 0; i < favorites.Count; i++)
                    {
                        Object findBtn = Grid.FindName("BtnFavorite" + i);
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

        void createFavButton(int i)
        {
            Button favorite = new Button();
            favorite.Name = "favorite" + i;
            favorite.VerticalAlignment = VerticalAlignment.Top;
            favorite.Margin = new Thickness(0, 75 + 50 * i, 10, 0);
            favorite.Tag = favorites[i];

            String shortenedUrl = favorites[i].Substring(favorites[i].IndexOf('w') + 4, favorites[i].Length - (favorites[i].IndexOf('w') + 4)).ToUpper();
            shortenedUrl = shortenedUrl.Substring(0, shortenedUrl.Length - (shortenedUrl.Length - shortenedUrl.IndexOf('.')));
            favorite.Content = shortenedUrl;

            favorite.Width = 205;
            favorite.Height = 30;
            favorite.HorizontalAlignment = HorizontalAlignment.Right;
            favorite.Click += favorite_click;
            Grid.Children.Add(favorite);
        }

        private void favorite_click(object sender, RoutedEventArgs e)
        {
            Button button = (Button) sender;
            Object url = button.Tag;
            wvMain.Navigate(new Uri((string)url));

            updateTbUrl((string)url);
        }

        private void tbUrl_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void btnFavoritesTemplate_Click(object sender, RoutedEventArgs e)
        {
            String url = tbUrl.Text;

            if (!favorites.Contains(url))
            {
                favorites.Add(url);
                createFavButton(favorites.Count - 1);
                btnAddFavorites.Margin = new Thickness(0, 75 + 50 * favorites.Count, 10, 0);
            }
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            searchFunction();
        }

        private void searchFunction()
        {
            String url = tbUrl.Text;
            wvMain.Navigate(new Uri(url));
            updateTbUrl(url);
        }

        private void tbUrl_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                searchFunction();
            }
        }
    }
}
