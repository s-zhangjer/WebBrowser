using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel.Chat;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using static System.Net.WebRequestMethods;

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
        bool refreshNavigate = false;
        bool lstHistoryNavigate = false;
        bool homepageNavigate = false;

        bool addCurrentFav = false;
        bool removeCurrentFav = false;
        bool firstLoadFromHistory = false;
        bool showLstHistory = false;
        bool settingsBar = false;

        String homepageUrl = "";
        
        public MainPage()
        {
            this.InitializeComponent();
            tbUrl.Text = "https://www.";
            currentUrl = "";
            favoritesBar = false;
            wvMain.Width = 2560;
            wvMain.Height = 1390;

            btnAddFavorites.Opacity = 0;
            btnRemoveFavorites.Opacity = 0;
            tbAddRemoveURL.Opacity = 0;
            btnAddRemoveFavs.Opacity = 0;
            btnCopyCurrentURLFav.Opacity = 0;
            btnSaveFavorites.Opacity = 0;
            btnSaveHistory.Opacity = 0;
            btnClearHistory.Opacity = 0;
            btnClearFavorites.Opacity = 0;
            lstHistory.Opacity = 0;
            btnShowHistory.Opacity = 0;     

            btnChangeBg.Opacity = 0;
            tbBgColor.Opacity = 0;
            tbHomepage.Opacity = 0;
            btnCopyHomepage.Opacity = 0;
            btnSetHomepage.Opacity = 0;
            btnSaveSettings.Opacity = 0;
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
                btnCopyCurrentURLFav.Opacity = 0;
                btnCopyCurrentURLFav.IsEnabled = false;
                addCurrentFav = false;
                removeCurrentFav = false;
                btnSaveFavorites.Opacity = 0;
                btnSaveFavorites.IsEnabled = false;
                btnSaveHistory.Opacity = 0;
                btnSaveHistory.IsEnabled = false;
                btnClearHistory.Opacity = 0;
                btnClearHistory.IsEnabled = false;
                btnClearFavorites.Opacity = 0;
                btnClearFavorites.IsEnabled = false;
                btnShowHistory.Opacity = 0;
                btnShowHistory.IsEnabled = false;

                lstHistory.Opacity = 0;
                lstHistory.IsEnabled = false;
                showLstHistory = false;

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
                btnSaveFavorites.IsEnabled = true;
                btnSaveHistory.Opacity = 100;
                btnSaveHistory.IsEnabled = true;
                btnClearHistory.Opacity = 100;
                btnClearHistory.IsEnabled = true;
                btnClearFavorites.Opacity = 100;
                btnClearFavorites.IsEnabled = true;
                btnShowHistory.Opacity = 100;
                btnShowHistory.IsEnabled = true;

                tbAddRemoveURL.Margin = new Thickness(0, 115 + 40 * favorites.Count, 42, 0);
                btnAddRemoveFavs.Margin = new Thickness(0, 115 + 40 * favorites.Count, 10, 0);
                btnCopyCurrentURLFav.Margin = new Thickness(0, 150 + 40 * favorites.Count, 10, 0);
                btnSaveFavorites.Margin = new Thickness(0, 0, 10, 45);
                btnSaveHistory.Margin = new Thickness(0, 0, 10, 10);
                btnClearHistory.Margin = new Thickness(0, 0, 10, 80);
                btnClearFavorites.Margin = new Thickness(0, 0, 10, 115);
                btnShowHistory.Margin = new Thickness(0, 0, 10, 150);

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
            btnCopyCurrentURLFav.Margin = new Thickness(0, 150 + 40 * favorites.Count, 10, 0);

            if (favorites[i].Contains("www"))
            {
                String shortenedUrl = favorites[i].Substring(favorites[i].IndexOf('w') + 4, favorites[i].Length - (favorites[i].IndexOf('w') + 4));
                shortenedUrl = shortenedUrl.Substring(0, shortenedUrl.Length - (shortenedUrl.Length - shortenedUrl.IndexOf('.')));
                shortenedUrl = shortenedUrl.Substring(0, 1).ToUpper() + shortenedUrl.Substring(1).ToLower();
                favorite.Content = shortenedUrl;
            }
            else
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
            String url = tbAddRemoveURL.Text;
            if (!url.Equals("https://www.") && url.Substring(12, url.Length - 12).Contains(".")) { 
                if (addCurrentFav)
                {
                    if (!url.Substring(url.Length - 1, 1).Equals("/"))
                    {
                        url += "/";
                    }

                    if (!favorites.Contains(url))
                    {
                        favorites.Add(url);
                        createFavButton(favorites.Count - 1);
                    }

                    tbAddRemoveURL.Text = "https://www.";
                }

                else if (removeCurrentFav)
                {
                    if (!url.Substring(url.Length - 1, 1).Equals("/"))
                    {
                        url += "/";
                    }

                    if (favorites.Contains(url))
                    {
                        removeFavoriteBtn(url);
                    }

                    tbAddRemoveURL.Text = "https://www.";
                }
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
                createFavButton(i);
            }


            btnAddFavorites.Margin = new Thickness(0, 75 + 40 * favorites.Count, 115, 0);
            btnRemoveFavorites.Opacity = 100;
            btnRemoveFavorites.IsEnabled = true;
            btnRemoveFavorites.Margin = new Thickness(0, 75 + 40 * favorites.Count, 10, 0);

            tbAddRemoveURL.Margin = new Thickness(0, 115 + 40 * favorites.Count, 42, 0);
            btnAddRemoveFavs.Margin = new Thickness(0, 115 + 40 * favorites.Count, 10, 0);
            btnCopyCurrentURLFav.Margin = new Thickness(0, 150 + 40 * favorites.Count, 10, 0);
        }

        private void btnAddFavorites_Click(object sender, RoutedEventArgs e)
        {
            if (!addCurrentFav)
            {
                tbAddRemoveURL.Opacity = 100;
                tbAddRemoveURL.IsEnabled = true;

                btnAddRemoveFavs.Opacity = 100;
                btnAddRemoveFavs.IsEnabled = true;

                btnCopyCurrentURLFav.Opacity = 100;
                btnCopyCurrentURLFav.IsEnabled = true;

                addCurrentFav = true;
                removeCurrentFav = false;
            }
            else
            {
                tbAddRemoveURL.Opacity = 0;
                tbAddRemoveURL.IsEnabled = false;

                btnAddRemoveFavs.Opacity = 0;
                btnAddRemoveFavs.IsEnabled = false;

                btnCopyCurrentURLFav.Opacity = 0;
                btnCopyCurrentURLFav.IsEnabled = false;

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

                btnCopyCurrentURLFav.Opacity = 100;
                btnCopyCurrentURLFav.IsEnabled = true;

                removeCurrentFav = true;
                addCurrentFav = false;
            }
            else
            {
                tbAddRemoveURL.Opacity = 0;
                tbAddRemoveURL.IsEnabled = false;

                btnAddRemoveFavs.Opacity = 0;
                btnAddRemoveFavs.IsEnabled = false;

                btnCopyCurrentURLFav.Opacity = 0;
                btnCopyCurrentURLFav.IsEnabled = false;

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
            if (!url.StartsWith("https://")){
                url = "https://" + url;
            }
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
            String url = homepageUrl;
            wvMain.Source = (new Uri(url));
            updateTbUrl(url);
            homepageNavigate = true;
        }

        private void btnCopyCurrentURLFav_Click(object sender, RoutedEventArgs e)
        {
            tbAddRemoveURL.Text = tbUrl.Text;
        }

        private void wvMain_NavigationCompleted(Microsoft.UI.Xaml.Controls.WebView2 sender, Microsoft.Web.WebView2.Core.CoreWebView2NavigationCompletedEventArgs args)
        {
            String url = wvMain.Source.ToString();
            tbUrl.Text = url;

            if (!backNavigate && !frontNavigate && !firstLoadFromHistory && !refreshNavigate && !lstHistoryNavigate && !homepageNavigate)
            {
                history.Insert(historyIndex + 1, url);
                historyIndex++;

                updateLstHistory();
            }
            else
            {
                backNavigate = false;
                frontNavigate = false;
                firstLoadFromHistory = false;
                refreshNavigate = false;
                lstHistoryNavigate = false;
                homepageNavigate = false;
            }
        }

        private void updateLstHistory()
        {
            lstHistory.Items.Clear();
            for (int i = 0; i < history.Count; i++)
            {
                lstHistory.Items.Add(history[i]);
            }
        }
        private void wvMain_Loaded(object sender, RoutedEventArgs e)
        {
            LoadSaveData();
            LoadSavedHistoryData();
            LoadSavedSettingsData();
        }

        private void btnSaveFavorites_Click(object sender, RoutedEventArgs e)
        {
            saveFavoritesFile();
        }

        public async Task<String> saveSettingsFile()
        {
            Windows.Storage.StorageFolder storageFolder =
                    Windows.Storage.ApplicationData.Current.LocalFolder;
            Windows.Storage.StorageFile savedHomepageFile =
                    await storageFolder.CreateFileAsync("savedHomepage.txt",
                    Windows.Storage.CreationCollisionOption.ReplaceExisting);
            String data = getSettingsSaveData();

            await Windows.Storage.FileIO.WriteTextAsync(savedHomepageFile, data);

            return data;
        }

        public String getSettingsSaveData()
        {
            String save = "";
            Color bgColor = (Grid.Background as SolidColorBrush).Color;
            int rInt = bgColor.R;
            int gInt = bgColor.G;
            int bInt = bgColor.B;
            save += rInt + "\n";
            save += gInt + "\n";
            save += bInt + "\n";
            save += homepageUrl;

            return save;
        }

        public async Task<String> saveFavoritesFile()
        {
            Windows.Storage.StorageFolder storageFolder =
                    Windows.Storage.ApplicationData.Current.LocalFolder;
            Windows.Storage.StorageFile savedFavoritesFile =
                    await storageFolder.CreateFileAsync("savedFavorites.txt",
                    Windows.Storage.CreationCollisionOption.ReplaceExisting);
            String data = getFavoritesSaveData();

            await Windows.Storage.FileIO.WriteTextAsync(savedFavoritesFile, data);

            return data;
        }

        public String getFavoritesSaveData()
        {
            String favoritesString = "";
            for(int i = 0; i < favorites.Count; i++)
            {
                favoritesString += favorites[i].ToString() + "\n";
            }
            return favoritesString.Substring(0, favoritesString.Length - 1);
        }

        private void btnSaveHistory_Click(object sender, RoutedEventArgs e)
        {
            saveHistoryFile();
        }

        public async Task<String> saveHistoryFile()
        {
            Windows.Storage.StorageFolder storageFolder =
                    Windows.Storage.ApplicationData.Current.LocalFolder;
            Windows.Storage.StorageFile savedHistoryFile =
                    await storageFolder.CreateFileAsync("savedHistory.txt",
                    Windows.Storage.CreationCollisionOption.ReplaceExisting);
            String data = getHistorySaveData();

            await Windows.Storage.FileIO.WriteTextAsync(savedHistoryFile, data);

            return data;
        }

        public String getHistorySaveData()
        {
            String historyString = "";
            for (int i = 0; i < history.Count; i++)
            {
                historyString += history[i].ToString() + "\n";
            }
            return historyString.Substring(0, historyString.Length - 1);
        }

        private async void LoadSaveData()
        {
            try
            {
                Windows.Storage.StorageFolder storageFolder =
                        Windows.Storage.ApplicationData.Current.LocalFolder;
                Windows.Storage.StorageFile savedFavoritesFile =
                    await storageFolder.GetFileAsync("savedFavorites.txt");

                string favoritesText = await Windows.Storage.FileIO.ReadTextAsync(savedFavoritesFile);

                String[] favoritesString = favoritesText.Split('\n');

                if (!favoritesString.Contains("https://www.nasa.gov/"))
                {
                    favorites.Insert(0, "https://www.nasa.gov/");
                }
                if (!favoritesString.Contains("https://www.amazon.com/"))
                {
                    favorites.Insert(0, "https://www.amazon.com/");
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

            if (!favorites.Contains("https://www.nasa.gov/"))
            {
                favorites.Insert(0, "https://www.nasa.gov/");
            }
            if (!favorites.Contains("https://www.amazon.com/"))
            {
                favorites.Insert(0, "https://www.amazon.com/");
            }

            favorites.Remove("");
        }

        private async void LoadSavedHistoryData()
        {
            try
            {
                Windows.Storage.StorageFolder storageFolder =
                    Windows.Storage.ApplicationData.Current.LocalFolder;
                Windows.Storage.StorageFile savedHistoryFile =
                    await storageFolder.GetFileAsync("savedHistory.txt");
                string historyText = await Windows.Storage.FileIO.ReadTextAsync(savedHistoryFile);
                String[] historyString = historyText.Split('\n');

                foreach (var line in historyString)
                {
                    history.Add(line.Trim());
                }
            }
            catch 
            { 
            }
            history.Remove("");

            historyIndex = history.Count - 1;

            if (historyIndex >= 0)
            {
                wvMain.Source = new Uri(history[historyIndex]);
                bool firstLoadFromHistory = true;
            }
        }

        private async void LoadSavedSettingsData()
        {
            try
            {
                Windows.Storage.StorageFolder storageFolder =
                    Windows.Storage.ApplicationData.Current.LocalFolder;
                Windows.Storage.StorageFile savedHomepageFile =
                    await storageFolder.GetFileAsync("savedHomepage.txt");
                string homepageText = await Windows.Storage.FileIO.ReadTextAsync(savedHomepageFile);

                String[] settingsString = homepageText.Split('\n');

                Color bgColor = Color.FromArgb(255, (byte) Int32.Parse(settingsString[0]), (byte)Int32.Parse(settingsString[1]), (byte)Int32.Parse(settingsString[2]));
                Grid.Background = new SolidColorBrush(bgColor);

                homepageUrl = (settingsString[3]);
            }
            catch
            {
            }

            if (homepageUrl.Equals("") || !(Uri.IsWellFormedUriString(homepageUrl, UriKind.Absolute)))
            {
                homepageUrl = "https://www.yahoo.com/";
            }
        }

        private void btnClearHistory_Click(object sender, RoutedEventArgs e)
        {
            history = new List<string>();
            historyIndex = -1;
            lstHistory.Items.Clear();
        }

        private void btnClearFavorites_Click(object sender, RoutedEventArgs e)
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
            favorites = new List<string>();

            favorites.Add("https://www.amazon.com/");
            favorites.Add("https://www.nasa.gov/");

            for (int i = 0; i < favorites.Count; i++)
            {
                createFavButton(i);
            }
        }

        private void ListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            wvMain.Source = new Uri(sender.ToString());
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            wvMain.Reload();
            refreshNavigate = true;
        }

        private void lstHistory_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (lstHistory.SelectedItem != null)
            {
                wvMain.Source = new Uri(lstHistory.SelectedItem.ToString());
                lstHistoryNavigate = true;
            }
        }

        private void btnShowHistory_Click(object sender, RoutedEventArgs e)
        {
            if (showLstHistory)
            {
                lstHistory.Opacity = 0;
                lstHistory.IsEnabled = false;
                showLstHistory = false;
            }
            else
            {
                lstHistory.Opacity = 100;
                lstHistory.IsEnabled = true;
                showLstHistory = true;
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
        private void btnChangeBg_Click(object sender, RoutedEventArgs e)
        {
            String r = tbBgColor.Text.Substring(1, 2);
            String g = tbBgColor.Text.Substring(3, 2);
            String b = tbBgColor.Text.Substring(5, 2);
            int rInt = 0;
            int gInt = 0;
            int bInt = 0;
            try
            {
                rInt = (int)Convert.ToInt64(r, 16);
                gInt = (int)Convert.ToInt64(g, 16);
                bInt = (int)Convert.ToInt64(b, 16);
            }
            catch 
            {
                rInt = 0;
                gInt = 0;
                bInt = 0;
            }

            Color bgColor = Color.FromArgb(255, (byte) rInt, (byte) gInt, (byte) bInt);
            Grid.Background = new SolidColorBrush(bgColor);
        }

        private void btnCopyHomepage_Click(object sender, RoutedEventArgs e)
        {
            tbHomepage.Text = tbUrl.Text;
        }

        private void btnSetHomepage_Click(object sender, RoutedEventArgs e)
        {
            homepageUrl = tbHomepage.Text;
        }

        private void btnSettings_Click(object sender, RoutedEventArgs e)
        {
            if (settingsBar)
            {
                wvMain.Width += 240;
                settingsBar = false;
                wvMain.Margin = new Thickness(0, 50, 0, 0);

                btnChangeBg.Opacity = 0;
                btnChangeBg.IsEnabled = false;
                tbBgColor.Opacity = 0;
                tbBgColor.IsEnabled = false;
                tbHomepage.Opacity = 0;
                tbHomepage.IsEnabled = false;
                btnCopyHomepage.Opacity = 0;
                btnCopyHomepage.IsEnabled = false;
                btnSetHomepage.Opacity = 0;
                btnSetHomepage.IsEnabled = false;
                btnSaveSettings.Opacity = 0;
                btnSaveSettings.IsEnabled = false;

            }
            else
            {
                wvMain.Width -= 240;
                settingsBar = true;
                wvMain.Margin = new Thickness(240, 50, 0, 0);

                btnChangeBg.Opacity = 100;
                btnChangeBg.IsEnabled = true;
                tbBgColor.Opacity = 100;
                tbBgColor.IsEnabled = true;
                tbHomepage.Opacity = 100;
                tbHomepage.IsEnabled = true;
                btnCopyHomepage.Opacity = 100;
                btnCopyHomepage.IsEnabled = true;
                btnSetHomepage.Opacity = 100;
                btnSetHomepage.IsEnabled = true;
                btnSaveSettings.Opacity = 100;
                btnSaveSettings.IsEnabled = true;
            }
        }

        private void btnSaveSettings_Click(object sender, RoutedEventArgs e)
        {
            saveSettingsFile();
        }
    }
}
