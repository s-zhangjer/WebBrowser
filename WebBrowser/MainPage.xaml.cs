using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
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
        List<String> history = new List<String>();
        int historyIndex = -1;
        bool favesFirstCreation = true;

        bool backNavigate = false;
        bool frontNavigate = false;
        bool addCurrentFav = false;
        bool removeCurrentFav = false;
        public MainPage()
        {
            this.InitializeComponent();
            tbUrl.Text = "https://www.";
            currentUrl = "";
            favoritesBar = false;
            wvMain.Width = 2560;
            wvMain.Height = 1440;

            btnAddFavorites.Opacity = 0;
            btnRemoveFavorites.Opacity = 0;
            tbAddRemoveURL.Opacity = 0;
            btnAddRemoveFavs.Opacity = 0;
            tbCopyCurrentURLFav.Opacity = 0;
            btnSaveFavorites.Opacity = 0;
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
                btnRemoveFavorites.Opacity = 0;
                btnRemoveFavorites.IsEnabled = false;
                tbAddRemoveURL.Opacity = 0;
                tbAddRemoveURL.IsEnabled = false;
                btnAddRemoveFavs.Opacity = 0;
                btnAddRemoveFavs.IsEnabled = false;
                tbCopyCurrentURLFav.Opacity = 0;
                tbCopyCurrentURLFav.IsEnabled = false;
                addCurrentFav = false;
                removeCurrentFav = false;
                btnSaveFavorites.Opacity = 0;
                btnSaveFavorites.IsEnabled = false;

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
                btnAddFavorites.Margin = new Thickness(0, 75 + 40 * favorites.Count, 115, 0);
                btnRemoveFavorites.Opacity = 100;
                btnRemoveFavorites.IsEnabled = true;
                btnRemoveFavorites.Margin = new Thickness(0, 75 + 40 * favorites.Count, 10, 0);
                btnSaveFavorites.Opacity = 100;

                tbAddRemoveURL.Margin = new Thickness(0, 115 + 40 * favorites.Count, 42, 0);
                btnAddRemoveFavs.Margin = new Thickness(0, 115 + 40 * favorites.Count, 10, 0);
                tbCopyCurrentURLFav.Margin = new Thickness(0, 150 + 40 * favorites.Count, 10, 0);

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
            favorite.Margin = new Thickness(0, 75 + 40 * i, 10, 0);
            favorite.Tag = favorites[i];

            btnAddFavorites.Margin = new Thickness(0, 75 + 40 * favorites.Count, 115, 0);
            btnRemoveFavorites.Margin = new Thickness(0, 75 + 40 * favorites.Count, 10, 0);
            tbAddRemoveURL.Margin = new Thickness(0, 115 + 40 * favorites.Count, 42, 0);
            btnAddRemoveFavs.Margin = new Thickness(0, 115 + 40 * favorites.Count, 10, 0);
            tbCopyCurrentURLFav.Margin = new Thickness(0, 150 + 40 * favorites.Count, 10, 0);

            if (favorites[i].Contains("www")){
                String shortenedUrl = favorites[i].Substring(favorites[i].IndexOf('w') + 4, favorites[i].Length - (favorites[i].IndexOf('w') + 4));
                shortenedUrl = shortenedUrl.Substring(0, shortenedUrl.Length - (shortenedUrl.Length - shortenedUrl.IndexOf('.')));
                shortenedUrl = shortenedUrl.Substring(0, 1).ToUpper() + shortenedUrl.Substring(1).ToLower();
                favorite.Content = shortenedUrl;
            } else
            {
                String shortenedUrl = favorites[i].Substring(favorites[i].IndexOf('/') + 2, favorites[i].Length - (favorites[i].IndexOf('/') + 4));
                shortenedUrl = shortenedUrl.Substring(0, shortenedUrl.Length - (shortenedUrl.Length - shortenedUrl.IndexOf('.')));
                shortenedUrl = shortenedUrl.Substring(0, 1).ToUpper() + shortenedUrl.Substring(1).ToLower();
                favorite.Content = shortenedUrl;
            }

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
            wvMain.Source = (new Uri((string)url));

            updateTbUrl((string)url);
        }

        private void btnAddRemoveFavs_Click(object sender, RoutedEventArgs e)
        {
            if(addCurrentFav)
            {
                String url = tbAddRemoveURL.Text;

                if (!favorites.Contains(url))
                {
                    if(!url.Substring(url.Length - 2, 1).Equals("/")){
                        favorites.Add(url + "/");
                    }
                    else
                    {
                        favorites.Add(url);
                    }
                    createFavButton(favorites.Count - 1);
                }

                tbAddRemoveURL.Text = "https://www.";
            }

            else if (removeCurrentFav)
            {
                String url = tbAddRemoveURL.Text;

                if (favorites.Contains(url))
                {
                    removeFavoriteBtn(url);
                }

                tbAddRemoveURL.Text = "https://www.";
            }
        }

        public void removeFavoriteBtn(String url)
        {
            for (int i = 0; i < favorites.Count; i++)
            {
                Object findBtn = Grid.FindName("favorite" + i);
                if (findBtn is Button)
                {
                    Button found = findBtn as Button;
                    Grid.Children.Remove(found);
                }
            }

            favorites.Remove(url);

            for (int i = 0; i < favorites.Count; i++)
            {
                Object findBtn = Grid.FindName("favorite" + i);
                if (findBtn is Button)
                {
                    Button found = findBtn as Button;
                    Grid.Children.Remove(found);
                }
            }

            for (int i = 0; i < favorites.Count; i++)
            {
                createFavButton(i);
            }


            btnAddFavorites.Margin = new Thickness(0, 75 + 40 * favorites.Count, 115, 0);
            btnRemoveFavorites.Opacity = 100;
            btnRemoveFavorites.IsEnabled = true;
            btnRemoveFavorites.Margin = new Thickness(0, 75 + 40 * favorites.Count, 10, 0);

            tbAddRemoveURL.Margin = new Thickness(0, 115 + 40 * favorites.Count, 42, 0);
            btnAddRemoveFavs.Margin = new Thickness(0, 115 + 40 * favorites.Count, 10, 0);
        }

        private void btnAddFavorites_Click(object sender, RoutedEventArgs e)
        {
            if (!addCurrentFav)
            {
                tbAddRemoveURL.Opacity = 100;
                tbAddRemoveURL.IsEnabled = true;

                btnAddRemoveFavs.Opacity = 100;
                btnAddRemoveFavs.IsEnabled = true;

                tbCopyCurrentURLFav.Opacity = 100;
                tbCopyCurrentURLFav.IsEnabled = true;

                addCurrentFav = true;
                removeCurrentFav = false;
            }
            else
            {
                tbAddRemoveURL.Opacity = 0;
                tbAddRemoveURL.IsEnabled = false;

                btnAddRemoveFavs.Opacity = 0;
                btnAddRemoveFavs.IsEnabled = false;

                tbCopyCurrentURLFav.Opacity = 0;
                tbCopyCurrentURLFav.IsEnabled = false;

                addCurrentFav = false;
                removeCurrentFav = false;
            }

        }

        private void btnRemoveFavorites_Click(object sender, RoutedEventArgs e)
        {
            if (!removeCurrentFav)
            {
                tbAddRemoveURL.Opacity = 100;
                tbAddRemoveURL.IsEnabled = true;

                btnAddRemoveFavs.Opacity = 100;
                btnAddRemoveFavs.IsEnabled = true;

                tbCopyCurrentURLFav.Opacity = 100;
                tbCopyCurrentURLFav.IsEnabled = true;

                removeCurrentFav = true;
                addCurrentFav = false;
            }
            else
            {
                tbAddRemoveURL.Opacity = 0;
                tbAddRemoveURL.IsEnabled = false;

                btnAddRemoveFavs.Opacity = 0;
                btnAddRemoveFavs.IsEnabled = false;

                tbCopyCurrentURLFav.Opacity = 0;
                tbCopyCurrentURLFav.IsEnabled = false;

                removeCurrentFav = false;
                addCurrentFav = false;
            }
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            if (history.Count() > 0 && historyIndex > 0)
            {
                historyIndex--;
                String url = history[historyIndex];
                wvMain.Source = new Uri(url);
                backNavigate = true;
            }
        }

        private void btnForward_Click(object sender, RoutedEventArgs e)
        {
            if (history.Count() > 0 && historyIndex < history.Count() - 1)
            {
                historyIndex++;
                String url = history[historyIndex];
                wvMain.Source = new Uri(url);
                frontNavigate = true;
            }
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            searchFunction();
        }

        private void searchFunction()
        {
            String url = tbUrl.Text;
            wvMain.Source = (new Uri(url));
            updateTbUrl(url);
        }

        private void tbUrl_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                searchFunction();
            }
        }

        private void btnHome_Click(object sender, RoutedEventArgs e)
        {
            String url = "https://www.yahoo.com";
            wvMain.Source = (new Uri(url));
            updateTbUrl(url);
        }

        private void tbCopyCurrentURLFav_Click(object sender, RoutedEventArgs e)
        {
            tbAddRemoveURL.Text = tbUrl.Text;
        }

        private void wvMain_NavigationCompleted(Microsoft.UI.Xaml.Controls.WebView2 sender, Microsoft.Web.WebView2.Core.CoreWebView2NavigationCompletedEventArgs args)
        {
            String url = wvMain.Source.ToString();
            tbUrl.Text = url;

            if (!backNavigate && !frontNavigate)
            {
                history.Insert(historyIndex + 1, url);
                historyIndex++;
            }
            else
            {
                backNavigate = false;
                frontNavigate = false;
            }
        }
        private void wvMain_Loaded(object sender, RoutedEventArgs e)
        {
            LoadSaveData();
        }

        private void btnSaveFavorites_Click(object sender, RoutedEventArgs e)
        {
            saveFile();
        }

        public async Task<String> saveFile()
        {
            Windows.Storage.StorageFolder storageFolder =
                    Windows.Storage.ApplicationData.Current.LocalFolder;
            Windows.Storage.StorageFile savedFavoritesFile =
                    await storageFolder.CreateFileAsync("savedFavorites.txt",
                    Windows.Storage.CreationCollisionOption.ReplaceExisting);
            String data = getSaveData();

            await Windows.Storage.FileIO.WriteTextAsync(savedFavoritesFile, data);

            return data;
        }

        public String getSaveData()
        {
            String favoritesString = "";
            for(int i = 0; i < favorites.Count; i++)
            {
                favoritesString += favorites[i].ToString() + "\n";
            }
            return favoritesString.Substring(0, favoritesString.Length - 1);
        }

        private async void LoadSaveData()
        {
            try
            {
                Windows.Storage.StorageFolder storageFolder =
                        Windows.Storage.ApplicationData.Current.LocalFolder;
                Windows.Storage.StorageFile savedFavoritesFile =
                    await storageFolder.GetFileAsync("savedFavorites.txt");

                string text = await Windows.Storage.FileIO.ReadTextAsync(savedFavoritesFile);

                String[] favoritesString = text.Split('\n');
                if(favoritesString.Length < 2)
                {
                    favorites.Insert(0, "https://www.amazon.com/");
                    favorites.Insert(1, "https://www.nasa.gov/");
                }

                else if (!favoritesString.Contains("https://www.amazon.com/"))
                {
                    favorites.Insert(0, "https://www.amazon.com/");
                    if(!favoritesString.Contains("https://www.nasa.gov/"))
                    {
                        favorites.Insert(1, "https://www.nasa.gov/");
                    }
                }
                foreach (var line in favoritesString)
                {
                    favorites.Add(line.Trim());
                }
            }
            catch (Exception)
            {
                // This is just in case the file doesn't exist
                // this is important on the FIRST run, and in case something goes wrong!
            }
        }


        private void wvMain_NavigationStarting(Microsoft.UI.Xaml.Controls.WebView2 sender, Microsoft.Web.WebView2.Core.CoreWebView2NavigationStartingEventArgs args)
        {
        }

        private void TextBlock_SelectionChanged(object sender, RoutedEventArgs e)
        {
        }
        private void tbUrl_TextChanged(object sender, TextChangedEventArgs e)
        {
        }
    }
}
